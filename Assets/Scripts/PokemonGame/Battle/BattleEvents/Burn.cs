using System;
using System.Collections.Generic;
using PokemonGame.General;
using PokemonGame.Global;
using PokemonGame.ScriptableObjects;
using UnityEngine;

namespace PokemonGame.Battle
{
    public class Burn : BattleEvent
    {
        public override string Name { get; set; } = "Burn";
        
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
                        int damageTaken = Mathf.CeilToInt(battler.stats.maxHealth / 8f);
                        
                        battler.TakeDamage(damageTaken, DamageSource.Burn);
                        
                        battle.AddVisibleBattleAction(VisibleBattleActionType.HasStatus, new List<object>
                        {
                            i,
                            j,
                            burn
                        });
                        
                        battle.AddVisibleBattleAction(VisibleBattleActionType.DamageDealt, new List<object>
                        {
                            i,
                            j,
                            damageTaken,
                            DamageSource.Burn,
                            0,
                            false
                        });
                    }
                }
            }
        }
    }
}