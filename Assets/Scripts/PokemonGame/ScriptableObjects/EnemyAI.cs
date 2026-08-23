
namespace PokemonGame.ScriptableObjects
{
    using System;
    using Game.Party;
    using Battle;
    using UnityEngine;
    using UnityEngine.Events;

    [CreateAssetMenu(order = 5, fileName = "New AI", menuName = "Pokemon Game/New AI")]
    public class EnemyAI : ScriptableObject
    {
        public new string name;

        public UnityEvent<Battle, int> aIMethodEvent;
        public UnityEvent<AISwitchEventArgs> aISwitchEvent;

        public void AIMethod(Battle b, int i)
        {
            aIMethodEvent?.Invoke(b, i);
        }

        public void AISwitchMethod(AISwitchEventArgs e)
        {
            aISwitchEvent?.Invoke(e);
        }
    }

    public class AISwitchEventArgs : EventArgs
    {
        public AISwitchEventArgs(int currentIndex, Party usableParty, Battle battle)
        {
            this.currentIndex = currentIndex;
            this.usableParty = usableParty;
            this.battle = battle;
            newBattlerIndex = currentIndex;
        }

        public int currentIndex;
        public int newBattlerIndex;
        public Party usableParty;
        public Battle battle;
    }
}