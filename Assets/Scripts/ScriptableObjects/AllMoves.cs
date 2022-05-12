using UnityEngine;
using System.Collections.Generic;
using System;
using PokemonGame.Battle;

namespace PokemonGame
{
    public class AllMoves : ScriptableObject, ISerializationCallbackReceiver
    {
        public List<string> _keys = new List<string>();
        public List<Move> _values = new List<Move>();

        public Dictionary<string, Move> moves = new Dictionary<string, Move>();

        public Move MoveToAdd;

        public void OnBeforeSerialize()
        {
            _keys.Clear();
            _values.Clear();

            foreach (var kvp in moves)
            {
                _keys.Add(kvp.Key);
                _values.Add(kvp.Value);
            }
        }

        public void OnAfterDeserialize()
        {
            moves = new Dictionary<string, Move>();

            for (int i = 0; i != Math.Min(_keys.Count, _values.Count); i++)
                moves.Add(_keys[i], _values[i]);
        }
    }

}