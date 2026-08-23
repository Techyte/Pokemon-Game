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
        private void RunAwayDialogue()
        {
            QueDialogue("Running Away!", DialogueBoxType.Event, "run");
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