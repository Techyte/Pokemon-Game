namespace PokemonGame.General
{
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using Global;

    public class LevelLoader : MonoBehaviour
    {
        [SerializeField] private SceneLoaderSetting loaderSetting;

        public bool levelSettingTrue => loaderSetting == SceneLoaderSetting.Name;

        [SerializeField] private bool test;
        
        [ConditionalHide("levelSettingTrue", true)]
        public string levelName;

        private void OnTriggerEnter(Collider other)
        {
            SceneLoader.ClearLoader();
            SceneManager.LoadScene(levelName);
        }
    }

    public enum SceneLoaderSetting
    {
        Name,
        Id
    }
}