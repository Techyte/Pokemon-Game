using UnityEngine;

namespace PokemonGame.Game
{
    public class GameLoader : MonoBehaviour
    {
        [SerializeField] private Transform player;
    
        private void Start()
        {
            if(SceneLoader.vars.Count == 0) return;
            LoadGameFromBattle();
        }

        private void LoadGameFromBattle()
        {
            //PartyManager.singleton.UpdatePlayerParty((Party) SceneLoader.vars[0]);
            
            player.position = (Vector3) SceneLoader.vars[2];
            if ((bool)SceneLoader.vars[3])
            {
                BattleStarter[] starters = FindObjectsOfType<BattleStarter>();
                foreach (BattleStarter starter in starters)
                {
                    if (starter.battlerId == (int)SceneLoader.vars[4])
                    {
                        if ((bool)SceneLoader.vars[5])
                        {
                            starter.Defeated();
                        }
                    }
                }
            }
            SceneLoader.ClearLoader();
        }
    }
}
