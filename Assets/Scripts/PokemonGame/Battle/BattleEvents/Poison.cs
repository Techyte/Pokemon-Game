using System;
using PokemonGame.General;
using PokemonGame.Global;
using PokemonGame.ScriptableObjects;

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
                        
                    }
                }
            }
        }
    }
}