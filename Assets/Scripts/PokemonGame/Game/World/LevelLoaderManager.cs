using PokemonGame.Global;

namespace PokemonGame.Game.World
{
    using UnityEngine;

    public class LevelLoaderManager : MonoBehaviour
    {
        private void Start()
        {
            if (SceneLoader.sceneLoadedFrom != "Battle" && SceneLoader.sceneLoadedFrom != "Boot")
            {
                string loaderName = (string)SceneLoader.GetVariable("loaderName");

                LevelLoader loader = transform.Find(loaderName).GetComponent<LevelLoader>();
                loader.SpawnFrom();
            }
        }
    }   
}