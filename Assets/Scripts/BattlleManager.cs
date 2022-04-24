using UnityEngine.SceneManagement;

public class BattlleManager
{
    public static void LoadBattleScene(Party playerParty, Party aponentParty)
    {
        BattleLoaderInfo.playerParty = playerParty;
        BattleLoaderInfo.aponentParty = aponentParty;

        SceneManager.LoadScene(0);
    }
}

public class BattleLoaderInfo
{
    public static Party playerParty;
    public static Party aponentParty;
}