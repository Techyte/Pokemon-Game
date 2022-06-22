using UnityEngine;
using System.Collections.Generic;
using System;
using System.Reflection;
using PokemonGame.Battle;

namespace PokemonGame
{
    [CreateAssetMenu(fileName = "New All Status Effects", menuName = "All/New All Status Effects")]
    public class AllStatusEffects : ScriptableObject, ISerializationCallbackReceiver
    {
        public List<string> keys = new List<string>();
        public List<StatusEffect> values = new List<StatusEffect>();

        public static Dictionary<string, StatusEffect> effects = new Dictionary<string, StatusEffect>();

        public StatusEffect effectToAdd;

        public void OnBeforeSerialize()
        {
            keys.Clear();
            values.Clear();

            foreach (var kvp in effects)
            {
                keys.Add(kvp.Key);
                values.Add(kvp.Value);
            }
        }

        public void OnAfterDeserialize()
        {
            effects = new Dictionary<string, StatusEffect>();

            for (int i = 0; i != Math.Min(keys.Count, values.Count); i++)
                effects.Add(keys[i], values[i]);
        }

        private void OnValidate()
        {
            foreach (var item in effects)
            {
                string name = item.Key;
                effects[name].effect = GetByNameEffect(typeof(StatusEffectsMethods), effects[name].name);
            }
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
    }

}