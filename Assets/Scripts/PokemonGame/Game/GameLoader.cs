namespace PokemonGame.Game
{
    using Party;
    using Trainers;
    using General;
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
            PartyManager.Instance.UpdatePlayerParty((Party.Party)SceneLoader.GetVariable("playerParty"));
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