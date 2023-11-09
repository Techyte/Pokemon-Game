using PokemonGame.Dialogue;
using Unity.VisualScripting;

namespace PokemonGame.Global
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    
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

        private static bool listening = false;
        private static int sceneIndex;
        private static string sceneName;

        /// <summary>
        /// Loads a scene from the sceneIndex param and what ever arguments you give it
        /// </summary>
        /// <param name="sceneIndex">The scene index to load</param>
        /// <param name="newVars">The variables to load the scene with</param>
        public static void LoadScene(int sceneIndex, Dictionary<string, object> newVars)
        {
            ClearLoader();
            _vars = newVars;
            if (DialogueManager.instance.dialogueIsPlaying && !listening)
            {
                Debug.Log("listening now");
                DialogueManager.instance.DialogueEnded += IsDialogueDone;
                listening = true;
                SceneLoader.sceneIndex = sceneIndex;
            }
            else
            {
                sceneLoadedFrom = SceneManager.GetActiveScene().name;
                SceneManager.LoadScene(sceneIndex);   
            }
        }

        /// <summary>
        /// Loads a a scene from the sceneIndex param
        /// </summary>
        /// <param name="sceneIndex">The index of the scene to load</param>
        public static void LoadScene(int sceneIndex)
        {
            ClearLoader();
            if (DialogueManager.instance.dialogueIsPlaying && !listening)
            {
                Debug.Log("listening now");
                DialogueManager.instance.DialogueEnded += IsDialogueDone;
                listening = true;
                SceneLoader.sceneIndex = sceneIndex;
            }
            else
            {
                sceneLoadedFrom = SceneManager.GetActiveScene().name;
                SceneManager.LoadScene(sceneIndex);   
            }
        }

        /// <summary>
        /// Loads a a scene from the name param
        /// </summary>
        /// <param name="sceneName">The name of the scene to load</param>
        public static void LoadScene(string sceneName)
        {
            ClearLoader();
            if (DialogueManager.instance.dialogueIsPlaying && !listening)
            {
                Debug.Log("listening now");
                DialogueManager.instance.DialogueEnded += IsDialogueDone;
                listening = true;
                SceneLoader.sceneName = sceneName;
            }
            else
            {
                sceneLoadedFrom = SceneManager.GetActiveScene().name;
                SceneManager.LoadScene(sceneName);   
            }
        }
    
        public static void LoadScene(string sceneName, Dictionary<string, object> newVars)
        {
            ClearLoader();
            _vars = newVars;
            Debug.Log("loading scene from a name");
            if (DialogueManager.instance.dialogueIsPlaying && !listening)
            {
                Debug.Log("listening now");
                DialogueManager.instance.DialogueEnded += IsDialogueDone;
                listening = true;
                SceneLoader.sceneName = sceneName;
            }
            else
            {
                sceneLoadedFrom = SceneManager.GetActiveScene().name;
                SceneManager.LoadScene(sceneName);   
            }
        }

        private static void IsDialogueDone(object sender, DialogueEndedEventArgs args)
        {
            Debug.Log(args.moreToGo);
            if (!args.moreToGo)
            {
                sceneLoadedFrom = SceneManager.GetActiveScene().name;
                if (string.IsNullOrEmpty(sceneName))
                {
                    SceneManager.LoadScene(sceneIndex);   
                }
                else
                {
                    SceneManager.LoadScene(sceneName);
                }

                sceneIndex = 0;
                sceneName = null;

                DialogueManager.instance.DialogueEnded -= IsDialogueDone;
                listening = false;
            }
        }
    
        /// <summary>
        /// Clears the scene loader arguments
        /// </summary>
        private static void ClearLoader()
        {
            if(_vars != null)
            {
                _vars.Clear();
            }
            sceneLoadedFrom = null;
        }
    
        public static T GetVariable<T>(string variableName)
        {
            if (_vars.TryGetValue(variableName, out object var))
            {
                return (T)var;
            }
            
            return default;
        }
    }
}