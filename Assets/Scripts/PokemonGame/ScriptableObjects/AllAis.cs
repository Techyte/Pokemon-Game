using UnityEngine;
using System.Collections.Generic;
using System;

namespace PokemonGame.ScriptableObjects
{
    /// <summary>
    /// A collection of every ai in the game
    /// </summary>
    [CreateAssetMenu(fileName = "New All Ais", menuName = "All/New All Ais")]
    public class AllAis : ScriptableObject
    {
        public List<string> keys = new List<string>();
        public List<EnemyAI> values = new List<EnemyAI>();

        /// <summary>
        /// The list of every ai
        /// </summary>
        public static Dictionary<string, EnemyAI> ais = new Dictionary<string, EnemyAI>();

        public EnemyAI aiToAdd;

        /// <summary>
        /// Attempts to get an ai from the register and handles errors
        /// </summary>
        /// <param name="AiName">The name of the ai that you want to fetch</param>
        /// <param name="ai">tThe outputted ai</param>
        public static bool GetAi(string AiName, out EnemyAI ai)
        {
            ai = null;
            if (ais.TryGetValue(AiName, out EnemyAI aiToReturn))
            {
                ai = aiToReturn;
                return true;
            }
            
            Debug.LogWarning("Ai was not present in the register");
            return false;
        }

        private void OnValidate()
        {
            ais = new Dictionary<string, EnemyAI>();

            for (int i = 0; i != Math.Min(keys.Count, values.Count); i++)
                ais.Add(keys[i], values[i]);
        }
    }
}