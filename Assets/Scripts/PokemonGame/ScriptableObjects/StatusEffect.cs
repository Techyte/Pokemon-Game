using System;
using UnityEngine;
using UnityEngine.Events;

namespace PokemonGame.ScriptableObjects
{
    [CreateAssetMenu(order = 4, fileName = "New Status Effect", menuName = "Pokemon Game/New Status Effect")]
    public class StatusEffect : ScriptableObject
    {
        public new string name;

        public UnityEvent<StatusEffectEventArgs> EffectEvent;

        public void Effect(StatusEffectEventArgs e)
        {
            try
            {
                EffectEvent.Invoke(e);
            }
            catch
            {
                Debug.LogWarning($"{name}s effect does not have a function associated with it");
            }
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