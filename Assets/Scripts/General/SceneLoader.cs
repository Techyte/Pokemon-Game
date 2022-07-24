using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

namespace PokemonGame
{
    public static class SceneLoader
    {
        public static void LoadScene(int sceneIndex, object[] vars)
        {
            LoaderInfo.vars = vars.ToList();
            SceneManager.LoadScene(sceneIndex);
        }

        public static void LoadScene(int sceneIndex, List<object> vars)
        {
            LoaderInfo.vars = vars;
            SceneManager.LoadScene(sceneIndex);
        }

        public static void ClearLoader()
        {
            LoaderInfo.vars.Clear();
        }
    }

    public static class LoaderInfo
    {
        public static List<object> vars;
    }
}