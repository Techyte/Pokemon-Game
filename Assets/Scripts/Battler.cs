using UnityEngine;

public class Battler : MonoBehaviour
{
    BattlerTemplate source;
    public int maxHealth;
    public int currentHealth;
    public int level;
    public int attack;
    public int defense;
    public int specialAttack;
    public int specialDefense;
    public int speed;
    public int accuracy;
    public int evasion;
    public Sprite texture;

    public Type primaryType;
    public Type secondaryType;

    public Move[] moves = new Move[4];

    public Battler SetUp(BattlerTemplate source, int level, int health, Move move1)
    {
        this.source = source;
        this.level = level;
        this.primaryType = source.primaryType;
        this.secondaryType = source.secondaryType;
        this.currentHealth = health;
        this.moves[0] = move1;
        CalculateStats();
        return this;
    }

    public Battler SetUp(BattlerTemplate source, int level, int health, Move move1, Move move2)
    {
        this.source = source;
        this.level = level;
        this.primaryType = source.primaryType;
        this.secondaryType = source.secondaryType;
        this.currentHealth = health;
        this.moves[0] = move1;
        this.moves[1] = move2;
        CalculateStats();
        return this;
    }

    public Battler SetUp(BattlerTemplate source, int level, int health, Move move1, Move move2, Move move3)
    {
        this.source = source;
        this.level = level;
        this.primaryType = source.primaryType;
        this.secondaryType = source.secondaryType;
        this.currentHealth = health;
        this.moves[0] = move1;
        this.moves[1] = move2;
        this.moves[2] = move3;
        CalculateStats();
        return this;
    }

    public Battler SetUp(BattlerTemplate source, int level, int health, Move move1, Move move2, Move move3, Move move4)
    {
        this.source = source;
        this.level = level;
        this.primaryType = source.primaryType;
        this.secondaryType = source.secondaryType;
        this.currentHealth = health;
        this.moves[0] = move1;
        this.moves[1] = move2;
        this.moves[2] = move3;
        this.moves[3] = move4;
        CalculateStats();
        return this;
    }

    void CalculateStats()
    {
        maxHealth = Mathf.FloorToInt(0.01f * (2 * source.baseHealth + 15 + Mathf.FloorToInt(0.25f * 15)) * level) + level + 10;
        attack = Mathf.FloorToInt(0.01f * (2 * source.baseAttack + 15 + Mathf.FloorToInt(0.25f * 15)) * level) + 5;
        defense = Mathf.FloorToInt(0.01f * (2 * source.baseDefense + 15 + Mathf.FloorToInt(0.25f * 15)) * level) + 5;
        specialAttack = Mathf.FloorToInt(0.01f * (2 * source.baseSpecialAttack + 15 + Mathf.FloorToInt(0.25f * 15)) * level) + 5;
        specialDefense = Mathf.FloorToInt(0.01f * (2 * source.baseSpecialDefense + 15 + Mathf.FloorToInt(0.25f * 15)) * level) + 5;
        speed = Mathf.FloorToInt(0.01f * (2 * source.baseSpeed + 15 + Mathf.FloorToInt(0.25f * 15)) * level) + 5;

        texture = source.texture;
    }
}
