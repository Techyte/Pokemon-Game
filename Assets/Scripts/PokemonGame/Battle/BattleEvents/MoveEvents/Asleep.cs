using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PokemonGame.Battle
{
    using General;
    using Global;
    using ScriptableObjects;
    
    [Serializable]
    public class Asleep : BattleEvent
    {
        public override string Name()
        {
            return "Asleep";
        }

        public override void Event(Battle battle, List<object> vars)
        {
            MoveStatus status = (MoveStatus)vars[0];

            if (status.Failed) // move has already failed and so there is nothing for us to do now
                return;

            Battler attacker = battle.activeBattlers[status.PlayerIndex][status.ActionIndex];
            
            StatusEffect asleep = Registry.GetStatusEffect("Asleep");

            if (attacker.statusEffect == asleep)
            {
                attacker.statusTurns--;
                if (attacker.statusTurns <= 0) // woke up
                {
                    attacker.statusEffect = StatusEffect.Healthy;
                    
                    battle.AddVisibleBattleAction(VisibleBattleActionType.StatusChange, new List<object>
                    {
                        status.PlayerIndex,
                        status.ActionIndex,
                        asleep,
                        StatusEffect.Healthy
                    });
                }
                else
                {
                    status.Failed = true;
                    battle.AddVisibleBattleAction(VisibleBattleActionType.HasStatus, new List<object>
                    {
                        status.PlayerIndex,
                        status.ActionIndex,
                        asleep
                    });
                }
            }
        }
    }
}