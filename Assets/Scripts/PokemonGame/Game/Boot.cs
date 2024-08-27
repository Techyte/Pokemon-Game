using PokemonGame.Global;

namespace PokemonGame.Game
{
    using UnityEngine;

    public class Boot : MonoBehaviour
    {
        [SerializeField] private GameObject[] DontDestroyObjects;
        
        private void Start()
        {
            Bag.Add(Registry.GetItem("Potion"), 2);
            Bag.Add(Registry.GetItem("Revive"), 2);
            Bag.Add(Registry.GetItem("Max Revive"), 2);
            Bag.GainMoney(10000);
            
            foreach (var objectToNotDestroy in DontDestroyObjects)
            {
                DontDestroyOnLoad(objectToNotDestroy);
            }
            SceneLoader.LoadScene(1);
        }
    }
}