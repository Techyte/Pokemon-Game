using System.Linq;
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
            player.position = (Vector3) SceneLoader.vars[0];
            if((bool)SceneLoader.vars[1])
                GameObject.Find((string) SceneLoader.vars[2]).GetComponent<BattleStarter>().isDefeated = (bool) SceneLoader.vars[3];
            SceneLoader.ClearLoader();
        }
    }
}
