using System;
using System.Collections.Generic;

namespace PokemonGame.ScriptableObjects
{
    using General;
    using Global;
    using UnityEngine;
    using UnityEngine.Events;

    [CreateAssetMenu(order = 4, fileName = "New Status Effect", menuName = "Pokemon Game/New Status Effect")]
    public class StatusEffect : ScriptableObject
    {
        public new string name;
        public Color colour;

        /// <summary>
        /// The default status effect
        /// </summary>
        public static StatusEffect Healthy => HealthyEffect();

        private static StatusEffect HealthyEffect()
        {
            StatusEffect effect = Registry.GetStatusEffect("Healthy");
            
            if (effect)
            {
                return effect;
            }
            Debug.LogWarning("Can't find the healthy status effect, something has gone terribly wrong if you are seeing this message lol");
            return null;
        }
    }
}