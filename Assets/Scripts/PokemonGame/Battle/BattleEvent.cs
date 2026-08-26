using System;
using System.Collections.Generic;

namespace PokemonGame.Battle
{
    [Serializable]
    public class BattleEvent
    {
        public virtual string Name()
        {
            return "BattleEvent";
        }

        public virtual void Event(Battle battle)
        {
            // called by battle class to begin an event
        }

        public virtual void Event(Battle battle, List<object> variables)
        {
            
        }
    }
}