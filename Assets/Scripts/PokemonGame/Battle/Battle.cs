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
    public class Battle : MonoBehaviour
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
        public int enemyMoveToDoIndex;
        
        [SerializeField] private bool playerHasChosenAttack;
        
        [SerializeField] private bool hasDoneChoosingUpdate;
        
        [SerializeField] private bool hasShowedMoves;

        private Battler playerCurrentBattler => playerParty[currentBattlerIndex];

        private Battler opponentCurrentBattler => opponentParty[opponentBattlerIndex];

        public List<Battler> battlersThatParticipated;


        private string _opponentName;

        private Vector3 _playerPos;
        private Quaternion _playerRotation;

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
            };
            opponentParty.PartyAllDefeated += (sender, args) =>
            {
                EndBattle(true);
            };

            for (int i = 0; i < opponentParty.Count; i++)
            {
                int newI = i;
                
                opponentParty[newI].OnFainted += (sender, args) =>
                {
                    Debug.Log(opponentParty[newI]);
                    
                    BattlerFainted(args, opponentParty[newI]);
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
                    RunStartOfTurnStatusEffects();
                    if (!hasDoneChoosingUpdate)
                    {
                        uiManager.ShowUI(true);
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
                uiManager.ShowUI(false);
                DoMoves();
                playerHasChosenAttack = false;
                currentTurn = TurnStatus.Ending;
            }
        }

        private void TurnEnding()
        {
            uiManager.ShowUI(false);
            hasDoneChoosingUpdate = false;
            hasShowedMoves = false;
            playerHasChosenAttack = false;
            RunEndOfTurnStatusEffects();
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

            playerMoveToDo.MoveMethod(new MoveMethodEventArgs(playerCurrentBattler, opponentCurrentBattler, playerMoveToDoIndex, playerMoveToDo, ExternalBattleData.Construct(this)));
            
            playerParty.CheckDefeatedStatus();

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

            enemyMoveToDo.MoveMethod(new MoveMethodEventArgs(opponentCurrentBattler, playerCurrentBattler, enemyMoveToDoIndex, enemyMoveToDo, ExternalBattleData.Construct(this)));
            
            playerParty.CheckDefeatedStatus();
            
            if (playerCurrentBattler.isFainted)
            {
                uiManager.SwitchBattlerBecauseOfDeath();
            }

            uiManager.UpdateHealthDisplays();
        }

        private void RunEndOfTurnStatusEffects()
        {
            foreach (var trigger in playerCurrentBattler.statusEffect.triggers)
            {
                if (trigger.trigger == StatusEffectCaller.EndOfTurn)
                {
                    trigger.EffectEvent.Invoke(new StatusEffectEventArgs(playerCurrentBattler));
                }
            }
            
            foreach (var trigger in opponentCurrentBattler.statusEffect.triggers)
            {
                if (trigger.trigger == StatusEffectCaller.EndOfTurn)
                {
                    trigger.EffectEvent.Invoke(new StatusEffectEventArgs(opponentCurrentBattler));
                }
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