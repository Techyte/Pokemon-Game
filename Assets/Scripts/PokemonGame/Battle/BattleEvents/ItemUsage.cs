using System;
using System.Collections.Generic;
using PokemonGame.Game.Party;
using PokemonGame.General;

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
                for (int j = 0; j < battle.playerActions[i].Count; j++)
                {
                    if (battle.playerActions[i][j].Type == BattleActionType.Item)
                    {
                        // this player wants to use an item

                        Item itemToUse = (Item)battle.playerActions[i][j].Variables[0];
                        int playerTarget = (int)battle.playerActions[i][j].Variables[1];
                        int battlerTarget = (int)battle.playerActions[i][j].Variables[2];

                        if (itemToUse is PokeBall)
                        {
                            CatchAttempt(battle, (PokeBall)itemToUse, i, battlerTarget, playerTarget);
                        }
                        else
                        {
                            UseItem(battle, itemToUse, i, playerTarget, battlerTarget);
                        }
                    }
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
        
        public static void CatchAttempt(Battle battle, PokeBall ball, int playerId, int playerTarget,  int battlerTarget)
        {
            bool captured = ExperienceCalculator.Captured(battle.activeBattlers[playerTarget][battlerTarget],
                battle.activeBattlers[playerId][0],
                ball);

            if (captured)
            {
                PartyManager.AddBattler(battle.activeBattlers[playerTarget][battlerTarget]);
            }
            
            battle.AddVisibleBattleAction(VisibleBattleActionType.Catch, new List<object>
            {
                playerId,
                ball,
                playerTarget,
                battlerTarget,
            });
        }
    }
}