using UnityEngine;
using System.Collections.Generic;
using System;

namespace PokemonGame.ScriptableObjects
{
    /// <summary>
    /// A collection of every status effect in the game
    /// </summary>
    [CreateAssetMenu(fileName = "New All Status Effects", menuName = "All/New All Status Effects")]
    public class AllStatusEffects : ScriptableObject, ISerializationCallbackReceiver
    {
        public List<string> _keys = new List<string>( );
        public List<StatusEffect> _values = new List<StatusEffect>();

        /// <summary>
        /// The list of every status effect
        /// </summary>
        public Dictionary<string, StatusEffect> effects = new Dictionary<string, StatusEffect>();

        public StatusEffect effectToAdd;

        /// <summary>
        /// Attempts to get an effect from the register and handles errors
        /// </summary>
        /// <param name="EffectName">The name of the effect that you want to fetch</param>
        /// <param name="effect">tThe outputted effect</param>
        public bool GetEffect(string EffectName, out StatusEffect effect)
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

        /// <summary>
        /// Adds a status effect to to the register
        /// </summary>
        /// <param name="effectToAdd">The move you want to add</param>
        public void AddEffect(StatusEffect effectToAdd)
        {
            if (!effects.ContainsKey(effectToAdd.name))
            {
                effects.Add(effectToAdd.name, effectToAdd);
            }
            else
            {
                Debug.LogWarning("Status effect is already in the list, please do not try and add it again");
            }
        }
        
        public void OnBeforeSerialize()
        {
            _keys.Clear();
            _values.Clear();

            foreach (var kvp in effects)
            {
                _keys.Add(kvp.Key);
                _values.Add(kvp.Value);
            }
        }

        public void OnAfterDeserialize()
        {
            effects = new Dictionary<string, StatusEffect>();

            for (int i = 0; i != Math.Min(_keys.Count, _values.Count); i++)
                effects.Add(_keys[i], _values[i]);
        }
    }
}