using UnityEngine;
using System.Collections.Generic;
using System;
using System.Reflection;
using PokemonGame.Battle;

namespace PokemonGame
{
    [CreateAssetMenu(fileName = "New All Ais", menuName = "All/New All Ais")]
    public class AllAis : ScriptableObject, ISerializationCallbackReceiver
    {
        public List<string> keys = new List<string>();
        public List<EnemyAI> values = new List<EnemyAI>();

        public Dictionary<string, EnemyAI> ais = new Dictionary<string, EnemyAI>();

        public EnemyAI aiToAdd;

        public void OnBeforeSerialize()
        {
            keys.Clear();
            values.Clear();

            foreach (var kvp in ais)
            {
                keys.Add(kvp.Key);
                values.Add(kvp.Value);
            }
        }

        public void OnAfterDeserialize()
        {
            ais = new Dictionary<string, EnemyAI>();

            for (int i = 0; i != Math.Min(keys.Count, values.Count); i++)
                ais.Add(keys[i], values[i]);
        }

        private void OnValidate()
        {
            foreach (var item in ais)
            {
                string name = item.Key;
                ais[name].aiMethod = GetByNameAI(typeof(EnemyAIMethods), ais[name].name);
            }
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