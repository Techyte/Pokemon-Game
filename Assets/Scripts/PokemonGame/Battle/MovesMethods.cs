using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PokeApiNet;
using PokemonGame.General;
using UnityEditor;
using PokemonGame.Dialogue;

namespace PokemonGame.Battle
{
    using Global;
    using UnityEngine;
    using ScriptableObjects;

    /// <summary>
    /// Contains all the logic for every move
    /// </summary>
    [CreateAssetMenu(fileName = "New Moves Methods", menuName = "All/New Moves Methods")]
    public class MovesMethods : ScriptableObject
    {
        public static int CalculateDamage(Move move, Battler battlerThatUsed, Battler battlerBeingAttacked, out int effectiveIndex, out bool hitCrit, bool canCrit = true)
        {
            //Damage calculation equation from: https://bulbapedia.bulbagarden.net/wiki/Damage#Generation_II
            
            int damage = 0;
            
            hitCrit = false;
            effectiveIndex = 0;
            
            //Checking to see if the move is capable of hitting the opponent battler
            foreach (var hType in move.type.cantHit)
            {
                if (hType == Type.FromBasic(battlerBeingAttacked.source.GetPrimaryType()) || hType == Type.FromBasic(battlerBeingAttacked.source.GetSecondaryType()))
                {
                    Debug.Log(move.type + " can't hit that battler");
                    effectiveIndex = 3;
                    return 0;
                }
            }

            float type = 1;

            //Calculating type disadvantages
            foreach (var weakType in move.type.weakAgainst)
            {
                if (weakType == Type.FromBasic(battlerBeingAttacked.source.GetPrimaryType()))
                {
                    type /= 2;
                }
                if (weakType == Type.FromBasic(battlerBeingAttacked.source.GetSecondaryType()))
                {
                    type /= 2;
                }
            }

            //Calculating type advantages
            foreach (var strongType in move.type.strongAgainst)
            {
                if (strongType == Type.FromBasic(battlerBeingAttacked.source.GetPrimaryType()))
                {
                    type *= 2;
                }
                if (strongType == Type.FromBasic(battlerBeingAttacked.source.GetSecondaryType()))
                {
                    type *= 2;
                }
            }

            //Failsafe
            if (type > 4)
                type = 4;
            if (type < .25f)
                type = .25f;

            //STAB =  Same type attack bonus
            int stab = 1;
            if (move.type == Type.FromBasic(battlerThatUsed.source.GetPrimaryType()))
            {
                stab = 2;
            }

            float attack = 0;
            float defense = 0;
            int level = battlerThatUsed.level;
            int power = move.damage;
            int item = 1;
            float critical = 1;
            int TK = 1;
            int weather = 1;
            // requires implementation of badges and gyms
            int badge = 1;
            int moveMod = 1;
            int doubleDmg = 1;
            float targets = 1;
            float pb = 1;
            float glaiveRush = 1;
            float burn = 1;

            if (move.category == MoveCategory.Physical && battlerThatUsed.statusEffect == Registry.GetStatusEffect("Burn"))
            {
                burn = 0.5f;
            }

            float other = 1;

            float zMove = 1;

            float random = Random.Range(0.85f, 1f);
            
            if (move.category == MoveCategory.Physical)
            {
                attack = battlerThatUsed.stats.attack * StatStages.GetMultiplierFromStage(battlerThatUsed.modifierStats.attackStage, false, false);
                defense = battlerBeingAttacked.stats.defense * StatStages.GetMultiplierFromStage(battlerBeingAttacked.modifierStats.defenseStage, false, false);
            }
            else if (move.category == MoveCategory.Special)
            {
                attack = battlerThatUsed.stats.specialAttack * StatStages.GetMultiplierFromStage(battlerThatUsed.modifierStats.specialAttackStage, false, false);
                defense = battlerBeingAttacked.stats.specialDefense * StatStages.GetMultiplierFromStage(battlerBeingAttacked.modifierStats.specialDefenseStage, false, false);
            }
            
            // critical hit calc

            int critStage = 1;
            
            if (move.increasedCritChance)
            {
                critStage += 1;
            }

            switch (critStage)
            {
                case 1:
                    hitCrit = Random.Range(1, 25) == 1;
                    break;
                case 2:
                    hitCrit = Random.Range(1, 9) == 1;
                    break;
                case 3:
                    hitCrit = Random.Range(1, 3) == 1;
                    break;
            }

            if (critStage >= 4)
            {
                hitCrit = true;
            }

            if (hitCrit && canCrit)
            {
                critical = 1.5f;
            }

            if (type > 1)
            {
                effectiveIndex = 2;
            }else if (type < 1)
            {
                effectiveIndex = 1;
            }
            else if (Mathf.Approximately(type, 1))
            {
                effectiveIndex = 0;
            }

            if (move.category == MoveCategory.Special)
            {
                effectiveIndex = 0;
            }

            damage = Mathf.RoundToInt(((((2f * level) / 5) * power * (attack / (float)defense)) / 50) + 2 * targets * pb * weather *
                glaiveRush * critical * random * stab * type * burn * other * zMove);

            int randomness = Mathf.RoundToInt(Random.Range(.8f * damage, damage * 1.2f));
            damage = randomness;

            return damage;
        }

        public void UpdateMoves()
        {
            UpdateMovesInfo();

#if UNITY_EDITOR
            AssetDatabase.SaveAssets();
#endif
        }

        private async void UpdateMovesInfo()
        {
            await UpdateMovesInfoTask();
        }

        private static async Task UpdateMovesInfoTask()
        {
            PokeApiClient pokeClient = new PokeApiClient();
            
            List<Move> moves = Resources.FindObjectsOfTypeAll<Move>().ToList();
            
            List<Move> movesToDelete = new List<Move>();
            
            foreach (var pokeMove in moves)
            {
                Debug.Log($"updating {pokeMove.name}");

                try
                {
                    PokeApiNet.Move move =
                        await pokeClient.GetResourceAsync<PokeApiNet.Move>(pokeMove.name.ToLower());

                    switch (move.Generation.Name)
                    {
                        case "generation-viii":
                            movesToDelete.Add(pokeMove);
                            break;
                        case "generation-ix":
                            movesToDelete.Add(pokeMove);
                            break;
                    }
                }
                catch (Exception e)
                {
                    Debug.LogWarning($"Could not find an entry for move: {pokeMove.name}");
                }
            }

#if UNITY_EDITOR
            foreach (var move in movesToDelete)
            {
                AssetDatabase.DeleteAsset($"Assets/Resources/Pokemon Game/Move/{move.type.name}/{move.name}");
            }
#endif
        }

        public static MovesMethods GetMoveMethods()
        {
            return Resources.Load<MovesMethods>("Pokemon Game/Move/Move Methods");
        }

        public int DefaultMoveMethod(MoveStatus e, int targetIndex)
        {
            Battler target = e.GetTarget(targetIndex);
            if (e.Move.category != MoveCategory.Status)
            {
                int damageDealt = CalculateDamage(e.Move, e.Attacker, target,
                    out int effectiveIndex, out bool crit);

                BattlerDamageSource source = new BattlerDamageSource(e.Attacker);
                
                e.Battle.AddVisibleBattleAction(VisibleBattleActionType.DamageDealt, new List<object>
                {
                    e.Targets[targetIndex].Item1,
                    e.Targets[targetIndex].Item2,
                    source,
                    effectiveIndex,
                    crit
                });
                
                target.TakeDamage(damageDealt, source);
                return damageDealt;
            }

            return 0;
        }
        
        public void Toxic(MoveStatus e, int targetIndex)
        {
            Battler target = e.GetTarget(targetIndex);
            target.statusEffect = Registry.GetStatusEffect("Poisoned");
            Battle.Singleton.QueDialogue($"{target.name} was poisoned!", DialogueBoxType.Event, "generalFinishing");
        }

        public void LeechLife(MoveStatus e, int targetIndex)
        {
            int damageDealt = DefaultMoveMethod(e, targetIndex);
            Battle.Singleton.QueDialogue($"{e.Attacker.name} healed {damageDealt/2} health!", DialogueBoxType.Event, "generalFinishing");
            e.Attacker.Heal(damageDealt/2);
        }

        public void SleepPowder(MoveStatus e, int targetIndex)
        {
            StatusEffect sleep = Registry.GetStatusEffect("Asleep");
            Battler target = e.GetTarget(targetIndex);
            if (target.BecomeAffectedBy(sleep))
            {
                target.statusTurns = Random.Range(1, 4);
                Battle.Singleton.QueDialogue($"{target.name} was put to sleep!", DialogueBoxType.Event, "generalFinishing");
            }
            else
            {
                e.Failures[targetIndex] = true;
            }
        }

        public void WillOWisp(MoveStatus e, int targetIndex)
        {
            StatusEffect burn = Registry.GetStatusEffect("Burn");
            Battler target = e.GetTarget(targetIndex);
            if (target.statusEffect == burn)
            {
                e.Failures[targetIndex] = true;
            }
            else
            {
                target.statusEffect = Registry.GetStatusEffect("Burn");
                Battle.Singleton.QueDialogue($"{target.name} was burned!", DialogueBoxType.Event, "generalFinishing");
            }
        }

        public void BadTime(MoveStatus e, int targetIndex)
        {
            Battler target = e.GetTarget(targetIndex);
            Battle.Singleton.QueDialogue($"{target.name} is going to have a very Bad Time", DialogueBoxType.Event, "generalFinishing");
            DefaultMoveMethod(e, targetIndex);
        }
    }
}