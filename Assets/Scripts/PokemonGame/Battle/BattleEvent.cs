using System;

namespace PokemonGame.Battle
{
    [Serializable]
    public class BattleEvent
    {
        public virtual void Event()
        {
            // called by battle class to begin an event
        }
    }
}