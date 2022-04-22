using UnityEngine.SceneManagement;

public class BattlleManager
{
    public static void LoadBattleScene(Battler[] playerParty, Battler[] aponentParty)
    {
        BattleLoaderInfo.playerParty = playerParty;
        BattleLoaderInfo.aponentParty = aponentParty;

        SceneManager.GetSceneByName("Battle");
    }
}

public class BattleLoaderInfo
{
    public static Battler[] playerParty;
    public static Battler[] aponentParty;
}