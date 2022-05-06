using System.Reflection;
using UnityEngine;
using System;
using PokemonGame.Battle;

namespace PokemonGame
{
    public class StatusSystemInitilizer : MonoBehaviour
    {
        public AllMoves allMoves;
        public AllStatusEffects allStatusEffects;
        public AllAis allEnemyAIs;
        public StatusMovesMethods moveMethods;
        public StatusEffectsMethods effectMethods;
        public EnemyAIMethods enemyAiMethods;

        private void Start()
        {
            foreach (var item in allMoves.moves)
            {
                string name = item.Key;
                allMoves.moves[name].moveMethod = GetByNameMove(moveMethods, allMoves.moves[name].name);
            }

            foreach (var item in allStatusEffects.effects)
            {
                string name = item.Key;
                allStatusEffects.effects[name].effect = GetByNameEffect(effectMethods, allStatusEffects.effects[name].name);
            }

            foreach (var item in allEnemyAIs.ais)
            {
                string name = item.Key;
                allEnemyAIs.ais[name].aiMethod = GetByNameAI(enemyAiMethods, allEnemyAIs.ais[name].name);
            }
        }

        private Move.MoveMethod GetByNameMove(object target, string methodName)
        {
            MethodInfo method = target.GetType()
                .GetMethod(methodName,
                           BindingFlags.Public
                           | BindingFlags.Instance
                           | BindingFlags.FlattenHierarchy);


            // Insert appropriate check for method == null here

            return (Move.MoveMethod)Delegate.CreateDelegate
                (typeof(Move.MoveMethod), target, method);
        }

        private StatusEffect.Effect GetByNameEffect(object target, string methodName)
        {
            MethodInfo method = target.GetType()
                .GetMethod(methodName,
                           BindingFlags.Public
                           | BindingFlags.Instance
                           | BindingFlags.FlattenHierarchy);

            // Insert appropriate check for method == null here

            return (StatusEffect.Effect)Delegate.CreateDelegate
                (typeof(StatusEffect.Effect), target, method);
        }

        private EnemyAI.AIMethod GetByNameAI(object target, string methodName)
        {
            MethodInfo method = target.GetType()
                .GetMethod(methodName,
                           BindingFlags.Public
                           | BindingFlags.Instance
                           | BindingFlags.FlattenHierarchy);

            // Insert appropriate check for method == null here

            return (EnemyAI.AIMethod)Delegate.CreateDelegate
                (typeof(EnemyAI.AIMethod), target, method);
        }
    }

}