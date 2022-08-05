using UnityEngine;
using Random = UnityEngine.Random;

namespace PokemonGame.Battle
{
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
        public GameObject playerUIHolder;
        
        [SerializeField] private BattleUIManager uiManager;

        [Space]
        [Header("Assignments")]
        public int currentBattlerIndex;

        public int opponentBattlerIndex;

        [Space]
        [Header("Other Readouts")]
        [SerializeField] public TurnStatus currentTurn = TurnStatus.Choosing;
        
        [SerializeField] public Party playerParty;
        
        [SerializeField] public Party opponentParty;
        
        [SerializeField] private EnemyAI _enemyAI;
        
        [SerializeField] private Move _playerMoveToDo;
        
        public Move enemyMoveToDo;
        
        [SerializeField] private bool playerHasChosenAttack;
        
        [SerializeField] private bool hasDoneChoosingUpdate;
        
        [SerializeField] private bool hasShowedMoves;
        

        private Vector3 playerSpawnPos;
        private int opponentId;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            //Loads relevant info like the opponent and player party
            playerParty = (Party)SceneLoader.vars[0];
            opponentParty = (Party)SceneLoader.vars[1];
            _enemyAI = (EnemyAI)SceneLoader.vars[2];
            playerSpawnPos = (Vector3)SceneLoader.vars[3];
            opponentId = (int)SceneLoader.vars[4];

            SceneLoader.ClearLoader();

            currentBattlerIndex = 0;
            opponentBattlerIndex = 0;
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
                        _enemyAI.AIMethod(new AIMethodEventArgs(opponentParty.party[opponentBattlerIndex], opponentParty));
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
            opponentParty.party[opponentBattlerIndex].statusEffect.Effect(new StatusEffectEventArgs(
                    opponentParty.party[opponentBattlerIndex]));

            playerParty.party[currentBattlerIndex].statusEffect.Effect(new StatusEffectEventArgs(
                    playerParty.party[currentBattlerIndex]));
            
            uiManager.UpdateHealthDisplays();
            CheckForWinCondition();
        }

        //Public method used by the move UI buttons
        public void ChooseMove(int moveID)
        {
            _playerMoveToDo = playerParty.party[currentBattlerIndex].moves[moveID];
            playerHasChosenAttack = true;
        }

        private void DoPlayerMove()
        {
            //You can add any animation calls for attacking here

            if (_playerMoveToDo)
            {
                if (_playerMoveToDo.category == MoveCategory.Status)
                {
                    _playerMoveToDo.MoveMethod(new MoveMethodEventArgs(opponentParty.party[opponentBattlerIndex]));
                }
                else
                {
                    int damageToDo = CalculateDamage(_playerMoveToDo, playerParty.party[currentBattlerIndex], opponentParty.party[opponentBattlerIndex]);
                    opponentParty.party[opponentBattlerIndex].TakeDamage(Mathf.RoundToInt(damageToDo));
                }
            }

            uiManager.UpdateHealthDisplays();
            CheckForWinCondition();
        }

        private int CalculateDamage(Move move, Battler battlerThatUsed, Battler battlerBeingAttacked)
        {
            //Checking to see if the move is capable of hitting the opponent battler
            foreach (var hType in move.type.cantHit)
            {
                if (hType == battlerBeingAttacked.primaryType || hType == battlerBeingAttacked.secondaryType)
                {
                    Debug.Log(move.type + " can't hit that battler");
                    return 0;
                }
            }

            float type = 1;

            //Calculating type disadvantages
            foreach (var weakType in move.type.weakAgainst)
            {
                if (weakType == battlerBeingAttacked.primaryType)
                {
                    type /= 2;
                }
                if (weakType == battlerBeingAttacked.secondaryType)
                {
                    type /= 2;
                }
            }

            //Calculating type advantages
            foreach (var strongType in move.type.strongAgainst)
            {
                if (strongType == battlerBeingAttacked.primaryType)
                {
                    type *= 2;
                }
                if (strongType == battlerBeingAttacked.secondaryType)
                {
                    type *= 2;
                }
            }

            //Failsafe
            if (type > 4)
                type = 4;
            if (type < .25f)
                type = .25f;

            //STAB =  Same type attack bonus
            int stab = 1;
            if (move.type == battlerThatUsed.primaryType)
            {
                stab = 2;
            }

            //Damage calculation is correct (took me way to long to get it right) source: https://bulbapedia.bulbagarden.net/wiki/Damage#Generation_II
            int damage = move.category == MoveCategory.Physical
                ? Mathf.RoundToInt(((2 * battlerThatUsed.level / 5 + 2) * move.damage *
                    (battlerThatUsed.attack / battlerBeingAttacked.defense) / 50 + 2) * stab * type)
                : Mathf.RoundToInt(((2 * battlerThatUsed.level / 5 + 2) * move.damage *
                    (battlerThatUsed.specialAttack / battlerBeingAttacked.specialDefense) / 50 + 2) * stab * type);

            int randomness = Mathf.RoundToInt(Random.Range(.8f * damage, damage * 1.2f));
            damage = randomness;

            return damage;
        }

        private void DoMoves()
        {
            if(playerParty.party[currentBattlerIndex].speed > opponentParty.party[opponentBattlerIndex].speed)
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
        }

        private void DoEnemyMove()
        {
            //You can add any animation calls for attacking here

            if (enemyMoveToDo.category == MoveCategory.Status)
            {
                enemyMoveToDo.MoveMethod(new MoveMethodEventArgs(opponentParty.party[currentBattlerIndex]));
            }
            else
            {
                float damageToDo = CalculateDamage(enemyMoveToDo, opponentParty.party[opponentBattlerIndex], playerParty.party[currentBattlerIndex]);
                playerParty.party[currentBattlerIndex].TakeDamage(Mathf.RoundToInt(damageToDo));
            }

            if (playerParty.party[currentBattlerIndex].isFainted)
            {
                uiManager.SwitchBattlerBecauseOfDeath();
            }
            
            CheckForWinCondition();

            uiManager.UpdateHealthDisplays();
        }

        private void EndBattle(bool isDefeated)
        {
            var playerPath = Application.persistentDataPath + "/party.json";
            var opponentPath = Application.persistentDataPath + "/opponentTestParty.json";

            SaveAndLoad<Party>.SaveJson(playerParty, playerPath);
            SaveAndLoad<Party>.SaveJson(opponentParty, opponentPath);

            object[] vars = { SaveAndLoad<Party>.LoadJson(playerPath), SaveAndLoad<Party>.LoadJson(opponentPath), playerSpawnPos, true, opponentId, isDefeated };

            SceneLoader.LoadScene(0, vars);
        }

        private void CheckForWinCondition()
        {
            //Counting how many fainted batlers in the players party
            var playerFaintedPokemon = 0;
            var playerPartyCount = 0;
            foreach (var partyPokemon in playerParty.party)
            {
                if (partyPokemon)
                {
                    playerPartyCount++;
                    if (partyPokemon.isFainted)
                        playerFaintedPokemon++;
                }
            }
            
            if (playerFaintedPokemon == playerPartyCount)
            {
                Debug.Log("Battle ended because player lost all battlers");
                EndBattle(true);
            }

            //Counting how many fainted batlers in the opponent party
            int enemyFaintedPokemon = 0;
            int enemyPartyCount = 0;
            foreach (var partyPokemon in opponentParty.party)
            {
                if (partyPokemon)
                {
                    enemyPartyCount++;
                    if (partyPokemon.isFainted)
                        enemyFaintedPokemon++;
                }
            }

            if (enemyFaintedPokemon == enemyPartyCount)
            {
                Debug.Log("Battle ended because enemy lost all battlers");
                EndBattle(true);
            }
        }
    }
}