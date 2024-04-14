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
        
        [SerializeField] private bool playerHasChosenAttack;
        
        [SerializeField] private bool hasDoneChoosingUpdate;
        
        [SerializeField] private bool hasShowedMoves;

        private Battler playerCurrentBattler => playerParty[currentBattlerIndex];

        private Battler opponentCurrentBattler => opponentParty[opponentBattlerIndex];

        public List<Battler> battlersThatParticipated;

        private string _opponentName;

        private Vector3 _playerPos;
        private Quaternion _playerRotation;

        private bool _waitingToEndTurnShowing;
        private bool _waitingToEndTurnEnding;

        private bool _wantToEnd = false;
        private bool _defeated = false;

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
                EndBattle(false);
                //_wantToEnd = true;
                //_defeated = false;
            };
            opponentParty.PartyAllDefeated += (sender, args) =>
            {
                EndBattle(true);
                //_wantToEnd = true;
                //_defeated = true;
            };

            DialogueManager.instance.DialogueEnded += (sender, args) =>
            {
                if (_waitingToEndTurnShowing && !args.moreToGo)
                {
                    EndTurnShowing();
                }
                if (_waitingToEndTurnEnding && !args.moreToGo)
                {
                    EndTurnEnding();
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
            if (playerHasChosenAttack)
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
                        uiManager.ShowControlUI(true);
                        RunStartOfTurnStatusEffects();
                        uiManager.UpdateBattlerMoveDisplays();
                        enemyAI.AIMethod(new AIMethodEventArgs(opponentCurrentBattler, opponentParty));
                        hasDoneChoosingUpdate = true;
                    }
                    break;
            }
        }

        private void TurnShowing()
        {
            if (!hasShowedMoves)
            {
                hasShowedMoves = true;
                
                uiManager.ShowControlUI(false);
                DoMoves();
                _waitingToEndTurnShowing = true;
            }
        }

        private void EndTurnShowing()
        {
            _waitingToEndTurnShowing = false;
            playerHasChosenAttack = false;
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
                hasDoneChoosingUpdate = false;
                hasShowedMoves = false;
                playerHasChosenAttack = false;

                if (_wantToEnd)
                {
                    EndBattle(_defeated);
                }
                else
                {
                    RunEndOfTurnStatusEffects();
                    _waitingToEndTurnEnding = true;
                }   
            }
        }

        private void EndTurnEnding()
        {
            _waitingToEndTurnEnding = false;
            uiManager.UpdateHealthDisplays();
            currentTurn = TurnStatus.Choosing;
        }

        //Public method used by the move UI buttons
        public void ChooseMove(int moveID)
        {
            playerMoveToDo = playerCurrentBattler.moves[moveID];
            playerMoveToDoIndex = moveID;
            playerHasChosenAttack = true;
        }

        public void AddParticipatedBattler(Battler battlerToParticipate)
        {
            if (!battlersThatParticipated.Contains(battlerToParticipate))
            {
                battlersThatParticipated.Add(battlerToParticipate);
            }
            else
            {
                //Debug.Log("this battler has also been noted down as participating");
            }
        }

        private void DoPlayerMove()
        {
            foreach (var trigger in playerCurrentBattler.statusEffect.triggers)
            {
                if (trigger.trigger == StatusEffectCaller.BeforeMove)
                {
                    trigger.EffectEvent.Invoke(new StatusEffectEventArgs(playerCurrentBattler));
                }
            }
            
            //You can add any animation calls for attacking here
            
            MoveMethodEventArgs e = new MoveMethodEventArgs(playerCurrentBattler, opponentCurrentBattler,
                playerMoveToDoIndex, playerMoveToDo, ExternalBattleData.Construct(this));
                
            playerMoveToDo.MoveMethod(e);
            
            opponentCurrentBattler.TakeDamage(e.damageDealt, new BattlerDamageSource(playerCurrentBattler));

            DialogueHurt(playerCurrentBattler.name, playerMoveToDo.name, opponentCurrentBattler.name,
                e.damageDealt.ToString());
            
            opponentParty.CheckDefeatedStatus();

            uiManager.UpdateHealthDisplays();
        }

        private void DoMoves()
        {
            if(playerCurrentBattler.speed > opponentCurrentBattler.speed)
            {
                //Player is faster
                DoPlayerMove();
                DoEnemyMove();
            }
            else
            {
                //Enemy is faster
                DoEnemyMove();
                DoPlayerMove();
            }

            enemyMoveToDo = null;
            playerMoveToDo = null;
        }

        private void DoEnemyMove()
        {
            foreach (var trigger in opponentCurrentBattler.statusEffect.triggers)
            {
                if (trigger.trigger == StatusEffectCaller.BeforeMove)
                {
                    trigger.EffectEvent.Invoke(new StatusEffectEventArgs(opponentCurrentBattler));
                }
            }
            
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
                uiManager.SwitchBattlerBecauseOfDeath();
            }

            uiManager.UpdateHealthDisplays();
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
            bool anyStatusEffectsUsed = false;
            
            foreach (var trigger in playerCurrentBattler.statusEffect.triggers)
            {
                if (trigger.trigger == StatusEffectCaller.EndOfTurn)
                {
                    trigger.EffectEvent.Invoke(new StatusEffectEventArgs(playerCurrentBattler));
                    anyStatusEffectsUsed = true;
                }
            }
            
            foreach (var trigger in opponentCurrentBattler.statusEffect.triggers)
            {
                if (trigger.trigger == StatusEffectCaller.EndOfTurn)
                {
                    trigger.EffectEvent.Invoke(new StatusEffectEventArgs(opponentCurrentBattler));
                    anyStatusEffectsUsed = true;
                }
            }

            if (!anyStatusEffectsUsed)
            {
                EndTurnEnding();
            }
        }

        private void RunStartOfTurnStatusEffects()
        {
            foreach (var trigger in playerCurrentBattler.statusEffect.triggers)
            {
                if (trigger.trigger == StatusEffectCaller.StartOfTurn)
                {
                    trigger.EffectEvent.Invoke(new StatusEffectEventArgs(playerCurrentBattler));
                }
            }
            
            foreach (var trigger in opponentCurrentBattler.statusEffect.triggers)
            {
                if (trigger.trigger == StatusEffectCaller.StartOfTurn)
                {
                    trigger.EffectEvent.Invoke(new StatusEffectEventArgs(opponentCurrentBattler));
                }
            }
        }

        private void EndBattle(bool isDefeated)
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
    }
}