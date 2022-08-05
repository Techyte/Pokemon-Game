using System.Collections.Generic;
using System.Linq;
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
        public static List<object> vars = new List<object>();
        
        /// <summary>
        /// Loads a scene from the sceneIndex param and what ever arguments you give it
        /// </summary>
        /// <param name="sceneIndex">The scene index to load</param>
        /// <param name="vars">The variables to load the scene with</param>
        public static void LoadScene(int sceneIndex, object[] vars)
        {
            //Debug.Log($"Loading level: {sceneIndex}");
            
            SceneLoader.vars = vars.ToList();
            SceneManager.LoadScene(sceneIndex);
        }

        /// <summary>
        /// Loads a scene from the sceneIndex param and what ever arguments you give it
        /// </summary>
        /// <param name="sceneIndex">The scene index to load</param>
        /// <param name="vars">The variables to load the scene with</param>
        public static void LoadScene(int sceneIndex, List<object> vars)
        {
            //Debug.Log($"Loading level: {sceneIndex}");

            SceneLoader.vars = vars;
            SceneManager.LoadScene(sceneIndex);
        }

        /// <summary>
        /// Clears the scene loader arguments
        /// </summary>
        public static void ClearLoader()
        {
            vars.Clear();
        }
    }
}