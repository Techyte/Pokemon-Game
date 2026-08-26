using System;
using UnityEngine;

namespace PokemonGame.Battle
{
    [Serializable]
    public class Switch : BattleEvent
    {
        public override string Name()
        {
            return "Switch";
        }

        public override void Event(Battle battle)
        {
            for (int i = 0; i < battle.playerActions.Count; i++)
            {
                for (int j = 0; j < battle.playerActions[i].Count; j++)
                {
                    if (battle.playerActions[i][j].Type == BattleActionType.Switch)
                    {
                        // this player wants to switch
                    
                        int switchingOut = (int)battle.playerActions[i][j].Variables[0];
                        int switchingIn = (int)battle.playerActions[i][j].Variables[1];

                        // range check
                        if (battle.players[i].Party.Count <= switchingOut)
                        {
                            Debug.LogWarning("Tried to switch to a battler outside of party, doing nothing");
                            return;
                        }
                        // existence check
                        if (battle.players[i].Party[switchingIn] == null)
                        {
                            Debug.LogWarning("Tried to switch to a battler that does not exist, doing nothing");
                            return;
                        }
                        // reasonableness check
                        if (battle.players[i].Party[switchingIn].isFainted)
                        {
                            Debug.LogWarning("Tried to switch to a battler that has fainted, doing nothing");
                            return;
                        }
                    
                        SwitchBattler(battle, i, switchingOut, switchingIn);
                    }
                }
            }
        }

        public static void SwitchBattler(Battle battle, int playerId, int switchingOut, int switchingIn)
        {
            battle.SetPlayerActiveBattlers(playerId, switchingOut, switchingIn);
            
            if (!battle.onlineBattle && playerId == 0)
            {
                // non online battle and this is 
                battle.AddParticipatedBattler(battle.players[playerId].Party[switchingIn]);
            }
        }
    }
}