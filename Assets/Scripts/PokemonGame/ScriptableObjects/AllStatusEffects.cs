using UnityEngine;
using System.Collections.Generic;
using System;
using PokemonGame.Battle;

namespace PokemonGame
{
    /// <summary>
    /// A collection of every status effect in the game
    /// </summary>
    [CreateAssetMenu(fileName = "New All Status Effects", menuName = "All/New All Status Effects")]
    public class AllStatusEffects : ScriptableObject, ISerializationCallbackReceiver
    {
        public List<string> keys = new List<string>();
        public List<StatusEffect> values = new List<StatusEffect>();

        public static Dictionary<string, StatusEffect> effects = new Dictionary<string, StatusEffect>();

        public StatusEffect effectToAdd;

        public void OnBeforeSerialize()
        {
            keys.Clear();
            values.Clear();

            foreach (var kvp in effects)
            {
                keys.Add(kvp.Key);
                values.Add(kvp.Value);
            }
        }

        public void OnAfterDeserialize()
        {
            effects = new Dictionary<string, StatusEffect>();

            for (int i = 0; i != Math.Min(keys.Count, values.Count); i++)
                effects.Add(keys[i], values[i]);
        }
    }

}