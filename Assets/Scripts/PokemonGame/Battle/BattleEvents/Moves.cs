using System.Collections.Generic;
using System.Runtime.Serialization;
using PokemonGame.General;
using PokemonGame.Global;
using PokemonGame.ScriptableObjects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PokemonGame.Battle
{
    public class Moves : BattleEvent/*, ISerializationCallbackReceiver*/
    {
        public override string Name { get; set; } = "Moves";
        
        public BattleSequence nestedMoveEvents;
        
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

                        int moveId = (int)battle.playerActions[i][j].Variables[1];
                        
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
            List<(int, int)> targets = (List<(int, int)>)battle.playerActions[playerIndex][actionIndex].Variables[0];
            int moveId = (int)battle.playerActions[playerIndex][actionIndex].Variables[1];
            
            Battler attacker = battle.activeBattlers[playerIndex][actionIndex];
            Move move = battle.activeBattlers[playerIndex][actionIndex].moves[moveId];

            List<object> vars = new List<object>
            {
                moveId,
                targets
            };

            foreach (var moveEvent in nestedMoveEvents.sequence)
            {
                MoveStatus currentStatus =
                    new MoveStatus(battle, playerIndex, actionIndex, attacker, moveId, move, targets);
                
                moveEvent.Event(battle, new List<object> { currentStatus });
                
                battle.AddVisibleBattleAction(VisibleBattleActionType.MoveUsed, vars);
            }
        }

        // public void OnBeforeSerialize()
        // {
        //     
        // }
        //
        // public void OnAfterDeserialize()
        // {
        //     
        // }
    }

    public class MoveStatus
    {
        public Battle Battle;
        public int PlayerIndex;
        public int ActionIndex;
        public int MoveId;
        public List<(int, int)> Targets;

        // have the other indicies but this is just easier in most cases to have a direct reference to the objects
        public Move Move;
        public Battler Attacker;

        // used when the USER failed to use the move
        public bool Failed;

        // used when the TARGET managed to avoid the move in some way
        public List<bool> Failures;

        public MoveStatus(Battle battle, int playerIndex, int actionIndex, Battler attacker, int moveId, Move move, List<(int, int)> targets)
        {
            Battle = battle;
            PlayerIndex = playerIndex;
            ActionIndex =  actionIndex;
            MoveId = moveId;
            Targets = targets;
            Move = move;
            Attacker = attacker;
            
            Failures = new List<bool>(targets.Count);

            Failed = false;
        }

        public Battler GetTarget(int index)
        {
            (int, int) target = Targets[index];

            return Battle.activeBattlers[target.Item1][target.Item2];
        }
    }
}