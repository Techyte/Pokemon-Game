using PokemonGame.Global;

namespace PokemonGame.ScriptableObjects
{
    using System;
    using General;
    using UnityEngine;
    using UnityEngine.Events;

    [CreateAssetMenu(order = 4, fileName = "New Status Effect", menuName = "Pokemon Game/New Status Effect")]
    public class StatusEffect : ScriptableObject
    {
        public new string name;

        public UnityEvent<StatusEffectEventArgs> EffectEvent;

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

        public void Effect(StatusEffectEventArgs e)
        {
            EffectEvent?.Invoke(e);
        }
    }

    public class StatusEffectEventArgs : EventArgs
    {
        public StatusEffectEventArgs(Battler battler)
        {
            this.battler = battler;
        }
        
        public Battler battler;
    }
}