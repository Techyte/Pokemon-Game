using UnityEngine;
using UnityEngine.SceneManagement;

namespace PokemonGame.Battle
{
    public enum TurnStatus
    {
        Chosing,
        Showing,
        Ending
    }

    public class Battle : MonoBehaviour
    {
        [Header("UI:")]
        public GameObject PlayerUIHolder;
        [SerializeField] private BattleUIManager UIManager;

        [Space]
        [Header("Assignments")]
        public int currentBattlerIndex;

        public int apponentBattlerIndex;

        [Space]
        [Header("Other Readouts")]
        public TurnStatus currentTurn = TurnStatus.Chosing;
        public Party playerParty;
        public Party apponentParty;

        EnemyAI enemyAI;

        private Move playerMoveToDo;
        private Move enemyMoveToDo;
        public bool playerHasChosenAttack;
        [SerializeField]private bool hasDoneChoosingUpdate;
        [SerializeField]private bool hasShowedMoves;
        [SerializeField]private bool hasEnded;

        private void Start()
        {
            playerParty = BattleLoaderInfo.playerParty;
            apponentParty = BattleLoaderInfo.apponentParty;
            enemyAI = BattleLoaderInfo.enemyAI;

            BattleManager.ClearBattleLoader();

            currentBattlerIndex = 0;

            apponentBattlerIndex = 0;

            apponentParty.party[apponentBattlerIndex].currentHealth = apponentParty.party[apponentBattlerIndex].maxHealth;
            playerParty.party[currentBattlerIndex].currentHealth = playerParty.party[currentBattlerIndex].maxHealth;
        }

        private void Update()
        {
            if (playerHasChosenAttack)
            {
                currentTurn = TurnStatus.Showing;
            }

            Debug.Log(currentTurn);
            /*
            switch (currentTurn)
            {
                case TurnStatus.Ending:
                    if (!hasEnded)
                    {
                        TurnEnding();
                        Debug.Log("Ending but the have not ended so you should see this once");
                    }
                    Debug.Log("Ending but we have ended so you shouldnt see this");
                    break;
                case TurnStatus.Showing:
                    TurnShowing();
                    break;
                case TurnStatus.Chosing:
                    if (!hasDoneChoosingUpdate)
                    {
                        UIManager.ShowUI(true);
                        enemyAI.aiMethod(apponentParty.party[apponentBattlerIndex], apponentParty, this);
                        hasDoneChoosingUpdate = true;
                        hasEnded = false;
                    }
                    break;
            }
            */

            if(currentTurn == TurnStatus.Ending)
            {
                if (!hasEnded)
                {
                    TurnEnding();
                    Debug.Log("Ending but the have not ended so you should see this once");
                }
                Debug.Log("Ending but we have ended so you shouldnt see this");
            }else if(currentTurn == TurnStatus.Showing)
            {
                TurnShowing();
            }else if(currentTurn == TurnStatus.Chosing)
            {
                if (!hasDoneChoosingUpdate)
                {
                    UIManager.ShowUI(true);
                    enemyAI.aiMethod(apponentParty.party[apponentBattlerIndex], apponentParty, this);
                    hasDoneChoosingUpdate = true;
                    hasEnded = false;
                }
            }
        }

        private void TurnShowing()
        {
            UIManager.ShowUI(false);
            if (!hasShowedMoves)
            {
                DoMoves();
                Debug.Log("Doing moves so you should only see this once");
            }
            hasShowedMoves = true;
            hasEnded = false;
            currentTurn = TurnStatus.Ending;
            Debug.Log(currentTurn);
        }

        private void TurnEnding()
        {
            UIManager.ShowUI(false);
            hasDoneChoosingUpdate = false;
            hasShowedMoves = false;
            playerHasChosenAttack = false;
            hasEnded = true;
            currentTurn = TurnStatus.Chosing;
        }

        public void ChooseMove(int moveID)
        {
            playerMoveToDo = playerParty.party[currentBattlerIndex].moves[moveID];
            playerHasChosenAttack = true;
        }

        private void DoPlayerMove()
        {
            //You can add any animation calls for attacking here

            if (playerMoveToDo.category == MoveCategory.Status)
            {
                playerMoveToDo.moveMethod(apponentParty.party[apponentBattlerIndex]);
            }
            else
            {
                float damageToDo = CalculateDamage(playerMoveToDo, playerParty.party[currentBattlerIndex], apponentParty.party[apponentBattlerIndex]);
                apponentParty.party[apponentBattlerIndex].currentHealth -= (int)damageToDo;
            }

            if (apponentParty.party[apponentBattlerIndex].currentHealth <= 0)
            {
                apponentParty.party[apponentBattlerIndex].isFainted = true;
            }

            CheckForWinCondition();

            UIManager.UpdateHealthDisplays();
        }

        private float CalculateDamage(Move move, Battler battlerThatUsed, Battler battlerBeingAttacked)
        {
            float finalDamage;

            for (int i = 0; i < move.type.cantHit.Length; i++)
            {
                if (move.type.cantHit[i] == battlerBeingAttacked.primaryType || move.type.cantHit[i] == battlerBeingAttacked.secondaryType)
                {
                    Debug.Log(move.type + " can't hit that battler");
                    return 0;
                }
            }

            float TYPE = 1;
            for (int i = 0; i < move.type.strongAgainst.Length; i++)
            {
                if (move.type.strongAgainst[i] == battlerBeingAttacked.primaryType || move.type.strongAgainst[i] == battlerBeingAttacked.secondaryType)
                {
                    TYPE = 1.5f;
                }
            }

            for (int i = 0; i < move.type.weakAgainst.Length; i++)
            {
                if (move.type.weakAgainst[i] == battlerBeingAttacked.primaryType || move.type.weakAgainst[i] == battlerBeingAttacked.secondaryType)
                {
                    Debug.Log(move.type + " is weak against " + move.type.weakAgainst[i]);
                    if (TYPE == 1.5f)
                    {
                        TYPE = 1;
                    }
                    else
                    {
                        TYPE = .5f;
                    }
                }
            }

            float STAB = 1;
            if (move.type == battlerThatUsed.primaryType)
            {
                STAB = 2;
            }

            if (move.type == battlerThatUsed.secondaryType)
            {
                if (TYPE == 2)
                {
                    TYPE += 2;
                }
                else
                {
                    TYPE = 2;
                }
            }

            float damage = 1;

            //Damage calculation is corect

            switch (move.category)
            {
                case MoveCategory.Physical:
                    damage = ((2 * battlerThatUsed.level / 5 + 2) * move.damage * (battlerThatUsed.attack / battlerBeingAttacked.defense) / 50 + 2) * STAB * TYPE;
                    break;
                case MoveCategory.Special:
                    damage = ((2 * battlerThatUsed.level / 5 + 2) * move.damage * (battlerThatUsed.specialAttack / battlerBeingAttacked.specialDefense) / 50 + 2) * STAB * TYPE;
                    break;
            }

            float RANDOMNESS = 0;
            RANDOMNESS = Mathf.RoundToInt(Random.Range(.8f * damage, damage * 1.2f));
            damage = RANDOMNESS;

            finalDamage = damage;

            return finalDamage;
        }

        private void DoMoves()
        {
            if(apponentParty.party[apponentBattlerIndex].speed > playerParty.party[currentBattlerIndex].speed)
            {
                //Enemy is faster
                DoEnemyMove();
                DoPlayerMove();
            }
            else
            {
                //Player is faster
                DoPlayerMove();
                DoEnemyMove();
            }

            playerParty.party[currentBattlerIndex].statusEffect.effect(playerParty.party[currentBattlerIndex]);
            apponentParty.party[apponentBattlerIndex].statusEffect.effect(apponentParty.party[apponentBattlerIndex]);
            UIManager.UpdateHealthDisplays();
        }

        public void DoMoveOnPlayer(Move move)
        {
            enemyMoveToDo = move;
        }

        private void DoEnemyMove()
        {
            float damageToDo = CalculateDamage(enemyMoveToDo, apponentParty.party[apponentBattlerIndex], playerParty.party[currentBattlerIndex]);

            if (playerParty.party[currentBattlerIndex].currentHealth <= 0)
            {
                playerParty.party[currentBattlerIndex].isFainted = true;
                UIManager.SwitchBattlerBecauseOfDeath();
            }

            CheckForWinCondition();

            playerParty.party[currentBattlerIndex].currentHealth -= (int)damageToDo;
        }

        public void EndBattle()
        {
            Debug.Log("Ending Battle");

            string playerPath = Application.persistentDataPath + "/party.json";
            string aponentPath = Application.persistentDataPath + "/apponentTestParty.json";

            SaveAndLoad<Party>.SaveJson(playerParty, playerPath);
            SaveAndLoad<Party>.SaveJson(apponentParty, aponentPath);

            SceneManager.LoadScene(1);
        }

        private void CheckForWinCondition()
        {
            int playerFaintedPokemon = 0;
            int playerPartyCount = 0;

            for (int i = 0; i < playerParty.party.Length; i++)
            {
                if (playerParty.party[i].name != "")
                {
                    playerPartyCount++;
                }
            }

            for (int i = 0; i < playerParty.party.Length; i++)
            {
                if (playerParty.party[i].isFainted)
                {
                    playerFaintedPokemon++;
                }
            }

            if (playerFaintedPokemon == playerPartyCount)
            {
                Debug.Log("Battle ended because player lost all pokemon");
                EndBattle();
            }

            int enemyFaintedPokemon = 0;
            int enemyPartyCount = 0;

            for (int i = 0; i < apponentParty.party.Length; i++)
            {
                if (apponentParty.party[i].name != "")
                {
                    enemyPartyCount++;
                }
            }

            for (int i = 0; i < apponentParty.party.Length; i++)
            {
                if (apponentParty.party[i].isFainted)
                {
                    enemyFaintedPokemon++;
                }
            }

            if (enemyFaintedPokemon == enemyPartyCount)
            {
                Debug.Log("Battle ended because enemy lost all pokemon");
                EndBattle();
            }
        }
    }
}