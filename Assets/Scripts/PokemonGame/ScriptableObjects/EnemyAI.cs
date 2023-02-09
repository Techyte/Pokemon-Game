namespace PokemonGame.ScriptableObjects
{
    using System;
    using Game.Party;
    using General;
    using UnityEngine;
    using UnityEngine.Events;

    [CreateAssetMenu(order = 5, fileName = "New AI", menuName = "Pokemon Game/New AI")]
    public class EnemyAI : ScriptableObject
    {
        public new string name;

        public UnityEvent<AIMethodEventArgs> aIMethodEvent;

        public void AIMethod(AIMethodEventArgs e)
        {
            aIMethodEvent?.Invoke(e);
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