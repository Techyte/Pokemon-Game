using System;
using System.Collections.Generic;
using PokemonGame.Dialogue;
using Riptide;
using PokemonGame.General;
using PokemonGame.Networking;
using PokemonGame.ScriptableObjects;
using UnityEngine;

namespace PokemonGame.Battle
{
    public class PlayerBattleController : MonoBehaviour
    {
        public static PlayerBattleController Instance;

        private void Awake()
        {
            Instance = this;
        }

        [SerializeField] private Battle battle;
        [SerializeField] private BattleUIManager uiManager;

        // temporary stored variables for level up screen
        private GameObject _currentLevelUpObj;
        [SerializeField] private LevelUpDisplay levelUpDisplayPrefab;

        private void ShowBattlerLeveled(OnLevelUpEventArgs args)
        {
            LevelUpDisplay display = Instantiate(levelUpDisplayPrefab, FindFirstObjectByType<Canvas>().transform);
            _currentLevelUpObj = display.gameObject;
            display.Init(args.oldStats, args.newStats);
        }

        private void OnEnable()
        {
            DialogueManager.instance.OnDialogueEnded += InstanceOnOnDialogueEnded;
        }

        private void InstanceOnOnDialogueEnded(object sender, DialogueEndedEventArgs e)
        {
            switch (e.id)
            {
                case "leveledUp":
                    OnLevelUpEventArgs args = battle.PlayerOneBattler.InitiateLevelUp();
                    ShowBattlerLeveled(args);
                    uiManager.UpdatePlayerOneBattlerDetails();
                    break;
            }
        }

        private void OnDisable()
        {
            DialogueManager.instance.OnDialogueEnded -= InstanceOnOnDialogueEnded;
        }

        public void FinishedViewingLevelUpScreen()
        {
            Destroy(_currentLevelUpObj);
        }

        //Public method used by the move UI buttons
        public void ChooseMove(int moveID)
        {
            if (battle.localPlayerOne)
            {
                battle.PlayerChooseMove(0, 0, moveID, new List<(int, int)>
                {
                    (0, 0)
                });
            }
            else
            {
                ClientSendPlayerMoveSelected(moveID);
            }
            uiManager.ShowControlUI(false);
        }
        
        public void StartPickingBattlerToUseItemOn(Item item)
        {
            if (item.lockedTarget)
            {
                battle.PlayerUseItem(0, 0, item, item.targetIndex, item.userParty);
            }
            else
            {
                uiManager.OpenUseItemOnBattler(item);
                uiManager.UpdateItemBattlerButtons();
            }
        }

        public void PlayerUseItem(Item item, int targetIndex, bool userParty)
        {
            battle.PlayerUseItem(0, 0, item, item.targetIndex, item.userParty);
        }

        public void PlayerPickedPokeBall(PokeBall ball)
        {
            battle.PlayerUseItem(0, 0, ball, ball.targetIndex, ball.userParty);
        }

        public void PlayerChooseToSwap(int battlerIndex)
        {
            if (battle.localPlayerOne)
            {
                battle.PlayerChooseToSwap(battlerIndex);
            }
            else
            {
                ClientSendPlayerSwapSelected(battlerIndex);
            }
            uiManager.ShowControlUI(false);
        }

        private void ClientSendPlayerMoveSelected(int moveIndex)
        {
            Message message = Message.Create(MessageSendMode.Reliable, ClientToServerMessageId.MoveSelected);
            message.AddInt(moveIndex);

            BattleNetworkManager.Instance.Client.Send(message);
        }

        private void ClientSendPlayerSwapSelected(int battlerIndex)
        {
            Message message = Message.Create(MessageSendMode.Reliable, ClientToServerMessageId.SwapSelected);
            message.AddInt(battlerIndex);

            BattleNetworkManager.Instance.Client.Send(message);
        }
    }
}