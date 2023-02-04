using UnityEngine;
using UnityEngine.SceneManagement;

namespace PokemonGame.General
{
    public class LevelLoader : MonoBehaviour
    {
        [SerializeField] private string levelName;

        private void OnTriggerEnter(Collider other)
        {
            SceneLoader.ClearLoader();
            SceneManager.LoadScene(levelName);
        }
    }
}