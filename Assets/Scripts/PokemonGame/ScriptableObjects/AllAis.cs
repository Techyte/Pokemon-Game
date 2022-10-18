using UnityEngine;
using System.Collections.Generic;
using System;

namespace PokemonGame.ScriptableObjects
{
    /// <summary>
    /// A collection of every ai in the game
    /// </summary>
    [CreateAssetMenu(fileName = "New All Ais", menuName = "All/New All Ais")]
    public class AllAis : ScriptableObject, ISerializationCallbackReceiver
    {
        public List<string> _keys = new List<string>( );
        public List<EnemyAI> _values = new List<EnemyAI>();
        
        /// <summary>
        /// The list of every ai
        /// </summary>
        public Dictionary<string, EnemyAI> ais = new Dictionary<string, EnemyAI>();

        public EnemyAI aiToAdd;

        /// <summary>
        /// Attempts to get an ai from the register and handles errors
        /// </summary>
        /// <param name="AiName">The name of the ai that you want to fetch</param>
        /// <param name="ai">tThe outputted ai</param>
        public bool GetAi(string AiName, out EnemyAI ai)
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

        /// <summary>
        /// Adds an ai to to the register
        /// </summary>
        /// <param name="aiToAdd">The ai you want to add</param>
        public void AddMove(EnemyAI aiToAdd)
        {
            if (!ais.ContainsKey(aiToAdd.name))
            {
                ais.Add(aiToAdd.name, aiToAdd);
            }
            else
            {
                Debug.LogWarning("AI is already in the list, please do not try and add it again");
            }
        }
        
        public void OnBeforeSerialize()
        {
            _keys.Clear();
            _values.Clear();

            foreach (var kvp in ais)
            {
                _keys.Add(kvp.Key);
                _values.Add(kvp.Value);
            }
        }

        public void OnAfterDeserialize()
        {
            ais = new Dictionary<string, EnemyAI>();

            for (int i = 0; i != Math.Min(_keys.Count, _values.Count); i++)
                ais.Add(_keys[i], _values[i]);
        }
    }
}