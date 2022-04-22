using System.IO;
using UnityEngine;

public class SaveAndLoad
{
    static string json;
    public static void SaveParty(Party party)
    {
        string path = Application.persistentDataPath + "/party.json";

        json = JsonUtility.ToJson(party, true);

        File.WriteAllText(path, json);
    }

    public static Party LoadParty()
    {
        string path = Application.persistentDataPath + "/party.json";
        Party returnParty = new Party();

        returnParty = JsonUtility.FromJson<Party>(json);

        return returnParty;
    }
}

[System.Serializable]
public class Party
{
    public Battler[] party;
}
