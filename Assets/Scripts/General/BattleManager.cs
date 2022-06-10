using UnityEngine.SceneManagement;
using PokemonGame.Battle;

namespace PokemonGame
{
    public class BattleManager
    {
        public static void LoadScene(Party playerParty, Party aponentParty, EnemyAI enemyAI, int sceneToLoad)
        {
            LoaderInfo.playerParty = playerParty;
            LoaderInfo.apponentParty = aponentParty;
            LoaderInfo.enemyAI = enemyAI;

            SceneManager.LoadScene(sceneToLoad);
        }

        public static void ClearLoader()
        {
            LoaderInfo.playerParty = null;
            LoaderInfo.apponentParty = null;
            LoaderInfo.enemyAI = null;
        }
    }

    public class LoaderInfo
    {
        public static Party playerParty;
        public static Party apponentParty;
        public static EnemyAI enemyAI;
    }
}