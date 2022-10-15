using UnityEngine;
using System.Collections.Generic;
using System;

namespace PokemonGame.ScriptableObjects
{
    /// <summary>
    /// A collection of every move in the game
    /// </summary>
    [CreateAssetMenu(fileName = "New All Moves", menuName = "All/New All Moves")]
    public class AllMoves : ScriptableObject
    {
        public List<string> keys = new List<string>();
        public List<Move> values = new List<Move>();

        /// <summary>
        /// The list of every move
        /// </summary>
        public static Dictionary<string, Move> moves = new Dictionary<string, Move>();

        public Move moveToAdd;

        /// <summary>
        /// Attempts to get a move from the register and handles errors
        /// </summary>
        /// <param name="MoveName">The name of the move that you want to fetch</param>
        /// <param name="move">tThe outputted move</param>
        public static bool GetMove(string MoveName, out Move move)
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

        private void OnValidate()
        {
            moves = new Dictionary<string, Move>();

            for (int i = 0; i != Math.Min(keys.Count, values.Count); i++)
                moves.Add(keys[i], values[i]);
        }
    }

}