using UnityEngine;

public class BattlerCreator
{
    public static Battler SetUp(BattlerTemplate source, int level, string name, int health, Move move1, Move move2, Move move3, Move move4)
    {
        Battler battler = new Battler();
        battler.source = source;
        battler.level = level;
        battler.name = name;
        battler.primaryType = source.primaryType;
        battler.secondaryType = source.secondaryType;
        battler.currentHealth = health;
        battler.moves[0] = move1;
        battler.moves[1] = move2;
        battler.moves[2] = move3;
        battler.moves[3] = move4;

        battler.maxHealth = Mathf.FloorToInt(0.01f * (2 * source.baseHealth + 15 + Mathf.FloorToInt(0.25f * 15)) * level) + level + 10;
        battler.attack = Mathf.FloorToInt(0.01f * (2 * source.baseAttack + 15 + Mathf.FloorToInt(0.25f * 15)) * level) + 5;
        battler.defense = Mathf.FloorToInt(0.01f * (2 * source.baseDefense + 15 + Mathf.FloorToInt(0.25f * 15)) * level) + 5;
        battler.specialAttack = Mathf.FloorToInt(0.01f * (2 * source.baseSpecialAttack + 15 + Mathf.FloorToInt(0.25f * 15)) * level) + 5;
        battler.specialDefense = Mathf.FloorToInt(0.01f * (2 * source.baseSpecialDefense + 15 + Mathf.FloorToInt(0.25f * 15)) * level) + 5;
        battler.speed = Mathf.FloorToInt(0.01f * (2 * source.baseSpeed + 15 + Mathf.FloorToInt(0.25f * 15)) * level) + 5;

        battler.texture = source.texture;

        return battler;
    }

    public static Battler SetUp(BattlerTemplate source, int level, string name, Move move1, Move move2, Move move3, Move move4)
    {
        Battler battler = new Battler();
        battler.source = source;
        battler.level = level;
        battler.name = name;
        battler.primaryType = source.primaryType;
        battler.secondaryType = source.secondaryType;
        battler.moves[0] = move1;
        battler.moves[1] = move2;
        battler.moves[2] = move3;
        battler.moves[3] = move4;

        battler.maxHealth = Mathf.FloorToInt(0.01f * (2 * source.baseHealth + 15 + Mathf.FloorToInt(0.25f * 15)) * level) + level + 10;
        battler.attack = Mathf.FloorToInt(0.01f * (2 * source.baseAttack + 15 + Mathf.FloorToInt(0.25f * 15)) * level) + 5;
        battler.defense = Mathf.FloorToInt(0.01f * (2 * source.baseDefense + 15 + Mathf.FloorToInt(0.25f * 15)) * level) + 5;
        battler.specialAttack = Mathf.FloorToInt(0.01f * (2 * source.baseSpecialAttack + 15 + Mathf.FloorToInt(0.25f * 15)) * level) + 5;
        battler.specialDefense = Mathf.FloorToInt(0.01f * (2 * source.baseSpecialDefense + 15 + Mathf.FloorToInt(0.25f * 15)) * level) + 5;
        battler.speed = Mathf.FloorToInt(0.01f * (2 * source.baseSpeed + 15 + Mathf.FloorToInt(0.25f * 15)) * level) + 5;

        battler.currentHealth = battler.maxHealth;

        battler.texture = source.texture;

        return battler;
    }
}
