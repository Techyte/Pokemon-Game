using UnityEngine;
using System.Collections.Generic;
using System;
using System.Reflection;
using PokemonGame.Battle;

namespace PokemonGame
{
    [CreateAssetMenu(fileName = "New All Moves", menuName = "All/New All Moves")]
    public class AllMoves : ScriptableObject, ISerializationCallbackReceiver
    {
        public List<string> keys = new List<string>();
        public List<Move> values = new List<Move>();

        public Dictionary<string, Move> moves = new Dictionary<string, Move>();

        public Move moveToAdd;

        public void OnBeforeSerialize()
        {
            keys.Clear();
            values.Clear();

            foreach (var kvp in moves)
            {
                keys.Add(kvp.Key);
                values.Add(kvp.Value);
            }
        }

        public void OnAfterDeserialize()
        {
            moves = new Dictionary<string, Move>();

            for (int i = 0; i != Math.Min(keys.Count, values.Count); i++)
                moves.Add(keys[i], values[i]);
        }

        private void OnValidate()
        {
            foreach (var item in moves)
            {
                string name = item.Key;
                moves[name].moveMethod = GetByNameMove(typeof(StatusMovesMethods), moves[name].name);
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
    }

}