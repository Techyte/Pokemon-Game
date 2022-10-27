using UnityEngine;

namespace PokemonGame.Game
{
    public class GameLoader : MonoBehaviour
    {
        public Player player;
    
        private void Awake()
        {
            if(SceneLoader.vars.Count == 0) return;
            LoadGameFromBattle();
        }

        private void LoadGameFromBattle()
        {
            //PartyManager.singleton.UpdatePlayerParty((Party) SceneLoader.vars[0]);
            
            
            player.transform.position = (Vector3) SceneLoader.vars[2];
            if ((bool)SceneLoader.vars[3])
            {
                Trainer[] starters = FindObjectsOfType<Trainer>();
                foreach (Trainer trainer in starters)
                {
                    Debug.Log("Found a starter");
                    if (trainer.id == (int)SceneLoader.vars[4])
                    {
                        Debug.Log("Found a starter with the id");
                        if ((bool)SceneLoader.vars[5])
                        {
                            Debug.Log("Was defeated");
                            trainer.Defeated((Vector3)SceneLoader.vars[6]);
                        }
                    }
                }
            }
            SceneLoader.ClearLoader();
            Debug.Log("Loaded");
        }
    }
}