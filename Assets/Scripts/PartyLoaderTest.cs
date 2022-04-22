using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyLoaderTest : MonoBehaviour
{
    public Battler[] party;

    void Start()
    {
        GameManagerGlobal.SaveParty(party);
    }
}
