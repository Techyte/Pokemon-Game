using UnityEngine;
using System.Collections.Generic;
using System;

namespace PokemonGame.ScriptableObjects
{
    /// <summary>
    /// A collection of every status effect in the game
    /// </summary>
    [CreateAssetMenu(fileName = "New All Status Effects", menuName = "All/New All Status Effects")]
    public class AllStatusEffects : ScriptableObject
    {
        public List<string> keys = new List<string>();
        public List<StatusEffect> values = new List<StatusEffect>();

        /// <summary>
        /// The list of every status effect
        /// </summary>
        public static Dictionary<string, StatusEffect> effects = new Dictionary<string, StatusEffect>();

        public StatusEffect effectToAdd;

        /// <summary>
        /// Attempts to get an effect from the register and handles errors
        /// </summary>
        /// <param name="EffectName">The name of the effect that you want to fetch</param>
        /// <param name="effect">tThe outputted effect</param>
        public static bool GetEffect(string EffectName, out StatusEffect effect)
        {
            effect = null;
            if (effects.TryGetValue(EffectName, out StatusEffect effectToReturn))
            {
                effect = effectToReturn;
                return true;
            }
            
            Debug.LogWarning("Effect was not present in the register");
            return false;
        }

        private void OnValidate()
        {
            effects = new Dictionary<string, StatusEffect>();

            for (int i = 0; i != Math.Min(keys.Count, values.Count); i++)
                effects.Add(keys[i], values[i]);
        }
    }

}