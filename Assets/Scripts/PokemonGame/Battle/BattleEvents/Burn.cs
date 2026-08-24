using System;
using System.Collections.Generic;
using PokemonGame.General;
using PokemonGame.Global;
using PokemonGame.ScriptableObjects;
using UnityEngine;

namespace PokemonGame.Battle
{
    [Serializable]
    public class Burn : BattleEvent
    {
        public override void Event(Battle battle)
        {
            for (int i = 0; i < battle.activeBattlers.Count; i++)
            {
                for (int j = 0; j < battle.activeBattlers[i].Count; j++)
                {
                    // run through every active battler

                    Battler battler = battle.activeBattlers[i][j];

                    StatusEffect burn = Registry.GetStatusEffect("Burn");

                    if (battler.statusEffect == burn)
                    {
                        battler.TakeDamage(Mathf.CeilToInt(battler.stats.maxHealth/8f), new StatusEffectDamageSource(battler.statusEffect));
                        
                        battle.AddVisibleBattleAction(VisibleBattleActionType.HasStatus, new List<object>
                        {
                            i,
                            j,
                            burn
                        });
                    }
                }
            }
        }
    }
}