using System;
using System.Collections.Generic;
using PokemonGame.General;
using PokemonGame.Global;
using PokemonGame.ScriptableObjects;
using UnityEngine;

namespace PokemonGame.Battle
{
    [Serializable]
    public class Poison : BattleEvent
    {
        public override void Event(Battle battle)
        {
            for (int i = 0; i < battle.activeBattlers.Count; i++)
            {
                for (int j = 0; j < battle.activeBattlers[i].Count; j++)
                {
                    // run through every active battler

                    Battler battler = battle.activeBattlers[i][j];

                    StatusEffect poisoned = Registry.GetStatusEffect("Poisoned");

                    if (battler.statusEffect == poisoned)
                    {
                        battler.TakeDamage(Mathf.CeilToInt(battler.stats.maxHealth/8f), new StatusEffectDamageSource(battler.statusEffect));
                        
                        battle.AddVisibleBattleAction(VisibleBattleActionType.HasStatus, new List<object>
                        {
                            i,
                            j,
                            poisoned
                        });
                    }
                }
            }
        }
    }
}