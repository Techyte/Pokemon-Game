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
        Debug.Log(allMoves.moves.Keys.Count);
        LoadBattle();
    }

    public void LoadBattle()
    {
        playerParty.party[0] = BattlerCreator.SetUp(
            playerPartyTemplate[0],
            5, playerPartyTemplate[0].name,
            allMoves.moves["Ember"],
            allMoves.moves["Tackle"],
            allMoves.moves["Toxic"],
            null);

        playerParty.party[1] = BattlerCreator.SetUp(
            playerPartyTemplate[1],
            5, playerPartyTemplate[1].name,
            allMoves.moves["Tackle"],
            allMoves.moves["Toxic"],
            null,
            null);

        apponentParty.party[0] = BattlerCreator.SetUp(
            apponentPartyTemplate[0],
            5, apponentPartyTemplate[0].name,
            apponentPartyTemplate[0].baseHealth,
            allMoves.moves["Tackle"],
            allMoves.moves["Razor Leaf"],
            allMoves.moves["Toxic"],
            null);

        string playerPath = Application.persistentDataPath + "/party.json";
        string aponentPath = Application.persistentDataPath + "/apponentTestParty.json";

        SaveAndLoad<Party>.SaveJson(playerParty, playerPath);
        SaveAndLoad<Party>.SaveJson(apponentParty, aponentPath);

        //While testing, when finished with the batle system I will switch to the more dynamic system
        //BattlleManager.LoadBattleScene(SaveAndLoad<Party>.LoadJson(playerPath), SaveAndLoad<Party>.LoadJson(aponentPath));
    }
}
