using System.Collections.Generic;
using UnityEngine;

namespace PokemonGame.Game
{
    public class TrainerRegister
    {
        public static List<TrainerInfo> trainer = new List<TrainerInfo>();

        /// <summary>
        /// Gets a battle starter from the id you provide
        /// </summary>
        /// <param name="id">The id of the battle starter you want to get</param>
        /// <returns></returns>
        public static TrainerInfo GetBattleStarter(int id)
        {
            foreach (var starter in trainer)
            {
                Debug.LogWarning($"Comparing {id} with {starter.id}");
                if (starter.id == id)
                {
                    return starter;
                }
            }
            
            Debug.LogWarning($"Could not find a starter with id {id}, returning null");
            return null;
        }
        
        /// <summary>
        /// Adds a battle starter to the list
        /// </summary>
        /// <param name="starter">The starter you want to add</param>
        /// <returns></returns>
        public static int AddBattleStarter(Trainer starter)
        {
            int id = trainer.Count + 1;
            trainer.Add(new TrainerInfo(starter, id));
            return id;
        }
    }

    public class TrainerInfo
    {
        public int id;
        public bool isDefeated;
        public Vector3 position;

        public TrainerInfo(Trainer trainer, int id)
        {
            this.id = id;
            isDefeated = trainer.isDefeated;
            position = trainer.transform.position;
        }

        public void UpdatePosition(Vector3 newPos)
        {
            position = newPos;
        }

        public void UpdateIsDefeated(bool isDefeated)
        {
            this.isDefeated = isDefeated;
        }
    }
}