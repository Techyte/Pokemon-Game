namespace PokemonGame.Game.World
{
    using Party;
    using Trainers;
    using Global;
    using UnityEngine;

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
            PartyManager.Instance.UpdatePlayerParty((Party)SceneLoader.GetVariable("playerParty"));
            string trainerName = (string)SceneLoader.GetVariable("trainerName");
            Vector3 playerPos = (Vector3)SceneLoader.GetVariable("playerPos");
            Quaternion playerRotation = (Quaternion)SceneLoader.GetVariable("playerRotation");
            bool isDefeated = (bool)SceneLoader.GetVariable("isDefeated");
                
            if (isDefeated)
            {
                GameObject.Find(trainerName).GetComponent<Trainer>().Defeated();
                
                player.transform.position = playerPos;
                player.transform.rotation = playerRotation;
            }
        }
    }    
}