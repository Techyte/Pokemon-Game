using System.Reflection;
using UnityEngine;
using System;

public class StatusSystemInitilizer : MonoBehaviour
{
    public Move[] statusMoves;
    public StatusEffect[] statusEffects;

    void Start()
    {
        for(int i = 0; i < statusMoves.Length; i++)
        {
            StatusMovesMethods moveMethods = new StatusMovesMethods();

            statusMoves[i].moveMethod = GetByNameMove(moveMethods, statusMoves[i].name);
        }

        for (int i = 0; i < statusEffects.Length; i++)
        {
            StatusEffectsMethods effectMethods = new StatusEffectsMethods();

            statusEffects[i].effect = GetByNameEffect(effectMethods, statusEffects[i].name);
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
}
