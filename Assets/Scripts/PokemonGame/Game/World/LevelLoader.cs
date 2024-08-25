using System.Collections;

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
        [SerializeField] private TransitionType transitionType;
        
        private void OnTriggerEnter(Collider other)
        {
            Player.Instance.movement.canMove = false;
            StartCoroutine(CloseTransition());
        }

        private void TransitionScene()
        {
            Dictionary<string, object> vars = new Dictionary<string, object>
            {
                { "loaderName", connectingLoaderName },
            };

            SceneLoader.LoadScene(levelName, vars);
        }

        public void SpawnFrom()
        {
            Player.Instance.movement.canMove = false;
            if (useSpawnPointRotation)
            {
                Player.Instance.SetPosRot(spawnPoint.position, spawnPoint.rotation);
            }
            else
            {
                Player.Instance.SetPosRot(spawnPoint.position, Player.Instance.transform.rotation);
            }

            StartCoroutine(OpenTransition());
        }

        private IEnumerator OpenTransition()
        {
            switch (transitionType)
            {
                case TransitionType.Circle:
                    Instantiate(Resources.Load("Pokemon Game/Transitions/CircleFadeOpen"));
                    break;
                case TransitionType.Spiky:
                    Instantiate(Resources.Load("Pokemon Game/Transitions/SpikyOpen"));
                    break;
            }
            
            yield return new WaitForSeconds(0.4f);
            Player.Instance.movement.canMove = true;
        }

        private IEnumerator CloseTransition()
        {
            switch (transitionType)
            {
                case TransitionType.Circle:
                    Instantiate(Resources.Load("Pokemon Game/Transitions/CircleFadeClose"));
                    break;
                case TransitionType.Spiky:
                    Instantiate(Resources.Load("Pokemon Game/Transitions/SpikyClose"));
                    break;
            }
            
            yield return new WaitForSeconds(0.4f);
            TransitionScene();
        }
    }
}