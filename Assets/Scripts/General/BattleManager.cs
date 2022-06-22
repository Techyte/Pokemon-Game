using UnityEngine.SceneManagement;
using PokemonGame.Battle;

namespace PokemonGame
{
    public static class BattleManager
    {
        public static void LoadScene(Party playerParty, Party opponentParty, EnemyAI enemyAI, int sceneToLoad)
        {
            LoaderInfo.playerParty = playerParty;
            LoaderInfo.opponentParty = opponentParty;
            LoaderInfo.enemyAI = enemyAI;

            SceneManager.LoadScene(sceneToLoad);
        }

        public static void ClearLoader()
        {
            LoaderInfo.playerParty = null;
            LoaderInfo.opponentParty = null;
            LoaderInfo.enemyAI = null;
        }
    }

    public static class LoaderInfo
    {
        public static Party playerParty;
        public static Party opponentParty;
        public static EnemyAI enemyAI;
    }
}