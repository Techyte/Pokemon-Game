using System;

namespace PokemonGame.Battle
{
    [Serializable]
    public class BattleEvent
    {
        public virtual void Event(Battle battle)
        {
            // called by battle class to begin an event
        }
    }
}