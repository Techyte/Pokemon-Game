using System;
using System.Collections.Generic;

namespace PokemonGame.Battle
{
    using Game;
    using ScriptableObjects;
    
    [Serializable]
    public class ItemUsage : BattleEvent
    {
        public override void Event(Battle battle)
        {
            for (int i = 0; i < battle.playerActions.Count; i++)
            {
                if (battle.playerActions[i].Type == BattleActionType.Item)
                {
                    // this player wants to use an item

                    Item itemToUse = (Item)battle.playerActions[i].Variables[0];
                    int playerTarget = (int)battle.playerActions[i].Variables[1];
                    int battlerTarget = (int)battle.playerActions[i].Variables[2];
                    
                    UseItem(battle, itemToUse, i, playerTarget, battlerTarget);
                }
            }
        }

        public static void UseItem(Battle battle, Item item, int playerId, int playerTarget, int battlerTarget)
        {
            ItemMethodEventArgs e = new ItemMethodEventArgs(battle.activeBattlers[playerTarget][battlerTarget], item);
            
            item.ItemMethod(e);
            
            Bag.Used(item);
            
            battle.AddVisibleBattleAction(VisibleBattleActionType.Item, new List<object>{
                playerId,
                item,
                playerTarget,
                battlerTarget,
                e.success
            });
        }
    }
}