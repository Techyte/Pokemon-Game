using UnityEngine;

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
        public Move enemyMoveToDo;
        public bool playerHasChosenAttack;
        [SerializeField]private bool hasDoneChoosingUpdate;
        [SerializeField]private bool hasShowedMoves;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            playerParty = LoaderInfo.playerParty;
            apponentParty = LoaderInfo.apponentParty;
            enemyAI = LoaderInfo.enemyAI;

            BattleManager.ClearLoader();

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

            switch (currentTurn)
            {
                case TurnStatus.Ending:
                    TurnEnding();
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
                    }
                    break;
            }
        }

        private void TurnShowing()
        {
            if (!hasShowedMoves)
            {
                UIManager.ShowUI(false);
                DoMoves();
                hasShowedMoves = true;
                playerHasChosenAttack = false;
                currentTurn = TurnStatus.Ending;
            }
        }

        private void TurnEnding()
        {
            UIManager.ShowUI(false);
            hasDoneChoosingUpdate = false;
            hasShowedMoves = false;
            playerHasChosenAttack = false;
            DoStatusEffects();
            currentTurn = TurnStatus.Chosing;
        }

        private void DoStatusEffects()
        {
            apponentParty.party[apponentBattlerIndex].statusEffect.effect(apponentParty.party[apponentBattlerIndex]);

            playerParty.party[currentBattlerIndex].statusEffect.effect(playerParty.party[currentBattlerIndex]);
            UIManager.UpdateHealthDisplays();
        }

        public void ChooseMove(int moveID)
        {
            playerMoveToDo = playerParty.party[currentBattlerIndex].moves[moveID];
            playerHasChosenAttack = true;
        }

        private void DoPlayerMove()
        {
            //You can add any animation calls for attacking here

            if (playerMoveToDo != null)
            {

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
                    //Debug.Log(move.type + " is weak against " + move.type.weakAgainst[i]);
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

            UIManager.UpdateHealthDisplays();
        }

        public void DoMoveOnPlayer(Move move)
        {
            enemyMoveToDo = move;
        }

        private void DoEnemyMove()
        {
            //You can add any animation calls for attacking here

            if (enemyMoveToDo.category == MoveCategory.Status)
            {
                enemyMoveToDo.moveMethod(apponentParty.party[currentBattlerIndex]);
            }
            else
            {
                float damageToDo = CalculateDamage(enemyMoveToDo, apponentParty.party[apponentBattlerIndex], playerParty.party[currentBattlerIndex]);
                playerParty.party[currentBattlerIndex].currentHealth -= (int)damageToDo;
            }

            if (playerParty.party[currentBattlerIndex].currentHealth <= 0)
            {
                playerParty.party[currentBattlerIndex].isFainted = true;
                UIManager.SwitchBattlerBecauseOfDeath();
            }

            CheckForWinCondition();

            UIManager.UpdateHealthDisplays();
        }

        public void EndBattle()
        {
            //Debug.Log("Ending Battle");

            string playerPath = Application.persistentDataPath + "/party.json";
            string aponentPath = Application.persistentDataPath + "/apponentTestParty.json";

            SaveAndLoad<Party>.SaveJson(playerParty, playerPath);
            SaveAndLoad<Party>.SaveJson(apponentParty, aponentPath);

            BattleManager.LoadScene(null, null, null, 0);
        }

        private void CheckForWinCondition()
        {
            int playerFaintedPokemon = 0;
            int playerPartyCount = 0;

            for (int i = 0; i < playerParty.party.Count; i++)
            {
                if (playerParty.party[i].name != "")
                {
                    playerPartyCount++;
                }
            }

            for (int i = 0; i < playerParty.party.Count; i++)
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

            for (int i = 0; i < apponentParty.party.Count; i++)
            {
                if (apponentParty.party[i].name != "")
                {
                    enemyPartyCount++;
                }
            }

            for (int i = 0; i < apponentParty.party.Count; i++)
            {
                if (apponentParty.party[i].isFainted)
                {
                    enemyFaintedPokemon++;
                }
            }

            if (enemyFaintedPokemon == enemyPartyCount)
            {
                //Debug.Log("Battle ended because enemy lost all pokemon");
                EndBattle();
            }
        }
    }
}