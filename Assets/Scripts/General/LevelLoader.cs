using UnityEngine;
using UnityEngine.SceneManagement;

namespace PokemonGame
{
    public class LevelLoader : MonoBehaviour
    {
        [SerializeField] private string levelName;

        private void OnTriggerEnter(Collider other)
        {
            SceneManager.LoadScene(levelName);
        }
    }
}