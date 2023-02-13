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
        public int currentBattlerIndex;

        public int opponentBattlerIndex;

        [Space]
        [Header("Other Readouts")]
        [SerializeField] public TurnStatus currentTurn = TurnStatus.Choosing;
        
        public BattleParty playerParty;
        
        public BattleParty opponentParty;
        
        [SerializeField] private EnemyAI enemyAI;
        
        [SerializeField] private Move playerMoveToDo;
        
        public Move enemyMoveToDo;
        
        [SerializeField] private bool playerHasChosenAttack;
        
        [SerializeField] private bool hasDoneChoosingUpdate;
        
        [SerializeField] private bool hasShowedMoves;

        private Battler playerCurrentBattler => playerParty[currentBattlerIndex];

        private Battler opponentCurrentBattler => opponentParty[opponentBattlerIndex];

        
        private string _opponentName;

        private Vector3 _playerPos;
        private Quaternion _playerRotation;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            //Loads relevant info like the opponent and player party
            playerParty = new BattleParty(PartyManager.Instance.GetParty());
            opponentParty = new BattleParty((Party)SceneLoader.GetVariable("opponentParty"));
            enemyAI = (EnemyAI)SceneLoader.GetVariable("enemyAI");
            _opponentName = (string)SceneLoader.GetVariable("opponentName");
            _playerPos = (Vector3)SceneLoader.GetVariable("playerPosition");
            _playerRotation = (Quaternion)SceneLoader.GetVariable("playerRotation");

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
                        uiManager.ShowUI(true);
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
            DoStatusEffects();
            currentTurn = TurnStatus.Choosing;
        }

        private void DoStatusEffects()
        {
            opponentCurrentBattler.statusEffect.Effect(new StatusEffectEventArgs(
                opponentCurrentBattler));

            playerCurrentBattler.statusEffect.Effect(new StatusEffectEventArgs(
                playerCurrentBattler));
            
            uiManager.UpdateHealthDisplays();
        }

        //Public method used by the move UI buttons
        public void ChooseMove(int moveID)
        {
            playerMoveToDo = playerCurrentBattler.moves[moveID];
            playerHasChosenAttack = true;
        }

        private void DoPlayerMove()
        {
            //You can add any animation calls for attacking here

            playerMoveToDo.MoveMethod(new MoveMethodEventArgs(playerCurrentBattler, opponentCurrentBattler, playerMoveToDo, ExternalBattleData.Construct(this)));

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
            //You can add any animation calls for attacking here

            enemyMoveToDo.MoveMethod(new MoveMethodEventArgs(opponentCurrentBattler, playerCurrentBattler, enemyMoveToDo, ExternalBattleData.Construct(this)));
            
            if (playerCurrentBattler.isFainted)
            {
                uiManager.SwitchBattlerBecauseOfDeath();
            }

            uiManager.UpdateHealthDisplays();
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