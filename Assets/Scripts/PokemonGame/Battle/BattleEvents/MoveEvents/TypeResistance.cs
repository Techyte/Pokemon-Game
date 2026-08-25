using System;
using System.Collections.Generic;
using PokemonGame.General;

namespace PokemonGame.Battle
{
    public class TypeResistance : BattleEvent
    {
        public override string Name { get; set; } = "Type Resistance";
        
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
                
                Battler target = status.Battle.activeBattlers[status.Targets[i].Item1][status.Targets[i].Item2];
                
                // test if the move can even hit the target
                if (!target.GetCanTypeHit(status.Move.type))
                {
                    status.Failures[i] = true;
                    battle.AddVisibleBattleAction(VisibleBattleActionType.Resists, new List<object>
                    {
                        status.Targets[i].Item1,
                        status.Targets[i].Item2
                    });
                }
            }
        }
    }
}