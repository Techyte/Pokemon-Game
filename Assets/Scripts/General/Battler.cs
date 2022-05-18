using PokemonGame.Battle;
using UnityEngine;

namespace PokemonGame
{
    [System.Serializable]
    public class Battler
    {
        public BattlerTemplate source;
        public string name;
        public int maxHealth;
        public int currentHealth;
        public int level;
        public int exp;
        public int attack;
        public int defense;
        public int specialAttack;
        public int specialDefense;
        public int speed;
        public Sprite texture;
        public bool isFainted;
        public StatusEffect statusEffect;

        public Type primaryType;
        public Type secondaryType;

        public Move[] moves;

        public Battler(BattlerTemplate source, int level, StatusEffect statusEffect, string name, int health, Move move1, Move move2, Move move3, Move move4)
        {
            this.source = source;
            this.level = level;
            this.name = name;
            isFainted = false;
            exp = 0;
            this.statusEffect = statusEffect;
            primaryType = source.primaryType;
            secondaryType = source.secondaryType;
            currentHealth = health;
            moves = new Move[4];
            moves[0] = move1;
            moves[1] = move2;
            moves[2] = move3;
            moves[3] = move4;

            maxHealth = Mathf.FloorToInt(0.01f * (2 * source.baseHealth + 15 + Mathf.FloorToInt(0.25f * 15)) * level) + level + 10;
            attack = Mathf.FloorToInt(0.01f * (2 * source.baseAttack + 15 + Mathf.FloorToInt(0.25f * 15)) * level) + 5;
            defense = Mathf.FloorToInt(0.01f * (2 * source.baseDefense + 15 + Mathf.FloorToInt(0.25f * 15)) * level) + 5;
            specialAttack = Mathf.FloorToInt(0.01f * (2 * source.baseSpecialAttack + 15 + Mathf.FloorToInt(0.25f * 15)) * level) + 5;
            specialDefense = Mathf.FloorToInt(0.01f * (2 * source.baseSpecialDefense + 15 + Mathf.FloorToInt(0.25f * 15)) * level) + 5;
            speed = Mathf.FloorToInt(0.01f * (2 * source.baseSpeed + 15 + Mathf.FloorToInt(0.25f * 15)) * level) + 5;

            texture = source.texture;
        }

        public Battler(BattlerTemplate source, int level, StatusEffect statusEffect, string name, Move move1, Move move2, Move move3, Move move4)
        {
            this.source = source;
            this.level = level;
            this.name = name;
            isFainted = false;
            exp = 0;
            this.statusEffect = statusEffect;
            primaryType = source.primaryType;
            secondaryType = source.secondaryType;
            moves = new Move[4];
            moves[0] = move1;
            moves[1] = move2;
            moves[2] = move3;
            moves[3] = move4;

            maxHealth = Mathf.FloorToInt(0.01f * (2 * source.baseHealth + 15 + Mathf.FloorToInt(0.25f * 15)) * level) + level + 10;
            attack = Mathf.FloorToInt(0.01f * (2 * source.baseAttack + 15 + Mathf.FloorToInt(0.25f * 15)) * level) + 5;
            defense = Mathf.FloorToInt(0.01f * (2 * source.baseDefense + 15 + Mathf.FloorToInt(0.25f * 15)) * level) + 5;
            specialAttack = Mathf.FloorToInt(0.01f * (2 * source.baseSpecialAttack + 15 + Mathf.FloorToInt(0.25f * 15)) * level) + 5;
            specialDefense = Mathf.FloorToInt(0.01f * (2 * source.baseSpecialDefense + 15 + Mathf.FloorToInt(0.25f * 15)) * level) + 5;
            speed = Mathf.FloorToInt(0.01f * (2 * source.baseSpeed + 15 + Mathf.FloorToInt(0.25f * 15)) * level) + 5;

            currentHealth = maxHealth;

            texture = source.texture;
        }
    }
}