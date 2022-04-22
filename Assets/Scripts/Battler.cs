using UnityEngine;

public class Battler
{
    public BattlerTemplate source;
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
}