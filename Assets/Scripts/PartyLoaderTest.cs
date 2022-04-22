using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyLoaderTest : MonoBehaviour
{
    public BattlerTemplate[] partyTemplate;

    public AllMoves allMoves;

    public Party party;

    void Awake()
    {
        for(int i = 0; i < partyTemplate.Length; i++)
        {
            party.party[i] = BattlerCreator.SetUp(partyTemplate[i], 5, partyTemplate[i].name, partyTemplate[i].baseHealth, allMoves.Ember, allMoves.Tackle, null, null);
        }

        SaveAndLoad.SaveParty(party);
    }
}
