using UnityEngine.SceneManagement;

public class BattlleManager
{
    public static void LoadBattleScene(Party playerParty, Party aponentParty)
    {
        BattleLoaderInfo.playerParty = playerParty;
        BattleLoaderInfo.apponentParty = aponentParty;

        SceneManager.LoadScene(0);
    }

    public static void ClearBattleLoader()
    {
        BattleLoaderInfo.playerParty = null;
        BattleLoaderInfo.apponentParty = null;
    }
}

public class BattleLoaderInfo
{
    public static Party playerParty;
    public static Party apponentParty;
}