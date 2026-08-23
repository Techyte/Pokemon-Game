using System;
using System.Collections.Generic;

namespace PokemonGame.Battle
{
    using General;
    [Serializable]
    public class PrimaryEffect : BattleEvent
    {
        public override void Event(Battle battle, List<object> vars)
        {
            MoveStatus status = (MoveStatus)vars[0];
            
            if (status.Failed) // move has already failed and so there is nothing for us to do now
                return;

            for (int i = 0; i < status.Targets.Count; i++)
            {
                // this move has already failed to hit this target and so we just ignore it
                if (status.Failures[i])
                    continue;
                
                // this move has not failed fully and has not failed to hit this target
                // do the basic damage calculation or primary effect
                
                status.Move.MoveMethod(status, i); // moves themselves handle their own effects and such, and queue visible actions accordingly
            }
        }
    }
}