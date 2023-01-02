using PokemonGame.Game.Trainers;
using UnityEngine;

namespace PokemonGame.Game
{
    public class GameLoader : MonoBehaviour
    {
        public Player player;
    
        private void Awake()
        {
            if(SceneLoader.sceneLoadedFrom == "Battle")
            {
                LoadGameFromBattle();
            }
        }

        private void LoadGameFromBattle()
        {
            string trainerName = (string)SceneLoader.vars[1];
            Vector3 newPos = (Vector3)SceneLoader.vars[2];
            Quaternion newRot = (Quaternion)SceneLoader.vars[3];
            bool isDefeated = (bool)SceneLoader.vars[4];
            Vector3 playerPos = (Vector3)SceneLoader.vars[5];
                
            if (isDefeated)
            {
                TrainerRegister.SetInfoWith(trainerName, true);

                GameObject.Find(trainerName).GetComponent<Trainer>().Defeated(newPos, newRot);

                player.transform.position = playerPos;
            }
        }
    }
}