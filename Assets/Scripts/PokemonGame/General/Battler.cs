using System;
using System.Collections.Generic;
using System.Linq;
using PokemonGame.ScriptableObjects;
using UnityEngine;
using Type = PokemonGame.ScriptableObjects.Type;

namespace PokemonGame
{
    /// <summary>
    /// The class that contains all the information for a battler
    /// </summary>
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

        public List<Move> moves;

        /// <summary>
        /// Inflict damage onto the battler
        /// </summary>
        /// <param name="damage">The amount of damage to inflict</param>
        public void TakeDamage(int damage)
        {
            currentHealth -= damage;
            if (currentHealth <= 0)
                isFainted = true;
        }

        /// <summary>
        /// Used to change the battlers current health without inflicting damage
        /// </summary>
        /// <param name="newHealth">The health to set it to</param>
        public void UpdateHealth(int newHealth)
        {
            currentHealth = newHealth;
            
            if (currentHealth > maxHealth)
                currentHealth = maxHealth;
        }

        //Used for updating stats and such outside of runtime
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
            
            //Making sure the battler always has 4 moves
            if (moves.Count < 4)
                for (int i = 0; i < 4-moves.Count; i++)
                    moves.Add(null);
        }
        
        //Updates the stats of the battler
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

        //Updates the source of the battler
        private void UpdateSource()
        {
            primaryType = source.primaryType;
            secondaryType = source.secondaryType;
            texture = source.texture;
        }

        /// <summary>
        /// Returns a battler that has been created using the parameters given
        /// </summary>
        /// <param name="source">The Battler Template that the new battler will use to calculate stats</param>
        /// <param name="level">The <see cref="level"/> of the new battler</param>
        /// <param name="statusEffect">The status effect that the new battler will have</param>
        /// <param name="name">The nickname of the new battler</param>
        /// <param name="move1">Move that shows up first when battling</param>
        /// <param name="move2">Move that shows up second when battling</param>
        /// <param name="move3">Move that shows up third when battling</param>
        /// <param name="move4">Move that shows up fourth when battling</param>
        /// <param name="autoAssignHealth">Auto assign health to the <see cref="maxHealth"/> when creating</param>
        /// <returns>A battler that has been created using the parameters given</returns>
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
            returnBattler.moves = new Move[4].ToList();
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
        
        /// <summary>
        /// Creates an exact copy of the battler it is given
        /// </summary>
        /// <param name="battler">The battler to replicate</param>
        /// <returns>The copied battler</returns>
        public static Battler CreateCopy(Battler battler)
        {
            Battler returnBattler = Init(battler.source, battler.level, battler.statusEffect, battler.name,
                battler.moves[0], battler.moves[1], battler.moves[2], battler.moves[3], false);

            returnBattler.currentHealth = battler.currentHealth;
            
            return returnBattler;
        }
    }
}