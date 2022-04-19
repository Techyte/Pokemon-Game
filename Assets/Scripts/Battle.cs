using UnityEngine;
using TMPro;
using UnityEngine.UI;

public enum Turn
{
    Player,
    Enemy
}

public class Battle : MonoBehaviour
{
    [Header("PLAYER UI:")]
    public GameObject PlayerUIHolder;

    [Space]
    [Header("Assignments")]
    public BattlerTemplate currentBattlerSource;
    public Battler currentBattler;
    public SpriteRenderer currentBattlerRenderer;

    public BattlerTemplate aponentBattlerSource;
    public Battler aponentBattler;
    public SpriteRenderer aponentBattlerRenderer;
    public Slider AponentHealthDisplay;

    public TextMeshProUGUI[] moveTexts;

    public AllMoves allMoves;

    [Space]
    [Header("Health Readouts")]
    public float currentHealth;
    public float aponentHealth;

    [Space]
    [Header("Other Readouts")]
    public Turn currentTurn = Turn.Player;

    private bool playerHasAttacked;

    private void Start()
    {
        currentBattler = new GameObject("CurrentBattlerHolder").AddComponent<Battler>();
        currentBattler.SetUp(currentBattlerSource, 5, currentBattlerSource.baseHealth, currentBattlerSource.moves[0], currentBattlerSource.moves[1]);

        aponentBattler = new GameObject("AponentBattlerHolder").AddComponent<Battler>();
        aponentBattler.SetUp(aponentBattlerSource, 5, aponentBattlerSource.baseHealth, aponentBattlerSource.moves[0]);

        currentBattlerRenderer.sprite = currentBattler.texture;
        aponentBattlerRenderer.sprite = aponentBattler.texture;

        for(int i = 0; i < moveTexts.Length; i++)
        {
            moveTexts[i].transform.parent.gameObject.SetActive(false);
        }

        for(int i = 0; i < currentBattler.moves.Length; i++)
        {
            if (currentBattler.moves[i])
            {
                moveTexts[i].transform.parent.gameObject.SetActive(true);
                moveTexts[i].text = currentBattler.moves[i].name;
            }
        }

        aponentBattler.currentHealth = aponentBattler.maxHealth;
        AponentHealthDisplay.maxValue = aponentBattler.maxHealth;
        AponentHealthDisplay.value = aponentBattler.maxHealth;
    }

    private void Update()
    {
        if(currentTurn == Turn.Player)
        {
            DoPlayerTurn();
        }

        if (currentTurn == Turn.Enemy)
        {
            DoEnemyTurn();
        }
    }

    public void DoMove(int moveID)
    {
        if (currentBattler.moves[moveID])
            DoMoveOnAposingBattler(currentBattler.moves[moveID]);
    }

    void DoMoveOnAposingBattler(Move move)
    {
        //You can add any animation calls for attacking here

        float damageToDo = CalculateDamage(move, currentBattler, aponentBattler);
        aponentBattler.currentHealth -= (int)damageToDo;
        Debug.Log(damageToDo);

        playerHasAttacked = true;
    }

    float CalculateDamage(Move move, Battler battlerThatUsed, Battler battlerBeingAttacked)
    {
        float finalDamage;

        float STAB = 1;
        for (int i = 0; i < move.type.strongAgainst.Length; i++)
        {
            if (move.type.strongAgainst[i] == battlerBeingAttacked.primaryType || move.type.strongAgainst[i] == battlerBeingAttacked.secondaryType)
            {
                STAB = 1.5f;
            }
        }

        float TYPE = 1;
        if (move.type == battlerThatUsed.primaryType)
        {
            TYPE = 2;
        }

        if (move.type == battlerThatUsed.secondaryType)
        {
            if(TYPE == 2)
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
        Debug.Log("Damage was " + damage + " but now it is " + RANDOMNESS);
        damage = RANDOMNESS;

        finalDamage = damage;

        return finalDamage;
    }

    void ChangeTurn()
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

    void DoPlayerTurn()
    {
        PlayerUIHolder.SetActive(true);

        if (playerHasAttacked)
        {
            AponentHealthDisplay.value = aponentBattler.currentHealth;
            playerHasAttacked = false;

            ChangeTurn();
        }
    }

    void DoEnemyTurn()
    {
        PlayerUIHolder.SetActive(false);
        Debug.Log("Is enemy turn");
        ChangeTurn();
    }
}
