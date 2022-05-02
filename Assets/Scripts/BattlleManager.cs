using UnityEngine.SceneManagement;

public class BattlleManager
{
    public static void LoadBattleScene(Party playerParty, Party aponentParty, float enemyAI)
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
        BattleLoaderInfo.enemyAI = 0;
    }
}

public class BattleLoaderInfo
{
    public static Party playerParty;
    public static Party apponentParty;
    public static float enemyAI;
}