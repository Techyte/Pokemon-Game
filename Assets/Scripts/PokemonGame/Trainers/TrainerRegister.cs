using System.Collections.Generic;

namespace PokemonGame.Trainers
{
    /// <summary>
    /// Class that manages trainers
    /// </summary>
    public static class TrainerRegister
    {
        private static List<string> _defeatedTrainers = new List<string>();

        /// <summary>
        /// Gets whether the trainer is in the list of defeated trainer
        /// </summary>
        /// <param name="trainer">The trainer that you want to test for</param>
        /// <returns></returns>
        public static bool IsDefeated(Trainer trainer)
        {
            if (_defeatedTrainers.Contains(trainer.name))
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
            _defeatedTrainers.Add(trainer.name);
        }
    } 
}