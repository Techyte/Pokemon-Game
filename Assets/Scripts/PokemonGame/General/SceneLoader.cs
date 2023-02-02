using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PokemonGame
{
    /// <summary>
    /// A custom scene loading class for loading scenes with data
    /// </summary>
    public static class SceneLoader
    {
        /// <summary>
        /// The list of variables that you loaded the scene with
        /// </summary>
        public static Dictionary<string, object> vars = new Dictionary<string, object>();

        public static string sceneLoadedFrom;
        
        /// <summary>
        /// Loads a scene from the sceneIndex param and what ever arguments you give it
        /// </summary>
        /// <param name="sceneIndex">The scene index to load</param>
        /// <param name="vars">The variables to load the scene with</param>
        public static void LoadScene(int sceneIndex, Dictionary<string, object> vars)
        {
            ClearLoader();
            sceneLoadedFrom = SceneManager.GetActiveScene().name;
            SceneLoader.vars = vars;
            SceneManager.LoadScene(sceneIndex);
        }

        public static void LoadScene(string sceneName, Dictionary<string, object> vars)
        {
            ClearLoader();
            sceneLoadedFrom = SceneManager.GetActiveScene().name;
            SceneLoader.vars = vars;
            SceneManager.LoadScene(sceneName);
        }

        /// <summary>
        /// Clears the scene loader arguments
        /// </summary>
        public static void ClearLoader()
        {
            vars.Clear();
            sceneLoadedFrom = null;
        }

        public static object GetVariable(string variableName)
        {
            if (vars.TryGetValue(variableName, out object var))
            {
                return var;
            }
            
            Debug.LogWarning("Could not find a variable with that name, returning null");
            return null;
        }
    }
}