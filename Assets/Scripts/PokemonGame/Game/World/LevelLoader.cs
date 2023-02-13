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
            Player.Instance.transform.position = spawnPoint.position;

            if (useSpawnPointRotation)
            {
                Player.Instance.transform.rotation = spawnPoint.rotation;
            }
        }
    }
}