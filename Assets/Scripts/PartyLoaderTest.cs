using UnityEngine;

public class PartyLoaderTest : MonoBehaviour
{
    public BattlerTemplate[] playerPartyTemplate;
    public BattlerTemplate[] aponentPartyTemplate;

    public AllMoves allMoves;

    public Party playerParty;
    public Party aponentParty;

    private void Awake()
    {
        LoadBattle();
    }

    public void LoadBattle()
    {
        playerParty.party[0] = BattlerCreator.SetUp(playerPartyTemplate[0], 5, playerPartyTemplate[0].name, playerPartyTemplate[0].baseHealth, allMoves.Ember, allMoves.Tackle, null, null);
        playerParty.party[1] = BattlerCreator.SetUp(playerPartyTemplate[1], 5, playerPartyTemplate[1].name, playerPartyTemplate[1].baseHealth, allMoves.Tackle, null, null, null);

        aponentParty.party[0] = BattlerCreator.SetUp(aponentPartyTemplate[0], 5, aponentPartyTemplate[0].name, aponentPartyTemplate[0].baseHealth, allMoves.Tackle, null, null, null);

        string playerPath = Application.persistentDataPath + "/party.json";
        string aponentPath = Application.persistentDataPath + "/aponentTestParty.json";

        SaveAndLoad<Party>.SaveJson(playerParty, playerPath);
        SaveAndLoad<Party>.SaveJson(aponentParty, aponentPath);

        //While testing, when finished with the batle system I will switch to the more dynamic system
        //BattlleManager.LoadBattleScene(SaveAndLoad<Party>.LoadJson(playerPath), SaveAndLoad<Party>.LoadJson(aponentPath));
    }
}
