using System.Collections.Generic;
using UnityEngine;

namespace PokemonGame.Game.Trainers
{
    /// <summary>
    /// Class that manages trainers
    /// </summary>
    public class TrainerRegister
    {
        private static List<Trainer> _defeatedTrainers = new List<Trainer>();

        /// <summary>
        /// Gets whether the trainer is in the list of defeated trainer
        /// </summary>
        /// <param name="trainer">The trainer that you want to test for</param>
        /// <returns></returns>
        public static bool IsDefeated(Trainer trainer)
        {
            if (_defeatedTrainers.Contains(trainer))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Adds the trainer to the list of defeated trainers
        /// </summary>
        /// <param name="trainer">The trainer that was defeated</param>
        public static void Defeated(Trainer trainer)
        {
            _defeatedTrainers.Add(trainer);
        }
    } 
}