using System;
using PokemonGame.Battle;
using UnityEngine;

namespace PokemonGame
{
    [Serializable]
    public class Battler : ScriptableObject
    {
        private BattlerTemplate _oldSource;
        public BattlerTemplate source;

        private int _oldLevel;
        public int level;

        public new string name;
        public int maxHealth;
        public int currentHealth;
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

        private void OnValidate()
        {
            if (!statusEffect)
                statusEffect = AllStatusEffects.effects["Healthy"];
            
            if (_oldLevel != level)
            {
                UpdateStats();
            }

            if (_oldSource != source)
            {
                UpdateStats();
                UpdateSource();
            }

            _oldSource = source;
            _oldLevel = level;

            if (currentHealth > maxHealth)
                currentHealth = maxHealth;
        }
        
        private void UpdateStats()
        {
            if(!source) return;
            
            maxHealth = Mathf.FloorToInt(0.01f * (2 * source.baseHealth + 15 + Mathf.FloorToInt(0.25f * 15)) * level) + level + 10;
            attack = Mathf.FloorToInt(0.01f * (2 * source.baseAttack + 15 + Mathf.FloorToInt(0.25f * 15)) * level) + 5;
            defense = Mathf.FloorToInt(0.01f * (2 * source.baseDefense + 15 + Mathf.FloorToInt(0.25f * 15)) * level) + 5;
            specialAttack = Mathf.FloorToInt(0.01f * (2 * source.baseSpecialAttack + 15 + Mathf.FloorToInt(0.25f * 15)) * level) + 5;
            specialDefense = Mathf.FloorToInt(0.01f * (2 * source.baseSpecialDefense + 15 + Mathf.FloorToInt(0.25f * 15)) * level) + 5;
            speed = Mathf.FloorToInt(0.01f * (2 * source.baseSpeed + 15 + Mathf.FloorToInt(0.25f * 15)) * level) + 5;
        }

        private void UpdateSource()
        {
            primaryType = source.primaryType;
            secondaryType = source.secondaryType;
            texture = source.texture;
        }

        public static Battler Init(BattlerTemplate source, int level, StatusEffect statusEffect, string name, Move move1, Move move2, Move move3, Move move4, bool autoAssignHealth)
        {
            Battler returnBattler = CreateInstance<Battler>();
            
            returnBattler.source = source;
            returnBattler.level = level;
            returnBattler.name = name;
            returnBattler.isFainted = false;
            returnBattler.exp = 0;
            returnBattler.statusEffect = statusEffect;
            returnBattler.primaryType = source.primaryType;
            returnBattler.secondaryType = source.secondaryType;
            returnBattler.moves = new Move[4];
            returnBattler.moves[0] = move1;
            returnBattler.moves[1] = move2;
            returnBattler.moves[2] = move3;
            returnBattler.moves[3] = move4;

            if (autoAssignHealth)
                returnBattler.currentHealth = returnBattler.maxHealth;
            
            returnBattler.UpdateStats();

            returnBattler.texture = source.texture;
            
            return returnBattler;
        }
        
        public static Battler CreateCoppy(Battler battler)
        {
            Battler returnBattler = Init(battler.source, battler.level, battler.statusEffect, battler.name,
                battler.moves[0], battler.moves[1], battler.moves[2], battler.moves[3], false);

            returnBattler.currentHealth = battler.currentHealth;
            
            return returnBattler;
        }
    }
}