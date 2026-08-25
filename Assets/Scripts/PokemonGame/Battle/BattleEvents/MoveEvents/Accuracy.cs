using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PokemonGame.Battle
{
    using General;
    
    public class Accuracy : BattleEvent
    {
        public override string Name { get; set; } = "Accuracy";
        
        public override void Event(Battle battle, List<object> vars)
        {
            MoveStatus status = (MoveStatus)vars[0];
            
            if (status.Failed) // move has already failed and so there is nothing for us to do now
                return;
            
            // actually has like a usable accuracy
            if (status.Move.accuracy != 0 && !Mathf.Approximately(status.Move.accuracy, 1))
            {
                for (int i = 0; i < status.Targets.Count; i++)
                {
                    int targetPlayer = status.Targets[i].Item1;
                    int targetBattler = status.Targets[i].Item2;

                    Battler defender = battle.activeBattlers[targetBattler][targetBattler];
                    
                    float accuracy = StatStages.GetMultiplierFromStage(status.Attacker.modifierStats.accuracyStage, true, false);
                    float evasiveness = StatStages.GetMultiplierFromStage(defender.modifierStats.evasionStage, true, true);
                    bool missed = Random.Range(1, 101) > status.Move.accuracy * accuracy * evasiveness * 100;

                    if (missed)
                    {
                        status.Failures[i] = true;
                    
                        battle.AddVisibleBattleAction(VisibleBattleActionType.Missed, new List<object>
                        {
                            status.PlayerIndex,
                            status.ActionIndex,
                            targetPlayer,
                            targetBattler
                        });
                    }
                }
            }
        }
    }
}