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
            string trainerName = (string)SceneLoader.GetVariable("trainerName");
            Vector3 playerPos = (Vector3)SceneLoader.GetVariable("playerPos");
            bool isDefeated = (bool)SceneLoader.GetVariable("isDefeated");
                
            if (isDefeated)
            {
                GameObject.Find(trainerName).GetComponent<Trainer>().Defeated();
                
                player.transform.position = playerPos;
            }
        }
    }
}