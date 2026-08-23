using System;
using System.Linq;
using PokemonGame.Game;
using PokemonGame.Networking;

namespace PokemonGame.Battle
{
    using System.Collections.Generic;
    using System.Collections;
    using Game.Party;
    using General;
    using Global;
    using ScriptableObjects;
    using UnityEngine;
    using Dialogue;

    public enum TurnStatus
    {
        Choosing,
        Showing,
        Ending
    }

    /// <summary>
    /// The main class that manages battles
    /// </summary>
    public class Battle : DialogueTrigger
    {
        private static Battle _singleton;
        public static Battle Singleton
        {
            get => _singleton;
            private set
            {
                if (_singleton == null)
                    _singleton = value;
                else if (_singleton != value)
                {
                    Debug.Log($"{nameof(Battle)} instance already exists, destroying duplicate!");
                }
            }
        }

        private void Awake()
        {
            Singleton = this;
            Initialise();
        }
        
        [Space]
        [Header("Assignments")] 
        [SerializeField] private float shrinkEffectDelay;

        [Space]
        [Header("Readouts")]
        [SerializeField] public TurnStatus currentTurn = TurnStatus.Choosing;

        public List<Player> players; // list of participating players
        public List<List<Battler>> activeBattlers; // list of actively in-use pokemon, eg; [0][0] gets player ones first slot pokemon in play
        
        [SerializeField] private EnemyAI enemyAI;
        
        /// <summary>
        /// making sure we don't run the inital choosing logic more than once
        /// </summary>
        private bool hasDoneChoosingUpdate;
        
        /// <summary>
        /// making sure we don't run the inital showing logic more than once
        /// </summary>
        private bool hasSetupShowing;

        [SerializeField] private List<BattleEvent> turnSequence;
        
        /// <summary>
        /// the list of battlers that participated on the players team during this battle
        /// </summary>
        public List<Battler> playerOneBattlersThatParticipated;
        
        // is this battle a trainer or wild battle
        public bool trainerBattle;

        /// <summary>
        /// List of actions players intend to use this turn
        /// </summary>
        public List<List<BattleAction>> playerActions;

        public List<VisibleBattleAction> visibleActions;
        
        // public events
        public EventHandler<int> OnNewTurnState;
        public EventHandler OnNewTurnItem;
        public EventHandler OnBattlerEvolved;
        public EventHandler<int> OnPlayerPickedAction;
        public EventHandler<bool> OnCatchAttempt;
        public EventHandler<int> OnSwapBecauseFainted;
        public EventHandler<int> OnChangeBattler;
        public EventHandler<int> OnStartChangeBattlerIndex;
        public EventHandler<int> OnPlayerMove;
        
        // events
        private EventHandler<int> _playerOneBattlerLeveledUp = null;
        private EventHandler<EvolutionData> _playerOneBattlerEvolved = null;

        public bool onlineBattle;

        public int battlersEach;

        private void Initialise()
        {
            LoadStartingVariables();

            for (int i = 0; i < players.Count; i++)
            {
                Player player = players[i];
                
                activeBattlers.Add(new List<Battler>());
                
                int assigned = 0;
                
                for (int j = 0; j < player.Party.Count; j++)
                {
                    if (!player.Party[j].isFainted)
                    {
                        activeBattlers[i].Add(player.Party[j]);
                        assigned++;
                        if (assigned >= battlersEach)
                        {
                            break;
                        }
                    }
                }
            }
            
            if (!onlineBattle)
            {
                ResetParticipatingBattlers();
            }
            
            Instantiate(Resources.Load("Pokemon Game/Transitions/SpikyOpen"));
        }

        private void LoadStartingVariables()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
            players = new List<Player>();

            onlineBattle = SceneLoader.GetVariable<bool>("online");
            battlersEach = SceneLoader.GetVariable<int>("battlersEach");
            if (!onlineBattle)
            {
                trainerBattle = SceneLoader.GetVariable<bool>("trainerBattle");
                
                if (trainerBattle)
                {
                    enemyAI = SceneLoader.GetVariable<EnemyAI>("enemyAI");
                }
                
                players = SceneLoader.GetVariable<List<Player>>("players");
            }
            else
            {
                // all of this is assuming we can trust all the clients players list, if they dont match there will be problems
                int index = 0;
                foreach (var player in BattleNetworkManager.Instance.Players.Values)
                {
                    Player battlePlayer = new Player(player, index);
                    players.Add(battlePlayer);
                    index++;
                }
            }
        }
        
        private IEnumerator ShowMove(MoveMethodEventArgs args)
        {
            bool queuedDialogue = false;
            EventHandler<DialogueQueuedEventArgs> dialogueQueued = (sender, eventArgs) =>
            {
                queuedDialogue = true;
            };

            DialogueManager.instance.OnDialogueQueued += dialogueQueued;
            
            DialogueMoveEffectiveness(args);

            if (!args.missed)
            {
                args.move.MoveMethod(args);
                args.movePPData.MoveWasUsed();
            }

            if (!args.success)
            {
                QueDialogue("But it failed!", DialogueBoxType.Event, "generalFinishing");
            }
            
            yield return new WaitForSeconds(1);
            
            // call anyway because it clears the force stop condition
            StartDialogue();
            
            DialogueManager.instance.OnDialogueQueued -= dialogueQueued;
        }

        private void Update()
        {
            switch (currentTurn)
            {
                case TurnStatus.Ending:
                    TurnEnding();
                    break;
                case TurnStatus.Showing:
                    TurnShowing();
                    break;
                case TurnStatus.Choosing:
                    TurnChoosing();
                    break;
            }
        }

        private void TurnChoosing()
        {
            if (!hasDoneChoosingUpdate)
            {
                // clear list of actions
                playerActions = new List<List<BattleAction>>();
                
                // create actions for each player
                for (int i = 0; i < players.Count; i++)
                {
                    playerActions.Add(new List<BattleAction>());
                    // create a possible action for each of the battlers they are entitled to
                    for (int j = 0; j < battlersEach; j++)
                    {
                        playerActions[i].Add(null);
                    }
                }
                
                OnNewTurnState?.Invoke(this, 0);
                if (trainerBattle && !onlineBattle)
                {
                    enemyAI.AIMethod(this, 1);
                }
                else if(!onlineBattle)
                {
                    EnemyAIMethods.WildPokemon(this, 1);
                }
                hasDoneChoosingUpdate = true;
            }
        }
        
        private void TurnShowing()
        {
            if (!hasSetupShowing)
            {
                hasSetupShowing = true;
                OnNewTurnState?.Invoke(this, 1);

                if (IsNotOnlineHost())
                {
                    return;
                }
            }
            
            if (IsNotOnlineHost())
            {
                return;
            }
        }

        public void AddVisibleBattleAction(VisibleBattleAction action)
        {
            visibleActions.Add(action);
        }

        public void AddVisibleBattleAction(VisibleBattleActionType type, List<object> variables)
        {
            visibleActions.Add(new VisibleBattleAction(type, variables));
        }
        
        private void DialogueMoveEffectiveness(MoveMethodEventArgs e)
        {
            if (e.move.category != MoveCategory.Status)
            {
                switch (e.effectiveIndex)
                {
                    case 1:
                        QueDialogue("Its Not Very Effective...", DialogueBoxType.Event, "generalFinishing");
                        break;
                    case 2:
                        QueDialogue("Its Super Effective!", DialogueBoxType.Event, "generalFinishing");
                        break;
                    case 3:
                        QueDialogue($"{e.target.name} is immune!", DialogueBoxType.Event, "generalFinishing");
                        break;
                }
            }

            if (e.crit)
            {
                QueDialogue("A Critical Hit!", DialogueBoxType.Event, "generalFinishing");
            }

            if (e.missed)
            {
                QueDialogue($"But it missed!", DialogueBoxType.Event, "generalFinishing");
            }
        }

        private void TurnEnding()
        {
            OnNewTurnState?.Invoke(this, 2);
            hasDoneChoosingUpdate = false;
            hasSetupShowing = false;
            
            EndTurnEnding();
        }

        private void EndTurnEnding()
        {
            currentTurn = TurnStatus.Choosing;
        }

        private void PickPlayerAction(int playerIndex, int actionIndex, BattleAction action)
        {
            playerActions[playerIndex][actionIndex] = action;
            OnPlayerPickedAction?.Invoke(this, playerIndex);
            CheckReadyToSimulate();
        }

        private void CheckReadyToSimulate()
        {
            foreach (var actions in playerActions)
            {
                foreach (var action in actions)
                {
                    if (action == null)
                    {
                        return;
                    }
                }
            }
            
            // all players have submitted all their entitled actions
            currentTurn = TurnStatus.Showing;
        }

        public void PlayerChooseMove(int playerIndex, int actionIndex, int moveID, List<(int, int)> targets)
        {
            PickPlayerAction(playerIndex, actionIndex, new BattleAction(BattleActionType.Move, new List<object>()
            {
                targets,
                moveID,
            }));
        }
        
        private void EvolutionEffect(Battler evolvedBattler)
        {
            if (evolvedBattler == playerOneCurrentBattler)
            {
                // shrinks the battler so we only call it when its the current battler
                OnBattlerEvolved?.Invoke(this, EventArgs.Empty);
                StartCoroutine(DelayEvolution(evolvedBattler));
            }
            else
            {
                evolvedBattler.EvolutionApproved();
            }
        }
        
        public void PlayerUseItem(int index, int actionIndex, Item item, int battlerToUseOn, bool useOnUserParty)
        {
            PickPlayerAction(index, actionIndex, new BattleAction(BattleActionType.Item, new List<object>()
            {
                item, // item to use
                useOnUserParty ? 0 : 1, // player to target
                battlerToUseOn // battler to target
            }));
        }

        public void SetPlayerActiveBattlers(int playerId, int switchingOut, int switchingIn)
        {
            activeBattlers[playerId][switchingOut] = players[playerId].Party[switchingIn];
            
            AddVisibleBattleAction(VisibleBattleActionType.Switch, new List<object>{
                playerId,
                switchingOut,
                switchingIn
            });
        }

        private IEnumerator DelayEvolution(Battler battlerToEvolve)
        {
            yield return new WaitForSeconds(shrinkEffectDelay);
            battlerToEvolve.EvolutionApproved();
            OnChangeBattler?.Invoke(this, 0);
        }

        private void ResetParticipatingBattlers()
        {
            playerOneBattlersThatParticipated.Clear();
            Debug.Log($"Resetting participating battlers to {playerOneBattlersThatParticipated.Count}");
            for (int i = 0; i < activeBattlers[0].Count; i++)
            {
                AddParticipatedBattler(activeBattlers[0][i]);
            }
        }

        public void AddParticipatedBattler(Battler battlerToParticipate)
        {
            if (!onlineBattle)
            {
                if (!playerOneBattlersThatParticipated.Contains(battlerToParticipate))
                {
                    playerOneBattlersThatParticipated.Add(battlerToParticipate);
                }
            }
        }

        public void DoPlayerOneMove(Move playerOneMoveToDo)
        {
            bool missed = false;

            // actually has like a usable accuracy
            if (playerOneMoveToDo.accuracy != 0 && !Mathf.Approximately(playerOneMoveToDo.accuracy, 1))
            {
                float accuracy = StatStages.GetMultiplierFromStage(playerOneCurrentBattler.modifierStats.accuracyStage, true, false);
                float evasiveness = StatStages.GetMultiplierFromStage(playerTwoCurrentBattler.modifierStats.evasionStage, true, true);
                missed = Random.Range(1, 101) > playerOneMoveToDo.accuracy * accuracy * evasiveness * 100;
            }
            
            int moveToDoIndex = GetIndexOfMovePlayerOne(playerOneMoveToDo);
            
            MoveMethodEventArgs e = new MoveMethodEventArgs(playerOneCurrentBattler, playerTwoCurrentBattler, playerOneMoveToDo, 
                playerOneCurrentBattler.movePpInfos[moveToDoIndex], ExternalBattleData.Construct(this));

            if (!missed)
            {
                e.damageDealt = MovesMethods.CalculateDamage(playerOneMoveToDo, PlayerOneBattler, PlayerTwoBattler,
                    out e.effectiveIndex, out e.crit);
            }

            e.missed = missed;
            
            if (onlineBattle)
            {
                BattleNetworkManager.Instance.ServerSendTurnPlayerMove(true, e);
            }
            PlayAuthoritativeMove(e, true);
            
            OnPlayerMove?.Invoke(this, 0);
        }
        
        public void DoPlayerTwoMove(Move playerTwoMoveToDo)
        {
            bool missed = false;

            // actually has like a usable accuracy
            if (playerTwoMoveToDo.accuracy != 0 && !Mathf.Approximately(playerTwoMoveToDo.accuracy, 1))
            {
                float accuracy = StatStages.GetMultiplierFromStage(playerTwoCurrentBattler.modifierStats.accuracyStage, true, false);
                float evasiveness = StatStages.GetMultiplierFromStage(playerOneCurrentBattler.modifierStats.evasionStage, true, true);
                missed = Random.Range(1, 101) > playerTwoMoveToDo.accuracy * accuracy * evasiveness * 100;
            }
            
            int moveToDoIndex = GetIndexOfMovePlayerTwo(playerTwoMoveToDo);
            
            MoveMethodEventArgs e = new MoveMethodEventArgs(playerTwoCurrentBattler, playerOneCurrentBattler,
                playerTwoMoveToDo, playerTwoCurrentBattler.movePpInfos[moveToDoIndex], ExternalBattleData.Construct(this));

            if (!missed)
            {
                e.damageDealt = MovesMethods.CalculateDamage(playerTwoMoveToDo, PlayerTwoBattler, PlayerOneBattler,
                    out e.effectiveIndex, out e.crit);
            }
            
            e.missed = missed;
            
            if (onlineBattle)
            {
                BattleNetworkManager.Instance.ServerSendTurnPlayerMove(false, e);
            }
            PlayAuthoritativeMove(e, false);
            
            OnPlayerMove?.Invoke(this, 1);
        }

        private void PlayerOneBattlerFainted(object sender, BattlerTookDamageArgs args)
        {
            Debug.Log("Player One Fainted");
            QueDialogue($"{GetPlayerOneName()} {playerOneCurrentBattler.name} Fainted!", DialogueBoxType.Event, "playerOneFainted");
            
            partyOne.CheckDefeatedStatus();
            
            if (!partyOne.defeated)
            {
                QueueTurnItem(TurnItemType.PlayerSwapBecauseFainted, new List<object>
                {
                    0
                });
            }
            
            turnItemQueue.RemoveAll(item => item.Type == TurnItemType.PlayerMove && (int)item.Variables[0] == 0);
            
            if (!onlineBattle)
            {
                playerOneBattlersThatParticipated.Remove(playerOneCurrentBattler);
            }
        }
        
        public void PlayerTwoBattlerFainted(object sender, BattlerTookDamageArgs args)
        {
            Debug.Log("Player Two Fainted");
            QueDialogue($"{GetPlayerTwoName()} {playerTwoCurrentBattler.name} Fainted!", DialogueBoxType.Event, "playerTwoFainted");
            
            if (!onlineBattle)
            {
                int exp = ExperienceCalculator.GetExperienceFromDefeatingBattler(playerTwoCurrentBattler, playerOneCurrentBattler, true,
                    playerOneBattlersThatParticipated.Count);
                
                foreach (Battler battler in playerOneBattlersThatParticipated)
                {
                    if (!battler.isFainted)
                    {
                        QueDialogue($"{battler.name} gained {exp} experience points", DialogueBoxType.Event, "generalFinishing");
                        battler.GainExp(exp);
                    }
                }
            }
            
            partyTwo.CheckDefeatedStatus();
            
            if (!partyTwo.defeated)
            {
                QueueTurnItem(TurnItemType.PlayerSwapBecauseFainted, new List<object>
                {
                    1
                });
            }
            turnItemQueue.RemoveAll(item => item.Type == TurnItemType.PlayerMove && (int)item.Variables[0] == 1);
            
            if (!onlineBattle)
            {
                playerOneCurrentBattler.EVs.maxHealth += playerTwoCurrentBattler.source.GetYields().maxHealth;
                playerOneCurrentBattler.EVs.attack += playerTwoCurrentBattler.source.GetYields().attack;
                playerOneCurrentBattler.EVs.defense += playerTwoCurrentBattler.source.GetYields().defense;
                playerOneCurrentBattler.EVs.specialAttack += playerTwoCurrentBattler.source.GetYields().specialAttack;
                playerOneCurrentBattler.EVs.specialDefense += playerTwoCurrentBattler.source.GetYields().specialDefense;
                playerOneCurrentBattler.EVs.speed += playerTwoCurrentBattler.source.GetYields().speed;
            }
        }
        
        public void RunFromBattle()
        {
            PickPlayerOneAction(new BattleAction(BattleActionType.Run));
        }

        private void RunAwayDialogue()
        {
            QueDialogue("Running Away!", DialogueBoxType.Event, "run");
        }

        public int GetIndexOfMovePlayerTwo(Move move)
        {
            for (int i = 0; i < playerTwoCurrentBattler.moves.Count; i++)
            {
                if (playerTwoCurrentBattler.moves[i] == move)
                {
                    return i;
                }
            }

            Debug.LogWarning($"Could not find move {move.name} on the current playerTwo battler");
            return -1;
        }
        
        public int GetIndexOfMovePlayerOne(Move move)
        {
            for (int i = 0; i < playerOneCurrentBattler.moves.Count; i++)
            {
                if (playerOneCurrentBattler.moves[i] == move)
                {
                    return i;
                }
            }

            Debug.LogWarning($"Could not find move {move.name} on the current player battler");
            return -1;
        }

        private void BattlerSwapped(object sender, int e)
        {
            if (currentTurnItem.Type == TurnItemType.PlayerEvolved)
            {
                return;
            }
            
            if (e == 0)
            {
                PlayerOneEffectMethods(EffectTrigger.EnterBattleSelf);
            }
            else
            {
                PlayerTwoEffectMethods(EffectTrigger.EnterBattleSelf);
            }
        }

        private void EffectMethods(EffectTrigger trigger)
        {
            bool queuedDialogue = false;
            EventHandler<DialogueQueuedEventArgs> dialogueQueued = (sender, eventArgs) =>
            {
                queuedDialogue = true;
            };
            
            DialogueManager.instance.OnDialogueQueued += dialogueQueued;

            PlayerOneEffectMethods(trigger, false);
            PlayerTwoEffectMethods(trigger, false);
            
            DialogueManager.instance.OnDialogueQueued -= dialogueQueued;

            if (!queuedDialogue)
            {
                TurnQueueItemEnded();
            }
        }

        private void PlayerOneEffectMethods(EffectTrigger trigger, bool callEnd = true)
        {
            bool queuedDialogue = false;
            EventHandler<DialogueQueuedEventArgs> dialogueQueued = (sender, eventArgs) =>
            {
                queuedDialogue = true;
            };
            
            DialogueManager.instance.OnDialogueQueued += dialogueQueued;

            if (!playerOneCurrentBattler.isFainted)
            {
                if (playerOneCurrentBattler.statusEffect)
                {
                    foreach (var effectTrigger in playerOneCurrentBattler.statusEffect.triggers)
                    {
                        if (effectTrigger.trigger == trigger)
                        {
                            effectTrigger.EffectEvent.Invoke(new StatusEffectEventArgs(playerOneCurrentBattler));
                        }
                    }
                }

                if (playerOneCurrentBattler.ability)
                {
                    foreach (var abilityTrigger in playerOneCurrentBattler.ability.triggers)
                    {
                        if (abilityTrigger.trigger == trigger)
                        {
                            abilityTrigger.effectEvent.Invoke(new AbilityEventArgs(playerOneCurrentBattler));
                        }
                    }
                }
            }
            
            partyOne.CheckDefeatedStatus();
            partyTwo.CheckDefeatedStatus();
            
            DialogueManager.instance.OnDialogueQueued -= dialogueQueued;

            if (!queuedDialogue && callEnd)
            {
                TurnQueueItemEnded();
            }
        }

        private void PlayerTwoEffectMethods(EffectTrigger trigger, bool callEnd = true)
        {
            bool queuedDialogue = false;
            EventHandler<DialogueQueuedEventArgs> dialogueQueued = (sender, eventArgs) =>
            {
                queuedDialogue = true;
            };
            
            DialogueManager.instance.OnDialogueQueued += dialogueQueued;

            if (!playerTwoCurrentBattler.isFainted)
            {
                if (playerTwoCurrentBattler.statusEffect)
                {
                    foreach (var effectTrigger in playerTwoCurrentBattler.statusEffect.triggers)
                    {
                        if (effectTrigger.trigger == trigger)
                        {
                            effectTrigger.EffectEvent.Invoke(new StatusEffectEventArgs(playerTwoCurrentBattler));
                        }
                    }
                }

                if (playerTwoCurrentBattler.ability)
                {
                    foreach (var abilityTrigger in playerTwoCurrentBattler.ability.triggers)
                    {
                        if (abilityTrigger.trigger == trigger)
                        {
                            abilityTrigger.effectEvent.Invoke(new AbilityEventArgs(playerTwoCurrentBattler));
                        }
                    }
                }
            }
            
            partyOne.CheckDefeatedStatus();
            partyTwo.CheckDefeatedStatus();
            
            DialogueManager.instance.OnDialogueQueued -= dialogueQueued;

            if (!queuedDialogue && callEnd)
            {
                TurnQueueItemEnded();
            }
        }
        
        public void RunStartOfTurnEffects()
        {
            EffectMethods(EffectTrigger.StartOfTurn);
        }

        private IEnumerator ExitBattleWin()
        {
            yield return new WaitForSeconds(0.5f);
            
            Dictionary<string, object> vars = new Dictionary<string, object>
            {
                { "partyOne", partyOne },
                { "trainerName", _playerTwoName },
                { "isDefeated", true },
                { "trainerBattle", trainerBattle}
            };
            
            Instantiate(Resources.Load("Pokemon Game/Transitions/SpikyClose"));
            yield return new WaitForSeconds(0.4f);
            
            SceneLoader.LoadScene("Game", vars);
        }

        private IEnumerator ExitBattleLoss()
        {
            yield return new WaitForSeconds(0.5f);
            
            Dictionary<string, object> vars = new Dictionary<string, object>
            {
                { "partyOne", partyOne },
                { "trainerName", _playerTwoName },
                { "isDefeated", false },
                { "loaderName", "ForcedHealPoint" },
                { "trainerBattle", trainerBattle}
            };
            
            Instantiate(Resources.Load("Pokemon Game/Transitions/SpikyClose"));
            yield return new WaitForSeconds(0.4f);
            
            SceneLoader.LoadScene("Poke Center", vars);
        }

        private void SomeoneDefeated(bool isDefeated)
        {
            if (isDefeated)
            {
                QueueTurnItem(TurnItemType.EndBattle, new List<object>
                {
                    0
                });
            }
            else
            {
                QueueTurnItem(TurnItemType.EndBattle, new List<object>
                {
                    1
                });
            }
        }
        
        public void BeginEndBattleDialogue(bool isDefeated)
        {
            if (isDefeated)
            {
                if (trainerBattle)
                {
                    QueDialogue($"All {GetPlayerTwoName()} Pokemon defeated!", DialogueBoxType.Event, "playerTwoDefeated");
                }
                else
                {
                    StartCoroutine(ExitBattleWin());
                }
            }
            else
            {
                QueDialogue($"All {GetPlayerOneName()} Pokemon fainted, running!", DialogueBoxType.Event, "playerDefeated");
            }
            
            turnItemQueue.Clear();
        }

        private string GetPlayerOneName()
        {
            if (IsNotOnlineHost())
            {
                return "Opponent";
            }
            return "Your";
        }

        private string GetPlayerTwoName()
        {
            if (IsNotOnlineHost())
            {
                return "Your";
            }
            return "Opponent";
        }


        private bool IsNotOnlineHost()
        {
            if (onlineBattle)
            {
                return !BattleNetworkManager.Instance.IsHost;
            }

            return false;
        }

        private bool IsOnlineHost()
        {
            if (onlineBattle)
            {
                return BattleNetworkManager.Instance.IsHost;
            }

            return false;
        }
    }
}