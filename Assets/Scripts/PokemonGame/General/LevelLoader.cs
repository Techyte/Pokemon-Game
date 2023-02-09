namespace PokemonGame.General
{
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using Global;

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