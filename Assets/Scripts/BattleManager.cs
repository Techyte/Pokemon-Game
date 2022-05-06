using UnityEngine.SceneManagement;
using PokemonGame.Battle;

namespace PokemonGame
{
    public class BattleManager
    {
        public static void LoadBattleScene(Party playerParty, Party aponentParty, EnemyAI enemyAI)
        {
            BattleLoaderInfo.playerParty = playerParty;
            BattleLoaderInfo.apponentParty = aponentParty;
            BattleLoaderInfo.enemyAI = enemyAI;

            SceneManager.LoadScene(0);
        }

        public static void ClearBattleLoader()
        {
            BattleLoaderInfo.playerParty = null;
            BattleLoaderInfo.apponentParty = null;
            BattleLoaderInfo.enemyAI = null;
        }
    }

    public class BattleLoaderInfo
    {
        public static Party playerParty;
        public static Party apponentParty;
        public static EnemyAI enemyAI;
    }
}