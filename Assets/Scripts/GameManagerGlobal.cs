using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GameManagerGlobal
{
    public static void SaveParty(Battler[] party)
    {
        try
        {
            string path = Application.persistentDataPath + "/party.pt";

            if (File.Exists(path)) File.Delete(path);

            FileStream file = File.Create(path);

            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(file, party);
            file.Close();
        }
        catch
        {
            Debug.Log("Could not save party");
        }
    }

    public static Battler[] LoadParty()
    {
        Battler[] returnParty = new Battler[1];
        string path = Application.persistentDataPath + "/party.pt";

        if (File.Exists(path))
        {
            FileStream file = File.Open(path, FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            returnParty = (Battler[]) bf.Deserialize(file);
        }

        return returnParty;
    }
}
