using System.Collections.Generic;
using UnityEngine;

namespace PokemonGame.Game.Trainers
{
    /// <summary>
    /// Class that manages trainers
    /// </summary>
    public class TrainerRegister
    {
        private static Dictionary<string, bool> _trainerRegistry  = new Dictionary<string, bool>();

        /// <summary>
        /// Gets a trainer with the same name as what you provide
        /// </summary>
        /// <param name="name">The name of the trainer you want to get</param>
        /// <returns></returns>
        public static bool GetIsDefeatedDataFrom(string name)
        {
            Debug.Log(_trainerRegistry.Count);
            if (_trainerRegistry.TryGetValue(name, out bool isDefeated))
            {
                return isDefeated;
            }
            Debug.Log("Could not find a trainer with that id");
            return false;
        }

        /// <summary>
        /// Sets info about a trainer in the registry
        /// </summary>
        /// <param name="name">The name of the trainer you want to modify</param>
        /// <param name="info">The new information about the trainer</param>
        public static void SetInfoWith(string name, bool isDefeated)
        {
            if (_trainerRegistry.ContainsKey(name))
            {
                _trainerRegistry[name] = isDefeated;
            }
            else
            {
                Debug.LogWarning("Trainer with that Id does not exist");
            }
        }

        /// <summary>
        /// Registers a trainer with the registry so it can be loaded and modified
        /// </summary>
        /// <param name="trainer">The trainer you want to register</param>
        public static void RegisterTrainer(Trainer trainer)
        {
            if(_trainerRegistry.ContainsKey(trainer.name)) return;
            
            Debug.Log("Registering");
            
            _trainerRegistry.Add(trainer.name, false);
            
            Debug.Log(_trainerRegistry.Count);
        }
    } 
}