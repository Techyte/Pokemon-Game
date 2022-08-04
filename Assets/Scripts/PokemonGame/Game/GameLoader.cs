using PokemonGame.Game;
using UnityEngine;

namespace PokemonGame.Battle
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
                BattleStarter[] starters = (BattleStarter[]) FindObjectsOfType(typeof(BattleStarter)); //returns Object[]
                foreach (var starter in starters)
                {
                    if (starter.battlerId == (int)SceneLoader.vars[4])
                    {
                        starter.isDefeated = (bool)SceneLoader.vars[5];
                    }
                }
            }
            SceneLoader.ClearLoader();
        }
    }
}
