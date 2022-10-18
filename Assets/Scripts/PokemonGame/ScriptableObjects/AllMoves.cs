using UnityEngine;
using System.Collections.Generic;
using System;

namespace PokemonGame.ScriptableObjects
{
    /// <summary>
    /// A collection of every move in the game
    /// </summary>
    [CreateAssetMenu(fileName = "New All Moves", menuName = "All/New All Moves")]
    public class AllMoves : ScriptableObject, ISerializationCallbackReceiver
    {
        public List<string> _keys = new List<string>( );
        public List<Move> _values = new List<Move>();

        /// <summary>
        /// The list of every move
        /// </summary>
        public Dictionary<string, Move> moves = new Dictionary<string, Move>();

        public Move moveToAdd;

        /// <summary>
        /// Attempts to get a move from the register and handles errors
        /// </summary>
        /// <param name="MoveName">The name of the move that you want to fetch</param>
        /// <param name="move">tThe outputted move</param>
        public bool GetMove(string MoveName, out Move move)
        {
            move = null;
            if (moves.TryGetValue(MoveName, out Move moveToReturn))
            {
                move = moveToReturn;
                return true;
            }
            
            Debug.LogWarning("Move was not present in the register");
            return false;
        }

        /// <summary>
        /// Adds a move to to the register
        /// </summary>
        /// <param name="moveToAdd">The move you want to add</param>
        public void AddMove(Move moveToAdd)
        {
            if (!moves.ContainsKey(moveToAdd.name))
            {
                moves.Add(moveToAdd.name, moveToAdd);
            }
            else
            {
                Debug.LogWarning("Move is already in the list, please do not try and add it again");
            }
        }
        
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