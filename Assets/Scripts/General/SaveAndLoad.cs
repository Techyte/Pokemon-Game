using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace PokemonGame
{
    public static class SaveAndLoad<Type>
    {
        public static void SaveJson(Type data, string path)
        {
            File.WriteAllText(path, JsonUtility.ToJson(data, true));
        }

        public static Type LoadJson(string path)
        {
            return JsonUtility.FromJson<Type>(File.ReadAllText(path));
        }
    }

    [System.Serializable]
    public class Party
    {
        public List<Battler> party = new List<Battler>();
    }

}