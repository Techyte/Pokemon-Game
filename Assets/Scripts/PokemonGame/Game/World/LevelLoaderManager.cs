using System.Collections;
using PokemonGame.Global;
using UnityEngine.SceneManagement;

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
            Debug.Log(SceneLoader.sceneLoadedFrom);
            if ((SceneLoader.sceneLoadedFrom != "Battle" && SceneLoader.sceneLoadedFrom != "Boot") || (SceneManager.GetActiveScene().name == "Poke Center" && SceneLoader.sceneLoadedFrom == "Battle"))
            {
                Debug.Log("Loading into an actual position");
                string loaderName = SceneLoader.GetVariable<string>("loaderName");
                Debug.Log(loaderName);

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
            else if(SceneLoader.sceneLoadedFrom == "Boot")
            {
                UseDefaultSpawn();
            }
        }

        private void UseDefaultSpawn()
        {
            Debug.Log("Using default spawn point");

            if (useDefaultRotation)
            {
                Player.Instance.SetPosRot(spawnPoint.position, spawnPoint.rotation);
            }
            else
            {
                Player.Instance.SetPosRot(spawnPoint.position, Player.Instance.transform.rotation);
            }
        }
    }

    public enum TransitionType
    {
        Circle,
        Spiky
    }
}