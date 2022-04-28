using UnityEngine;

public class PartyLoaderTest : MonoBehaviour
{
    public BattlerTemplate[] playerPartyTemplate;
    public BattlerTemplate[] apponentPartyTemplate;

    public AllMoves allMoves;

    public Party playerParty;
    public Party apponentParty;

    private void Awake()
    {
        LoadBattle();
    }

    public void LoadBattle()
    {
        playerParty.party[0] = BattlerCreator.SetUp(playerPartyTemplate[0], 5, playerPartyTemplate[0].name, allMoves.Ember, allMoves.Tackle, allMoves.Toxic, null);
        playerParty.party[1] = BattlerCreator.SetUp(playerPartyTemplate[1], 5, playerPartyTemplate[1].name, allMoves.Tackle, allMoves.Toxic, null, null);

        apponentParty.party[0] = BattlerCreator.SetUp(apponentPartyTemplate[0], 5, apponentPartyTemplate[0].name, apponentPartyTemplate[0].baseHealth, allMoves.Tackle, allMoves.RazorLeaf, allMoves.Toxic, null);

        string playerPath = Application.persistentDataPath + "/party.json";
        string aponentPath = Application.persistentDataPath + "/apponentTestParty.json";

        SaveAndLoad<Party>.SaveJson(playerParty, playerPath);
        SaveAndLoad<Party>.SaveJson(apponentParty, aponentPath);

        //While testing, when finished with the batle system I will switch to the more dynamic system
        //BattlleManager.LoadBattleScene(SaveAndLoad<Party>.LoadJson(playerPath), SaveAndLoad<Party>.LoadJson(aponentPath));
    }
}
