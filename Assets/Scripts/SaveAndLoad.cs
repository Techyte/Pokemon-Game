using System.IO;
using UnityEngine;

public class SaveAndLoad<Type>
{
    public static void SaveJson(Type data, string path)
    {
        string json = JsonUtility.ToJson(data, true);

        File.WriteAllText(path, json);
    }

    public static Type LoadJson(string path)
    {
        Type returnObject;

        string json = File.ReadAllText(path);

        returnObject = JsonUtility.FromJson<Type>(json);

        return returnObject;
    }
}

[System.Serializable]
public class Party
{
    public Battler[] party = new Battler[6];
}
