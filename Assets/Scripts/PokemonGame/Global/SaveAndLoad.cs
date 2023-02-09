namespace PokemonGame.Global
{
    using System.IO;
    using UnityEngine;

    /// <summary>
    /// A class for saving and loading data with json
    /// </summary>
    /// <typeparam name="Type">The type to save/load</typeparam>
    public static class SaveAndLoad<Type>
    {
        /// <summary>
        /// Save an object to a json file
        /// </summary>
        /// <param name="data">The object to save</param>
        /// <param name="path">The path to save the file to</param>
        public static void SaveJson(Type data, string path)
        {
            File.WriteAllText(path, JsonUtility.ToJson(data, true));
        }

        /// <summary>
        /// Load a json file from a path into an object
        /// </summary>
        /// <param name="path">The path to load the json from</param>
        /// <returns></returns>
        public static Type LoadJson(string path)
        {
            return JsonUtility.FromJson<Type>(File.ReadAllText(path));
        }
    }
}