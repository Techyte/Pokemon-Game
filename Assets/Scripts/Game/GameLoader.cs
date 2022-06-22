using PokemonGame.Game;
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
            player.position = GameWorldData.playerTransform;
            if(GameWorldData.fromBattle)
                GameObject.Find(GameWorldData.battleStarterName).GetComponent<BattleStarter>().isDefeated = GameWorldData.isDefeated;
            GameWorldData.fromBattle = false;
        }
    }

    public static class GameWorldData
    {
        public static Vector3 playerTransform;
        public static bool fromBattle;
        public static string battleStarterName;
        public static bool isDefeated;
    }   
}
