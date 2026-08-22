using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace PokemonGame.Battle
{
    using General;
    using Global;
    using ScriptableObjects;
    
    [Serializable]
    public class Confusion : BattleEvent
    {
        public override void Event(Battle battle, List<object> vars)
        {
            MoveStatus status = (MoveStatus)vars[0];

            if (status.Failed) // move has already failed and so there is nothing for us to do now
                return;

            Battler attacker = battle.activeBattlers[status.PlayerIndex][status.ActionIndex];
            
            StatusEffect confusion = Registry.GetStatusEffect("Confusion");

            if (attacker.statusEffect == confusion)
            {
                battle.AddVisibleBattleAction(VisibleBattleActionType.HasStatus, new List<object>
                {
                    status.PlayerIndex,
                    status.ActionIndex,
                    confusion
                });
                
                attacker.statusTurns--;
                if (attacker.statusTurns <= 0) // snapped out of confusion
                {
                    attacker.statusEffect = StatusEffect.Healthy;
                    
                    battle.AddVisibleBattleAction(VisibleBattleActionType.StatusChange, new List<object>
                    {
                        status.PlayerIndex,
                        status.ActionIndex,
                        confusion,
                        StatusEffect.Healthy
                    });
                }
                else
                {
                    Confused(battle, status);
                }
            }
        }
        
        public void Confused(Battle battle, MoveStatus status)
        {
            bool hitSelf = Random.Range(1, 4) == 1; // 1 in 3 chance of hitting self

            if (hitSelf)
            {
                status.Failed = true;
                
                
                
                battle.AddVisibleBattleAction(VisibleBattleActionType.HitSelf, new List<object>
                {
                    status.PlayerIndex,
                    status.ActionIndex,
                    confusion,
                    StatusEffect.Healthy
                });
            }
            else
            {
                // do nothing cause confusion ultimately had no effect on the battler
            }
        }
    }
}