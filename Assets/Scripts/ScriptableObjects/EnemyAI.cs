using System;
using UnityEngine;

namespace PokemonGame.Battle
{
    [CreateAssetMenu(order = 5, fileName = "New AI", menuName = "Pokemon Game/New AI")]
    public class EnemyAI : ScriptableObject
    {
        public new string name;

        public void AIMethod(object sender, AIMethodEventArgs e)
        {
            try
            {
                aiMethod.Invoke(sender, e);
            }
            catch
            {
                Debug.LogWarning($"{name}s effect does not have a function associated with it");
            }
        }
        
        public event EventHandler<AIMethodEventArgs> aiMethod;
    }

    public class AIMethodEventArgs : EventArgs
    {
        public AIMethodEventArgs(Battler battlerToUse, Party usableParty)
        {
            this.battlerToUse = battlerToUse;
            this.usableParty = usableParty;
        }
        
        public Battler battlerToUse;
        public Party usableParty;
    }

}