using UnityEngine;

public class PartyLoaderTest : MonoBehaviour
{
    public BattlerTemplate[] playerPartyTemplate;
    public BattlerTemplate[] aponentPartyTemplate;

    public AllMoves allMoves;

    public Party playerParty;
    public Party aponentParty;

    void Awake()
    {
        for(int i = 0; i < playerPartyTemplate.Length; i++)
        {
            playerParty.party[i] = BattlerCreator.SetUp(playerPartyTemplate[i], 5, playerPartyTemplate[i].name, playerPartyTemplate[i].baseHealth, allMoves.Ember, allMoves.Tackle, null, null);
        }

        for (int i = 0; i < aponentPartyTemplate.Length; i++)
        {
            aponentParty.party[i] = BattlerCreator.SetUp(aponentPartyTemplate[i], 5, aponentPartyTemplate[i].name, aponentPartyTemplate[i].baseHealth, allMoves.Tackle, null, null, null);
        }

        string playerPath = Application.persistentDataPath + "/party.json";
        string aponentPath = Application.persistentDataPath + "/aponentTestParty.json";

        SaveAndLoad<Party>.SaveJson(playerParty, playerPath);
        SaveAndLoad<Party>.SaveJson(aponentParty, aponentPath);
    }
}
