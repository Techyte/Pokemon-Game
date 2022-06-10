using UnityEngine;

namespace PokemonGame.Battle
{
    public class GameLoader : MonoBehaviour
    {
        [SerializeField] private Transform player;
    
        private void Start()
        {
            LoadGame();
        }

        private void LoadGame()
        {
            player = GameWorldData.playerTransform;
        }
    }

    public class GameWorldData
    {
        public static Transform playerTransform;
    }   
}
