using UnityEngine;
using UnityEngine.SceneManagement;

namespace PokemonGame.Battle
{
    public enum Turn
    {
        Player,
        Enemy
    }

    public class Battle : MonoBehaviour
    {
        [Header("UI:")]
        public GameObject PlayerUIHolder;
        [SerializeField] BattleUIManager UIManager;

        [Space]
        [Header("Assignments")]
        public int currentBattlerIndex;

        public int apponentBattlerIndex;

        //For testing
        public bool enemyCanAttack;

        [Space]
        [Header("Other Readouts")]
        public Turn currentTurn = Turn.Player;
        public Party playerParty;
        public Party apponentParty;

        private bool playerHasAttacked;
        EnemyAI enemyAI;

        private void Start()
        {
            playerParty = BattleLoaderInfo.playerParty;
            apponentParty = BattleLoaderInfo.apponentParty;
            enemyAI = BattleLoaderInfo.enemyAI;

            currentBattlerIndex = 0;

            apponentBattlerIndex = 0;

            apponentParty.party[apponentBattlerIndex].currentHealth = apponentParty.party[apponentBattlerIndex].maxHealth;
            playerParty.party[currentBattlerIndex].currentHealth = playerParty.party[currentBattlerIndex].maxHealth;

            //UIManager.UpdateBattlerButtons();

            BattleManager.ClearBattleLoader();
        }

        private void Update()
        {
            switch (currentTurn)
            {
                case Turn.Player:
                    DoPlayerTurn();
                    break;
                case Turn.Enemy:
                    DoEnemyTurn();
                    break;
            }
        }

        public void DoMove(int moveID)
        {
            DoMoveOnAposingBattler(playerParty.party[currentBattlerIndex].moves[moveID]);
        }

        private void DoMoveOnAposingBattler(Move move)
        {
            //You can add any animation calls for attacking here

            if (move.category == MoveCategory.Status)
            {
                move.moveMethod(apponentParty.party[apponentBattlerIndex]);
            }
            else
            {
                float damageToDo = CalculateDamage(move, playerParty.party[currentBattlerIndex], apponentParty.party[apponentBattlerIndex]);
                apponentParty.party[apponentBattlerIndex].currentHealth -= (int)damageToDo;
            }

            if (apponentParty.party[apponentBattlerIndex].currentHealth <= 0)
            {
                apponentParty.party[apponentBattlerIndex].isFainted = true;
            }

            CheckForWinCondition();

            UIManager.UpdateHealthDisplays();

            playerHasAttacked = true;
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

        public void ChangeTurn()
        {
            switch (currentTurn)
            {
                case Turn.Player:
                    currentTurn = Turn.Enemy;
                    break;
                case Turn.Enemy:
                    currentTurn = Turn.Player;
                    break;
            }
        }

        private void DoPlayerTurn()
        {
            PlayerUIHolder.SetActive(true);

            if (playerHasAttacked)
            {
                UIManager.UpdateHealthDisplays();

                playerParty.party[currentBattlerIndex].statusEffect.effect(playerParty.party[currentBattlerIndex]);
                apponentParty.party[apponentBattlerIndex].statusEffect.effect(apponentParty.party[apponentBattlerIndex]);

                playerHasAttacked = false;

                ChangeTurn();
            }
        }

        private void DoEnemyTurn()
        {
            PlayerUIHolder.SetActive(false);

            if (enemyCanAttack)
            {
                enemyAI.aiMethod(apponentParty.party[apponentBattlerIndex], apponentParty, this);
            }

            //Debug.Log("Is enemy turn");
            ChangeTurn();
        }

        public void DoMoveOnPlayer(Move move)
        {
            float damageToDo = CalculateDamage(move, apponentParty.party[apponentBattlerIndex], playerParty.party[currentBattlerIndex]);

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