using PokemonGame.Global;

namespace PokemonGame.Game.World
{
    using UnityEngine;

    public class LevelLoaderManager : MonoBehaviour
    {
        [Header("Default Spawn Point")]
        [SerializeField] private bool useDefaultRotation = true;
        [SerializeField] private Transform spawnPoint;
        
        private void Start()
        {
            if (SceneLoader.sceneLoadedFrom != "Battle" && SceneLoader.sceneLoadedFrom != "Boot")
            {
                string loaderName = (string)SceneLoader.GetVariable("loaderName");

                Transform spawnPointObject;
                
                if (!string.IsNullOrEmpty(loaderName))
                {
                    spawnPointObject = transform.Find(loaderName);   
                }
                else
                {
                    UseDefaultSpawn();
                    return;
                }

                if (spawnPointObject)
                {
                    LevelLoader loader = spawnPointObject.GetComponent<LevelLoader>();
                    loader.SpawnFrom();   
                }
                else
                {
                    UseDefaultSpawn();
                }
            }
            else
            {
                UseDefaultSpawn();
            }
        }

        private void UseDefaultSpawn()
        {
            Player.Instance.transform.position = spawnPoint.position;

            if (useDefaultRotation)
            {
                Player.Instance.transform.rotation = spawnPoint.rotation;
            }
        }
    }   
}