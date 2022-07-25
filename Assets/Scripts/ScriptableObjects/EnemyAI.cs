using System;
using UnityEngine;
using UnityEngine.Events;

namespace PokemonGame.Battle
{
    [CreateAssetMenu(order = 5, fileName = "New AI", menuName = "Pokemon Game/New AI")]
    public class EnemyAI : ScriptableObject
    {
        public new string name;

        public UnityEvent<AIMethodEventArgs> AIMethodEvent;

        public void AIMethod(AIMethodEventArgs e)
        {
            try
            {
                AIMethodEvent.Invoke(e);
            }
            catch
            {
                Debug.LogWarning($"{name}s effect does not have a function associated with it");
            }
        }
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