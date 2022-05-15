using UnityEngine;
using UnityEngine.SceneManagement;

namespace PokemonGame
{
    public class LevelLoader : MonoBehaviour
    {
        [SerializeField] private string LevelName;

        private void OnTriggerEnter(Collider other)
        {
            SceneManager.LoadScene(LevelName);
        }
    }
}