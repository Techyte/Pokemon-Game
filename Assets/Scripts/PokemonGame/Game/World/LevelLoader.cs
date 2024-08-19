namespace PokemonGame.Game.World
{
    using UnityEngine;
    using Global;
    using System.Collections.Generic;

    public class LevelLoader : MonoBehaviour
    {
        [SerializeField] private string levelName;
        [SerializeField] private string connectingLoaderName;
        [SerializeField] private bool useSpawnPointRotation = true;
        [SerializeField] private Transform spawnPoint;
        
        private void OnTriggerEnter(Collider other)
        {
            Dictionary<string, object> vars = new Dictionary<string, object>
            {
                { "loaderName", connectingLoaderName },
            };

            SceneLoader.LoadScene(levelName, vars);
        }

        public void SpawnFrom()
        {
            if (useSpawnPointRotation)
            {
                Player.Instance.SetPosRot(spawnPoint.position, spawnPoint.rotation);
            }
            else
            {
                Player.Instance.SetPosRot(spawnPoint.position, Player.Instance.transform.rotation);
            }
        }
    }
}