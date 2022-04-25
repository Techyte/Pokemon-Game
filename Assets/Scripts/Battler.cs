using UnityEngine;

[System.Serializable]
public class Battler
{
    public BattlerTemplate source;
    public string name;
    public int maxHealth;
    public int currentHealth;
    public int level;
    public int exp = 0;
    public int attack;
    public int defense;
    public int specialAttack;
    public int specialDefense;
    public int speed;
    public Sprite texture;
    public bool isFainted = false;
    public StatusEffect statusEffect;

    public Type primaryType;
    public Type secondaryType;

    public Move[] moves = new Move[4];
}