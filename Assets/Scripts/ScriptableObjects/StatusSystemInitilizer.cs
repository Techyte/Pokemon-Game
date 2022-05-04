using System.Reflection;
using UnityEngine;
using System;

public class StatusSystemInitilizer : MonoBehaviour
{
    public Move[] statusMoves;
    public StatusEffect[] statusEffects;
    public EnemyAI[] enemyAis;
    public StatusMovesMethods moveMethods;
    public StatusEffectsMethods effectMethods;
    public EnemyAIMethods enemyAiMethods;

    void Start()
    {
        for(int i = 0; i < statusMoves.Length; i++)
        {
            statusMoves[i].moveMethod = GetByNameMove(moveMethods, statusMoves[i].name);
        }

        for (int i = 0; i < statusEffects.Length; i++)
        {
            statusEffects[i].effect = GetByNameEffect(effectMethods, statusEffects[i].name);
        }

        for (int i = 0; i < enemyAis.Length; i++)
        {
            enemyAis[i].aiMethod = GetByNameAI(enemyAiMethods, enemyAis[i].name);
        }
    }

    StatusEffect.Effect GetByNameEffect(object target, string methodName)
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

    Move.MoveMethod GetByNameMove(object target, string methodName)
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

    EnemyAI.AIMethod GetByNameAI(object target, string methodName)
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
