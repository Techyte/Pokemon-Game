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