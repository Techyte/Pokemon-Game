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
        public List<BattleAction> playerActions;

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

            ChangePlayerTwoBattlerIndex(0, true);
            
            if (!onlineBattle)
            {
                ResetParticipatingBattlers();
            }
            
            HookEvents();
            
            Instantiate(Resources.Load("Pokemon Game/Transitions/SpikyOpen"));
        }

        private void LoadStartingVariables()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            //Loads relevant info like the playerTwo and player party
            onlineBattle = SceneLoader.GetVariable<bool>("online");
            trainerBattle = SceneLoader.GetVariable<bool>("trainerBattle");
            if (trainerBattle)
            {
                enemyAI = SceneLoader.GetVariable<EnemyAI>("enemyAI");
                _playerTwoName = SceneLoader.GetVariable<string>("opponentName");
            }

            if (onlineBattle)
            {
                localPlayerOne = BattleNetworkManager.Instance.IsHost;
                List<NetworkPlayer> players = BattleNetworkManager.Instance.Players.Values.ToList();
                partyOne = new BattleParty(players[0].Party);
                partyTwo = new BattleParty(players[1].Party);
            }
            else
            {
                partyOne = new BattleParty(SceneLoader.GetVariable<Party>("partyOne"));
                partyTwo = new BattleParty(SceneLoader.GetVariable<Party>("partyTwo"));
                localPlayerOne = true;
            }
        }

        private void HookEvents()
        {
            DialogueManager.instance.OnDialogueEnded += DialogueEnded;
            partyOne.PartyAllDefeated += PlayerOnePartyAllDefeated;
            partyTwo.PartyAllDefeated += PlayerTwoPartyAllDefeated;
            
            OnChangeBattler += BattlerSwapped;

            if (!onlineBattle)
            {
                _playerOneBattlerLeveledUp = (s, e) => BattlerLeveledUp(partyOne.party.Find(x => x == (Battler)s), e);
                _playerOneBattlerEvolved = (s, e) => BattlerEvolved(partyOne.party.Find(x => x == (Battler)s), e.evolution);
            }
            
            for (int i = 0; i < partyOne.Count; i++)
            {
                partyOne[i].OnCanLevelUp += _playerOneBattlerLeveledUp;
                partyOne[i].OnFainted += PlayerOneBattlerFainted;
                partyOne[i].OnCanEvolve += _playerOneBattlerEvolved;
            }
            
            for (int i = 0; i < partyTwo.Count; i++)
            {
                partyTwo[i].OnFainted += PlayerTwoBattlerFainted;
            }
        }

        private void OnDisable()
        {
            partyOne.PartyAllDefeated -= PlayerOnePartyAllDefeated;
            partyTwo.PartyAllDefeated -= PlayerTwoPartyAllDefeated;
            DialogueManager.instance.OnDialogueEnded -= DialogueEnded;
            
            OnChangeBattler -= BattlerSwapped;
            
            for (int i = 0; i < partyOne.Count; i++)
            {
                partyOne[i].OnCanLevelUp -= _playerOneBattlerLeveledUp;
                partyOne[i].OnFainted -= PlayerOneBattlerFainted;
                partyOne[i].OnCanEvolve -= _playerOneBattlerEvolved;
            }
            
            for (int i = 0; i < partyTwo.Count; i++)
            {
                partyTwo[i].OnFainted -= PlayerTwoBattlerFainted;
            }
        }

        private void PlayerOnePartyAllDefeated(object sender, EventArgs e)
        {
            SomeoneDefeated(false);
        }

        private void PlayerTwoPartyAllDefeated(object sender, EventArgs e)
        {
            SomeoneDefeated(true);
        }

        private void BattlerLevelUpEvent(string newBattlerName, int newLevel)
        {
            QueDialogue($"{newBattlerName} reached level {newLevel}!", DialogueBoxType.Narration, "leveledUp");
        }

        private void BattlerLeveledUp(Battler battlerThatLeveled, int newLevel)
        {
            Debug.Log("Queuing player level up");
            
            InsertTurnItem(TurnItemType.PlayerLevelUp, new List<object>()
            {
                battlerThatLeveled,
                newLevel
            });
        }

        private void BattlerEvolved(Battler battlerThatEvolved, BattlerTemplate newTemplate)
        {
            int location = PlanningOnEndingTheBattle() ? turnItemQueue.Count - 1 : turnItemQueue.Count;
            
            InsertTurnItem(TurnItemType.PlayerEvolved, location, new List<object>()
            {
                battlerThatEvolved,
                newTemplate
            });
        }

        private void BattlerEvolvedEvent(Battler battlerThatEvolved, BattlerTemplate newTemplate)
        {
            QueDialogue($"{battlerThatEvolved.name} wants to evolve!", DialogueBoxType.Narration, "evolved");
            QueDialogue($"{battlerThatEvolved.name} evolved into a {newTemplate.name}!", DialogueBoxType.Narration, "generalFinishing");
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
            
            // if during the showing we need to display some more dialogue
            if (!queuedDialogue)
            {
                TurnQueueItemEnded();
            }
            
            DialogueManager.instance.OnDialogueQueued -= dialogueQueued;
        }

        private void Update()
        {
            if (playerOneAction != null && playerTwoAction != null)
            {
                currentTurn = TurnStatus.Showing;
            }
            
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
                OnNewTurnState?.Invoke(this, 0);
                if (trainerBattle && !onlineBattle)
                {
                    enemyAI.AIMethod(new AIMethodEventArgs(playerTwoCurrentBattler, partyTwo,
                        ExternalBattleData.Construct(this)));
                }
                else if(!onlineBattle)
                {
                    EnemyAIMethods.WildPokemon(new AIMethodEventArgs(playerTwoCurrentBattler, partyTwo,
                        ExternalBattleData.Construct(this)));
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

        private IEnumerator TurnStartDelay()
        {
            yield return new WaitForSeconds(1);
            EndTurnItem();
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

        private void PickPlayerOneAction(BattleAction action)
        {
            playerActions[0] = action;
            OnPlayerPickedAction?.Invoke(this, 0);
        }

        private void PickPlayerTwoAction(BattleAction action)
        {
            playerActions[1] = action;
            OnPlayerPickedAction?.Invoke(this, 1);
        }

        public void PlayerOneChooseMove(int moveID)
        {
            PickPlayerOneAction(new BattleAction(BattleActionType.Move, new List<object>()
            {
                0,
                moveID,
            }));
        }

        public void PlayerTwoChooseMove(int moveID)
        {
            PickPlayerTwoAction(new BattleAction(BattleActionType.Move, new List<object>()
            {
                1,
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
        
        public void PlayerOneUseItem(Item item, int battlerToUseOn, bool useOnUserParty)
        {
            PickPlayerOneAction(new BattleAction(BattleActionType.Item, new List<object>()
            {
                item, // item to use
                useOnUserParty ? 0 : 1, // player to target
                battlerToUseOn // battler to target
            }));
        }
        
        public void PlayerTwoUseItem(Item item, int battlerToUseOn, int useOnUserParty)
        {
            PickPlayerTwoAction(new BattleAction(BattleActionType.Item, new List<object>()
            {
                1,
                item,
                battlerToUseOn,
                useOnUserParty
            }));
        }
        
        public void PlayerOnePickedPokeBall(PokeBall ball)
        {
            PickPlayerOneAction(new TurnItem(TurnItemType.CatchAttempt, new List<object>()
            {
                ball
            }));
            Bag.Used(ball);
        }

        private void CatchAttempt()
        {
            QueDialogue($"Threw a pokeball at {playerTwoCurrentBattler.name}!", DialogueBoxType.Event);

            if (ExperienceCalculator.Captured(playerTwoCurrentBattler, playerOneCurrentBattler, (PokeBall)playerOneAction.Variables[0]))
            {
                QueDialogue($"Caught {playerTwoCurrentBattler.name}!", DialogueBoxType.Event, "generalFinishing");
                PartyManager.AddBattler(playerTwoCurrentBattler);
                InsertTurnItem(TurnItemType.EndBattle, new List<object>
                {
                    0
                });
                OnCatchAttempt?.Invoke(this, true);
            }
            else
            {
                QueDialogue($"Failed to catch {playerTwoCurrentBattler.name}!", DialogueBoxType.Event, "generalFinishing");
                OnCatchAttempt?.Invoke(this, false);
            }
        }

        private void PlayerOneUseItemEvent(Item itemToUse, int battlerToUseOn, int useOnUserParty)
        {
            Battler battlerBeingUsedOn = useOnUserParty == 0 ? partyOne[battlerToUseOn] : partyTwo[battlerToUseOn];
            
            ItemMethodEventArgs e = new ItemMethodEventArgs(battlerBeingUsedOn, itemToUse);
            
            itemToUse.ItemMethod(e);
            
            Bag.Used(itemToUse);
            
            QueDialogue($"{GetPlayerOneName()} used {itemToUse.name} on {battlerBeingUsedOn.name}!", DialogueBoxType.Event, "generalFinishing");

            if (!e.success)
            {
                QueDialogue("But it failed!", DialogueBoxType.Event, "generalFinishing");
            }
        }

        private void PlayerTwoUseItemEvent(Item itemToUse, int battlerToUseOn, int useOnUserParty)
        {
            Battler battlerBeingUsedOn = useOnUserParty == 1 ? partyTwo[battlerToUseOn] : partyOne[battlerToUseOn];
            
            ItemMethodEventArgs e = new ItemMethodEventArgs(battlerBeingUsedOn, itemToUse);
            
            itemToUse.ItemMethod(e);
            
            Bag.Used(itemToUse);
            
            QueDialogue($"{GetPlayerTwoName()} used {itemToUse.name} on {battlerBeingUsedOn.name}!", DialogueBoxType.Event, "generalFinishing");

            if (!e.success)
            {
                QueDialogue("But it failed!", DialogueBoxType.Event, "generalFinishing");
            }
        }

        public void PlayerOneChooseToSwap(int newBattlerIndex)
        {
            if (_currentlyRunningQueueItem) // swapping mid turn showing aka after a battler faints
            {
                currentTurnItem.Variables.Add(newBattlerIndex);
                currentTurnItem.Variables.Add(true); // swaped because of fainted
                if (!onlineBattle)
                {
                    AddParticipatedBattler(partyOne[newBattlerIndex]);
                }
                QueDialogue($"{GetPlayerOneName()} sent out {partyOne[newBattlerIndex].name}", DialogueBoxType.Event, "playerOneSwap");
            
                if (IsOnlineHost())
                {
                    BattleNetworkManager.Instance.ServerSendTurnPlayerSwap(true, newBattlerIndex, true, true);
                }
            }
            else // player chose to swap as their move
            {
                PickPlayerOneAction(new BattleAction(BattleActionType.Switch, new List<object>()
                {
                    0, // battler switching out
                    newBattlerIndex, // battler switching in
                }));
            }
        }
        
        public void PlayerTwoChooseToSwap(int newBattlerIndex)
        {
            if (_currentlyRunningQueueItem) // swapping mid turn showing aka after a battler faints
            {
                currentTurnItem.Variables.Add(newBattlerIndex);
                currentTurnItem.Variables.Add(true); // swapped because of fainted
                QueDialogue($"{GetPlayerTwoName()} sent out {partyTwo[newBattlerIndex].name}", DialogueBoxType.Event, "playerTwoSwap");
                
                if (IsOnlineHost())
                {
                    BattleNetworkManager.Instance.ServerSendTurnPlayerSwap(false, newBattlerIndex, true, true);
                }
            }
            else // player chose to swap as their move
            {
                PickPlayerTwoAction(new BattleAction(BattleActionType.Switch, new List<object>()
                {
                    0, // battler switching out
                    newBattlerIndex, // battler switching in
                }));
            }
        }

        public void BeginSwapPlayerOneBattler()
        {
            OnSwapBecauseFainted?.Invoke(this, 0);
        }

        public void BeginSwapPlayerTwoBattler()
        {
            if (!onlineBattle)
            {
                AISwitchEventArgs e =
                    new AISwitchEventArgs(playerTwoBattlerIndex, partyTwo, ExternalBattleData.Construct(this));
            
                enemyAI.AISwitchMethod(e);
                    
                PlayerTwoChooseToSwap(e.newBattlerIndex);
            }
            OnSwapBecauseFainted?.Invoke(this, 1);
        }

        private void SetPlayerActiveBattlers(int playerId, int switchingOut, int switchingIn)
        {
            activeBattlers[playerId][switchingOut] = players[playerId].Party[switchingIn];
        }

        private IEnumerator DelayEvolution(Battler battlerToEvolve)
        {
            yield return new WaitForSeconds(shrinkEffectDelay);
            battlerToEvolve.EvolutionApproved();
            OnChangeBattler?.Invoke(this, 0);
        }

        private void ChangePlayerTwoBattlerIndex(int index, bool skipShrink = false)
        {
            playerTwoBattlerIndex = index;
            if (!skipShrink)
            {
                StartCoroutine(DelayChangePlayerTwoBattlerIndex(index));
            }
            
            OnStartChangeBattlerIndex?.Invoke(this, 1);
            
            if (skipShrink)
            {
                FinishedChangingPlayerTwoBattler(index);
            }
        }

        private IEnumerator DelayChangePlayerTwoBattlerIndex(int index)
        {
            yield return new WaitForSeconds(shrinkEffectDelay);
            FinishedChangingPlayerTwoBattler(index);
        }

        private void FinishedChangingPlayerTwoBattler(int newIndex)
        {
            playerTwoDisplayBattlerIndex = newIndex;
            if (!onlineBattle)
            {
                ResetParticipatingBattlers();
            }
            OnChangeBattler?.Invoke(this, 1);
        }

        private void ResetParticipatingBattlers()
        {
            playerOneBattlersThatParticipated.Clear();
            Debug.Log($"Resetting participating battlers to {playerOneBattlersThatParticipated.Count}");
            AddParticipatedBattler(playerOneCurrentBattler);
        }

        public void PlayerOneSwappedBattler(int playerOneSwapIndex, bool becauseFainted)
        {
            ChangePlayerOneBattlerIndex(playerOneSwapIndex, becauseFainted);
            
            if (!onlineBattle)
            {
                AddParticipatedBattler(partyOne[playerOneSwapIndex]);
            }

            if (!becauseFainted)
            {
                QueDialogue($"{GetPlayerOneName()} sent out {partyOne[playerOneSwapIndex].name}", DialogueBoxType.Event);
            }
            QueDialogue($"Go ahead {partyOne[playerOneSwapIndex].name}!", DialogueBoxType.Event, "generalFinishing");
        }

        public void PlayerTwoSwappedBattler(int playerTwoSwapIndex, bool becauseFainted)
        {
            ChangePlayerTwoBattlerIndex(playerTwoSwapIndex, becauseFainted);

            if (!becauseFainted)
            {
                QueDialogue($"{GetPlayerTwoName()} sent out {partyTwo[playerTwoSwapIndex].name}", DialogueBoxType.Event);
            }
            QueDialogue($"Go ahead {partyTwo[playerTwoSwapIndex].name}!", DialogueBoxType.Event, "generalFinishing");
        }

        public void PlayerOneParalysed()
        {
            QueDialogue($"{GetPlayerOneName()} {playerOneCurrentBattler.name} is Paralysed! It is unable to move!", DialogueBoxType.Event, "generalFinishing");
        }

        public void PlayerTwoParalysed()
        {
            QueDialogue($"The {GetPlayerTwoName()} {playerTwoCurrentBattler.name} is Paralysed! It is unable to move!", DialogueBoxType.Event, "generalFinishing");
        }

        public void PlayerOneAsleep()
        {
            QueDialogue($"The {GetPlayerTwoName()} {playerTwoCurrentBattler.name} is Asleep", DialogueBoxType.Event, "generalFinishing");
        }

        public void PlayerTwoAsleep()
        {
            QueDialogue($"The {GetPlayerTwoName()} {playerTwoCurrentBattler.name} is Asleep", DialogueBoxType.Event, "generalFinishing");
        }

        public void MoveMissed()
        {
            QueDialogue($"But it missed!", DialogueBoxType.Event, "generalFinishing");
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

        public void PlayAuthoritativeMove(MoveMethodEventArgs e, bool player)
        {
            if (IsNotOnlineHost())
            {
                if (player)
                {
                    currentTurnItem = new TurnItem(TurnItemType.PlayerMove, new List<object>
                    {
                        0
                    });
                }
                else
                {
                    currentTurnItem = new TurnItem(TurnItemType.PlayerMove, new List<object>
                    {
                        1
                    });
                }
            }
            
            DialogueMoveUsed(e, player);
            currentTurnItem.Variables.Add(e);
        }

        private void QueueMoves()
        {
            if (playerOneAction.Type is not TurnItemType.PlayerMove && playerTwoAction.Type is TurnItemType.PlayerMove)
            {
                AddPlayerTwoMoveToQueue();
                // dont add the player move to queue because they are doing something else

                return;
            }
            
            if (playerOneAction.Type is TurnItemType.PlayerMove && playerTwoAction.Type is not TurnItemType.PlayerMove)
            {
                AddPlayerOneMoveToQueue();
                // dont add the player move to queue because they are doing something else

                return;
            }
            
            if (playerOneAction.Type is not TurnItemType.PlayerMove && playerTwoAction.Type is not TurnItemType.PlayerMove)
            {
                // dont add the players move to queue because they are doing something else
                return;
            }

            float playerAdjustedSpeed = playerOneCurrentBattler.stats.speed * StatStages.GetMultiplierFromStage(playerOneCurrentBattler.modifierStats.speedStage, false, false);
            float playerTwoAdjustedSpeed = playerTwoCurrentBattler.stats.speed * StatStages.GetMultiplierFromStage(playerTwoCurrentBattler.modifierStats.speedStage, false, false);

            if (playerOneCurrentBattler.statusEffect == Registry.GetStatusEffect("Paralysed"))
            {
                playerAdjustedSpeed /= 2;
            }
            
            if (playerTwoCurrentBattler.statusEffect == Registry.GetStatusEffect("Paralysed"))
            {
                playerTwoAdjustedSpeed /= 2;
            }

            Move playerOneMoveToDo = playerOneCurrentBattler.moves[(int)playerOneAction.Variables[1]];
            Move playerTwoMoveToDo = playerTwoCurrentBattler.moves[(int)playerTwoAction.Variables[1]];
            
            if (playerOneMoveToDo.priority == playerTwoMoveToDo.priority)
            {
                if(playerAdjustedSpeed > playerTwoAdjustedSpeed)
                {
                    //PlayerOne BATTLER is faster
                    AddPlayerOneMoveToQueue();
                    AddPlayerTwoMoveToQueue();
                }
                else
                {
                    //Enemy BATTLER is faster
                    AddPlayerTwoMoveToQueue();
                    AddPlayerOneMoveToQueue();
                }
            }
            else
            {
                if(playerOneMoveToDo.priority > playerTwoMoveToDo.priority)
                {
                    //PlayerOne MOVE is faster
                    AddPlayerOneMoveToQueue();
                    AddPlayerTwoMoveToQueue();
                }
                else
                {
                    //Enemy MOVE is faster
                    AddPlayerTwoMoveToQueue();
                    AddPlayerOneMoveToQueue();
                }
            }
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