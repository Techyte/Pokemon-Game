using System.Collections;
using PokemonGame.Dialogue;

namespace PokemonGame.Battle
{
    using System.Collections.Generic;
    using Game.Party;
    using General;
    using Global;
    using ScriptableObjects;
    using UnityEngine;

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
                    Destroy(value);
                }
            }
        }

        private void Awake()
        {
            Singleton = this;
        }

        [Header("UI:")]
        [SerializeField] private BattleUIManager uiManager;

        [Space] 
        
        [Header("Assignments")] 
        [SerializeField] private ExperienceCalculator expCalculator;

        [SerializeField] private TextAsset battlerUsedText;

        [Space]
        [Header("Main Readouts")]
        public int currentBattlerIndex;

        public int opponentBattlerIndex;

        [Space]
        [Header("Other Readouts")]
        [SerializeField] public TurnStatus currentTurn = TurnStatus.Choosing;
        
        public BattleParty playerParty;
        
        public BattleParty opponentParty;
        
        [SerializeField] private EnemyAI enemyAI;
        
        [SerializeField] private Move playerMoveToDo;
        private int playerMoveToDoIndex;
        
        public Move enemyMoveToDo;
        
        [SerializeField] private bool playerHasChosenMove;
        
        [SerializeField] private bool hasDoneChoosingUpdate;
        
        [SerializeField] private bool hasSetupShowing;

        private Battler playerCurrentBattler => playerParty[currentBattlerIndex];

        private Battler opponentCurrentBattler => opponentParty[opponentBattlerIndex];

        public List<TurnItem> turnItemQueue = new List<TurnItem>();
        [SerializeField] private bool _currentlyRunningQueueItem = false;

        public List<Battler> battlersThatParticipated;

        private string _opponentName;

        private Vector3 _playerPos;
        private Quaternion _playerRotation;

        private bool _availableToEndTurnShowing;
        private bool _waitingToEndTurnEnding;

        private bool _playerWantsToSwap;
        private int _playerSwapIndex;

        private bool _opponentDefeated;
        private bool _endingDialogueRunning;

        private bool _playerSwappedThisTurn;
        private bool _playerChoseToSwap;
        
        private void Start()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            //Loads relevant info like the opponent and player party
            playerParty = new BattleParty(SceneLoader.GetVariable<Party>("playerParty"));
            opponentParty = new BattleParty(SceneLoader.GetVariable<Party>("opponentParty"));
            enemyAI = SceneLoader.GetVariable<EnemyAI>("enemyAI");
            _opponentName = SceneLoader.GetVariable<string>("opponentName");
            _playerPos = SceneLoader.GetVariable<Vector3>("playerPosition");
            _playerRotation = SceneLoader.GetVariable<Quaternion>("playerRotation");

            currentBattlerIndex = 0;
            opponentBattlerIndex = 0;

            playerParty.PartyAllDefeated += (sender, args) =>
            {
                SomeoneDefeated(false);
            };
            opponentParty.PartyAllDefeated += (sender, args) =>
            {
                SomeoneDefeated(true);
            };

            DialogueManager.instance.DialogueEnded += (sender, args) =>
            {
                bool swapped = false;
                
                if (_playerWantsToSwap)
                {
                    swapped = true;
                    Debug.Log(currentTurn);
                    PlayerSwappedBattler();
                }
                
                if (_currentlyRunningQueueItem && !args.moreToGo && !swapped)
                {
                    TurnQueueItemEnded();
                }
                
                if (_endingDialogueRunning)
                {
                    ExitBattle(_opponentDefeated);
                }
            };

            for (int i = 0; i < opponentParty.Count; i++)
            {
                int index = i;
                
                opponentParty[index].OnFainted += (sender, args) =>
                {
                    BattlerFainted(args, opponentParty[index]);
                };
            }
            
            // adds current battler to list of participating battlers
            battlersThatParticipated.Add(playerCurrentBattler);
        }

        private void ClearTurnQueue()
        {
            turnItemQueue.Clear();
        }

        public void BattlerFainted(BattlerTookDamageArgs e, Battler defeated)
        {
            Debug.Log(defeated);
            
            int exp = expCalculator.GetExperienceFromDefeatingBattler(defeated, true, battlersThatParticipated.Count);

            foreach (Battler battler in battlersThatParticipated)
            {
                battler.exp += exp;
            }
        }

        private void Update()
        {
            if (playerHasChosenMove)
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
                    if (!hasDoneChoosingUpdate)
                    {
                        Debug.Log("Begin Turn Choosing");
                        uiManager.ShowControlUI(true);
                        uiManager.ShowUI(true);
                        uiManager.UpdateBattlerMoveDisplays();
                        enemyAI.AIMethod(new AIMethodEventArgs(opponentCurrentBattler, opponentParty));
                        hasDoneChoosingUpdate = true;
                        Debug.Log("Setting swapped to false");
                        _playerSwappedThisTurn = false;
                        _playerChoseToSwap = false;
                    }
                    break;
            }
        }

        private void TurnShowing()
        {
            if (!hasSetupShowing)
            {
                Debug.Log("Starting Turn Showing");
                hasSetupShowing = true;
                
                uiManager.ShowControlUI(false);
                
                turnItemQueue.Add(TurnItem.StartDelay);
                if (_playerChoseToSwap)
                {
                    turnItemQueue.Add(TurnItem.PlayerSwap);
                }
                turnItemQueue.Add(TurnItem.StartOfTurnStatusEffects);
                QueueMoves();
                turnItemQueue.Add(TurnItem.EndOfTurnStatusEffects);
            }

            if (!_currentlyRunningQueueItem)
            {
                if (turnItemQueue.Count > 0)
                {
                    Debug.Log("Running a new turn item");
                    
                    _currentlyRunningQueueItem = true;
                    
                    TurnItem nextTurnItem = turnItemQueue[0];
                    turnItemQueue.RemoveAt(0);

                    switch (nextTurnItem)
                    {
                        case TurnItem.StartDelay:
                            StartCoroutine(TurnStartDelay());
                            break;
                        case TurnItem.PlayerMove:
                            DoPlayerMove();
                            break;
                        case TurnItem.OpponentMove:
                            DoEnemyMove();
                            break;
                        case TurnItem.EndBattlePlayerWin:
                            BeginEndBattleDialogue(true);
                            break;
                        case TurnItem.EndBattleOpponentWin:
                            BeginEndBattleDialogue(false);
                            break;
                        case TurnItem.PlayerSwapBecauseFainted:
                            BeginSwapPlayerBattler();
                            break;
                        case TurnItem.PlayerSwap:
                            PlayerSwappedBattler();
                            break;
                        case TurnItem.OpponentSwap:
                            break;
                        case TurnItem.StartOfTurnStatusEffects:
                            RunStartOfTurnStatusEffects();
                            break;
                        case TurnItem.EndOfTurnStatusEffects:
                            RunEndOfTurnStatusEffects();
                            break;
                    }
                }
                else
                {
                    EndTurnShowing();
                }
            }
        }

        private IEnumerator TurnStartDelay()
        {
            yield return new WaitForSeconds(1);
            TurnQueueItemEnded();
        }

        private void TurnQueueItemEnded()
        {
            _currentlyRunningQueueItem = false;
        }

        private void EndTurnShowing()
        {
            playerHasChosenMove = false;
            currentTurn = TurnStatus.Ending;
        }

        private void DialogueHurt(string battlerUsed, string moveUsed, string battlerHit, string damageDealt)
        {
            Dictionary<string, string> variables = new Dictionary<string, string>();
            variables.Add("battlerUsed", battlerUsed);
            variables.Add("moveUsed", moveUsed);
            variables.Add("battlerHit", battlerHit);
            variables.Add("damageDealt", damageDealt);
            
            QueDialogue(battlerUsedText, true, variables);
        }

        private void TurnEnding()
        {
            if (!_waitingToEndTurnEnding)
            {
                Debug.Log("Ending Turn");
                hasDoneChoosingUpdate = false;
                hasSetupShowing = false;
                playerHasChosenMove = false;
                
                enemyMoveToDo = null;
                playerMoveToDo = null;

                _waitingToEndTurnEnding = true;
                EndTurnEnding();
            }
        }

        private void EndTurnEnding()
        {
            _waitingToEndTurnEnding = false;
            currentTurn = TurnStatus.Choosing;
        }

        //Public method used by the move UI buttons
        public void ChooseMove(int moveID)
        {
            playerMoveToDo = playerCurrentBattler.moves[moveID];
            playerMoveToDoIndex = moveID;
            playerHasChosenMove = true;
        }

        public void ChooseToSwap(int newBattlerIndex)
        {
            if (_currentlyRunningQueueItem)
            {
                _playerWantsToSwap = true;
                _playerSwapIndex = newBattlerIndex;
                QueDialogue($"You sent out {playerParty[newBattlerIndex].name}", true);
            }
            else
            {
                _playerSwapIndex = newBattlerIndex;
                playerHasChosenMove = true;
                _playerChoseToSwap = true;
            }
        }

        private void BeginSwapPlayerBattler()
        {
            uiManager.SwitchBattlerBecauseOfDeath();
        }

        private void PlayerSwappedBattler()
        {
            currentBattlerIndex = _playerSwapIndex;
            
            AddParticipatedBattler(playerParty[_playerSwapIndex]);

            uiManager.UpdatePlayerBattlerDetails();
            
            _playerWantsToSwap = false;
            
            _playerSwappedThisTurn = true;
            
            QueDialogue($"Go ahead {playerCurrentBattler.name}!", true);
        }

        public void AddParticipatedBattler(Battler battlerToParticipate)
        {
            if (!battlersThatParticipated.Contains(battlerToParticipate))
            {
                battlersThatParticipated.Add(battlerToParticipate);
            }
        }

        private void DoPlayerMove()
        {
            //You can add any animation calls for attacking here
            
            MoveMethodEventArgs e = new MoveMethodEventArgs(playerCurrentBattler, opponentCurrentBattler,
                playerMoveToDoIndex, playerMoveToDo, ExternalBattleData.Construct(this));
            
            playerMoveToDo.MoveMethod(e);
            
            opponentCurrentBattler.TakeDamage(e.damageDealt, new BattlerDamageSource(playerCurrentBattler));

            DialogueHurt(playerCurrentBattler.name, playerMoveToDo.name, opponentCurrentBattler.name,
                e.damageDealt.ToString());
            
            opponentParty.CheckDefeatedStatus();
        }

        private void QueueMoves()
        {
            if (_playerSwappedThisTurn || turnItemQueue.Contains(TurnItem.PlayerSwap))
            {
                turnItemQueue.Add(TurnItem.OpponentMove);

                return;
            }
            
            if(playerCurrentBattler.speed > opponentCurrentBattler.speed)
            {
                //Player is faster
                turnItemQueue.Add(TurnItem.PlayerMove);
                turnItemQueue.Add(TurnItem.OpponentMove);
            }
            else
            {
                //Enemy is faster
                turnItemQueue.Add(TurnItem.OpponentMove);
                turnItemQueue.Add(TurnItem.PlayerMove);
            }
        }

        private void DoEnemyMove()
        {
            //You can add any animation calls for attacking here

            int moveToDoIndex = GetIndexOfMoveOnCurrentEnemy(enemyMoveToDo);

            MoveMethodEventArgs e = new MoveMethodEventArgs(opponentCurrentBattler, playerCurrentBattler, moveToDoIndex,
                enemyMoveToDo, ExternalBattleData.Construct(this));
            
            enemyMoveToDo.MoveMethod(e);
            
            playerCurrentBattler.TakeDamage(e.damageDealt, new BattlerDamageSource(opponentCurrentBattler));
            
            DialogueHurt(opponentCurrentBattler.name, enemyMoveToDo.name, playerCurrentBattler.name,
                e.damageDealt.ToString());
            
            playerParty.CheckDefeatedStatus();
            
            if (playerCurrentBattler.isFainted)
            {
                PlayerBattlerDied();
            }
        }

        private void PlayerBattlerDied()
        {
            uiManager.UpdatePlayerBattlerDetails();

            turnItemQueue.RemoveAll(item => item == TurnItem.PlayerMove);
            
            turnItemQueue.Add(TurnItem.PlayerSwapBecauseFainted);
        }

        private int GetIndexOfMoveOnCurrentEnemy(Move move)
        {
            for (int i = 0; i < opponentCurrentBattler.moves.Count; i++)
            {
                if (opponentCurrentBattler.moves[i] == move)
                {
                    return i;
                }
            }

            Debug.LogWarning($"Could not find move {move.name} on the current opponent battler");
            return -1;
        }
        
        private int GetIndexOfMoveOnCurrentPlayer(Move move)
        {
            for (int i = 0; i < playerCurrentBattler.moves.Count; i++)
            {
                if (playerCurrentBattler.moves[i] == move)
                {
                    return i;
                }
            }

            Debug.LogWarning($"Could not find move {move.name} on the current player battler");
            return -1;
        }

        private void RunEndOfTurnStatusEffects()
        {
            bool anyEffects = false;
            
            if (!playerCurrentBattler.isFainted)
            {
                foreach (var trigger in playerCurrentBattler.statusEffect.triggers)
                {
                    if (trigger.trigger == StatusEffectCaller.EndOfTurn)
                    {
                        trigger.EffectEvent.Invoke(new StatusEffectEventArgs(playerCurrentBattler));
                        anyEffects = true;
                    }
                }
            }

            if (!opponentCurrentBattler.isFainted)
            {
                foreach (var trigger in opponentCurrentBattler.statusEffect.triggers)
                {
                    if (trigger.trigger == StatusEffectCaller.EndOfTurn)
                    {
                        trigger.EffectEvent.Invoke(new StatusEffectEventArgs(opponentCurrentBattler));
                        anyEffects = true;
                    }
                }
            }
            
            if (!anyEffects)
            {
                TurnQueueItemEnded();
            }
        }

        private void RunStartOfTurnStatusEffects()
        {
            bool anyEffects = false;

            if (!playerCurrentBattler.isFainted)
            {
                foreach (var trigger in playerCurrentBattler.statusEffect.triggers)
                {
                    if (trigger.trigger == StatusEffectCaller.StartOfTurn)
                    {
                        trigger.EffectEvent.Invoke(new StatusEffectEventArgs(playerCurrentBattler));
                        anyEffects = true;
                    }
                }
            }

            if (!opponentCurrentBattler.isFainted)
            {
                foreach (var trigger in opponentCurrentBattler.statusEffect.triggers)
                {
                    if (trigger.trigger == StatusEffectCaller.StartOfTurn)
                    {
                        trigger.EffectEvent.Invoke(new StatusEffectEventArgs(opponentCurrentBattler));
                        anyEffects = true;
                    }
                }
            }

            if (!anyEffects)
            {
                TurnQueueItemEnded();
            }
        }

        private void ExitBattle(bool isDefeated)
        {
            Debug.Log("ending the battle");
            
            Dictionary<string, object> vars = new Dictionary<string, object>
            {
                { "playerParty", playerParty },
                { "trainerName", _opponentName },
                { "playerPos", _playerPos },
                { "playerRotation", _playerRotation },
                { "isDefeated", isDefeated }
            };

            SceneLoader.LoadScene("Game", vars);
        }

        private void SomeoneDefeated(bool isDefeated)
        {
            if (isDefeated)
            {
                turnItemQueue.Insert(0, TurnItem.EndBattlePlayerWin);
            }
            else
            {
                turnItemQueue.Insert(0, TurnItem.EndBattleOpponentWin);
            }
        }
        
        private void BeginEndBattleDialogue(bool isDefeated)
        {
            _opponentDefeated = isDefeated;
            
            if (isDefeated)
            {
                QueDialogue("All opponent Pokemon defeated!", true);
            }
            else
            {
                QueDialogue("All your Pokemon defeated!", true);
            }

            _endingDialogueRunning = true;
        }
    }
}