using System;
using UnityEngine;

namespace PokemonGame.Battle
{
    [CreateAssetMenu(order = 4, fileName = "New Status Effect", menuName = "Pokemon Game/New Status Effect")]
    public class StatusEffect : ScriptableObject
    {
        public new string name;

        public void Effect(object sender, StatusEffectEventArgs e)
        {
            try
            {
                effect.Invoke(sender, e);
            }
            catch
            {
                Debug.LogWarning($"{name}s effect does not have a function associated with it");
            }
        }
        
        public event EventHandler<StatusEffectEventArgs> effect;
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