using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

namespace PokemonGame
{
    public static class SceneLoader
    {
        public static List<object> vars = new List<object>();
        public static void LoadScene(int sceneIndex, object[] vars)
        {
            //Debug.Log($"Loading level: {sceneIndex}");
            
            SceneLoader.vars = vars.ToList();
            SceneManager.LoadScene(sceneIndex);
        }

        public static void LoadScene(int sceneIndex, List<object> vars)
        {
            //Debug.Log($"Loading level: {sceneIndex}");

            SceneLoader.vars = vars;
            SceneManager.LoadScene(sceneIndex);
        }

        public static void ClearLoader()
        {
            vars.Clear();
        }
    }
}