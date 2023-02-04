using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PokemonGame.General
{
    /// <summary>
    /// A custom scene loading class for loading scenes with data
    /// </summary>
    public static class SceneLoader
    {
        /// <summary>
        /// The list of variables that you loaded the scene with
        /// </summary>
        private static Dictionary<string, object> _vars = new Dictionary<string, object>();

        public static string sceneLoadedFrom;
        
        /// <summary>
        /// Loads a scene from the sceneIndex param and what ever arguments you give it
        /// </summary>
        /// <param name="sceneIndex">The scene index to load</param>
        /// <param name="newVars">The variables to load the scene with</param>
        public static void LoadScene(int sceneIndex, Dictionary<string, object> newVars)
        {
            ClearLoader();
            sceneLoadedFrom = SceneManager.GetActiveScene().name;
            _vars = newVars;
            SceneManager.LoadScene(sceneIndex);
        }

        public static void LoadScene(string sceneName, Dictionary<string, object> newVars)
        {
            ClearLoader();
            sceneLoadedFrom = SceneManager.GetActiveScene().name;
            _vars = newVars;
            SceneManager.LoadScene(sceneName);
        }

        /// <summary>
        /// Clears the scene loader arguments
        /// </summary>
        public static void ClearLoader()
        {
            _vars.Clear();
            sceneLoadedFrom = null;
        }

        public static object GetVariable(string variableName)
        {
            if (_vars.TryGetValue(variableName, out object var))
            {
                return var;
            }
            
            Debug.LogWarning("Could not find a variable with that name, returning null");
            return null;
        }
    }
}