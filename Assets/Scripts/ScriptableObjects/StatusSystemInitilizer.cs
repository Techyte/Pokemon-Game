using System.Reflection;
using UnityEngine;
using System;
using PokemonGame.Battle;

namespace PokemonGame
{
    public class StatusSystemInitilizer : MonoBehaviour
    {
        public Move[] allMoves;
        public StatusEffect[] allStatusEffects;
        public EnemyAI[] allEnemyAIs;
        public StatusMovesMethods moveMethods;
        public StatusEffectsMethods effectMethods;
        public EnemyAIMethods enemyAiMethods;

        private void Start()
        {
            for(int i = 0; i < allMoves.Length; i++)
            {
                allMoves[i].moveMethod = GetByNameMove(moveMethods, allMoves[i].name);
            }

            for (int i = 0; i < allStatusEffects.Length; i++)
            {
                allStatusEffects[i].effect = GetByNameEffect(effectMethods, allStatusEffects[i].name);
            }

            for (int i = 0; i < allEnemyAIs.Length; i++)
            {
                allEnemyAIs[i].aiMethod = GetByNameAI(enemyAiMethods, allEnemyAIs[i].name);
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