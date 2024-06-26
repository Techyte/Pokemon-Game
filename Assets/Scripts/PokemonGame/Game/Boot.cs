using PokemonGame.Global;

namespace PokemonGame.Game
{
    using UnityEngine;

    public class Boot : MonoBehaviour
    {
        [SerializeField] private GameObject[] DontDestroyObjects;
        
        private void Start()
        {
            Debug.Log("start");
            foreach (var objectToNotDestroy in DontDestroyObjects)
            {
                DontDestroyOnLoad(objectToNotDestroy);
            }
            SceneLoader.LoadScene(1);
        }
    }
}