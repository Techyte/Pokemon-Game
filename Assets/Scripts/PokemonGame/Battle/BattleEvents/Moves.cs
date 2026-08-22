using System;
using System.Collections.Generic;
using PokemonGame.General;
using PokemonGame.Global;
using PokemonGame.ScriptableObjects;
using Random = UnityEngine.Random;

namespace PokemonGame.Battle
{
    [Serializable]
    public class Moves : BattleEvent
    {
        public List<BattleEvent> nestedMoveEvents;
        
        public override void Event(Battle battle)
        {
            List<(int, int, int, float)> moves = new List<(int, int, int, float)>(); // (playerIndex, actionIndex, movePriority, attackerSpeed)
            
            for (int i = 0; i < battle.playerActions.Count; i++)
            {
                for (int j = 0; j < battle.playerActions[i].Count; j++)
                {
                    if (battle.playerActions[i][j].Type == BattleActionType.Move)
                    {
                        // have identified that we have a move to use
                        // now we need to put it into the right part of the list
                        // i is the playerIndex
                        // j is the action (or battler) index

                        int moveId = (int)battle.playerActions[i][j].Variables[0];
                        
                        Move move = battle.activeBattlers[i][j].moves[moveId];
                        Battler attacker = battle.activeBattlers[i][j];

                        float attackerModSpeed = RealSpeed(attacker);

                        if (moves.Count > 0)
                        {
                            bool inserted = false;
                            for (int k = 0; k < moves.Count; k++)
                            {
                                if (moves[k].Item3 < move.priority) // if we have a higher priority
                                {
                                    moves.Insert(k, (i, j, move.priority, attackerModSpeed));
                                    inserted = true;
                                    break;
                                }
                                else if (moves[k].Item3 == move.priority) // if we have the same priority
                                {
                                    if (moves[k].Item4 > attackerModSpeed) // they have the higher speed
                                    {
                                        moves.Insert(k+1, (i, j, move.priority, attackerModSpeed));
                                    }
                                    else if (moves[k].Item4 < attackerModSpeed) // we have the higher speed
                                    {
                                        moves.Insert(k, (i, j, move.priority, attackerModSpeed));
                                    }
                                    else // speed equal, so just pick random
                                    {
                                        int insertionPoint = Random.Range(0, 2);
                                        
                                        moves.Insert(k+insertionPoint, (i, j, move.priority, attackerModSpeed));
                                    }

                                    break;
                                }
                            }

                            if (!inserted)
                            {
                                moves.Add((i, j, move.priority, attackerModSpeed));
                            }
                        }
                        else
                        {
                            moves.Add((i, j, move.priority, attackerModSpeed));
                        }
                    }
                }
            }
            
            // at this point we SHOULD have a sorted list of moves in order or priority and speed, picking a random one when all else fails

            foreach (var move in moves)
            {
                SimulateMove(battle, move.Item1, move.Item2);
            }
        }

        private float RealSpeed(Battler battler)
        {
            float playerAdjustedSpeed = battler.stats.speed * StatStages.GetMultiplierFromStage(battler.modifierStats.speedStage, false, false);

            if (battler.statusEffect == Registry.GetStatusEffect("Paralysed"))
            {
                playerAdjustedSpeed /= 2;
            }

            return playerAdjustedSpeed;
        }

        private void SimulateMove(Battle battle, int playerIndex, int actionIndex)
        {
            int moveId = (int)battle.playerActions[playerIndex][actionIndex].Variables[0];
            int targetPlayer = (int)battle.playerActions[playerIndex][actionIndex].Variables[1];
            int targetBattler = (int)battle.playerActions[playerIndex][actionIndex].Variables[2];

            List<object> vars = new List<object>
            {
                moveId,
                targetPlayer,
                targetBattler
            };

            foreach (var moveEvent in nestedMoveEvents)
            {
                MoveStatus currentStatus =
                    new MoveStatus(playerIndex, actionIndex, moveId, targetPlayer, targetBattler);

                battle.AddVisibleBattleAction(VisibleBattleActionType.MoveUsed, new List<object> { currentStatus });
                moveEvent.Event(battle, vars);
            }
        }
    }

    public class MoveStatus
    {
        public int PlayerIndex;
        public int ActionIndex;
        public int MoveId;
        public int TargetPlayer;
        public int TargetBattler;

        public bool Failed;

        public MoveStatus(int playerIndex, int actionIndex, int moveId, int targetPlayer, int targetBattler)
        {
            PlayerIndex = playerIndex;
            ActionIndex =  actionIndex;
            MoveId = moveId;
            TargetPlayer = targetPlayer;
            TargetBattler = targetBattler;

            Failed = false;
        }
    }
}