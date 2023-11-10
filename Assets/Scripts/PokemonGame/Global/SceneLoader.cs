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
        /// Loads the scene with the same index as sceneIndex
        /// </summary>
        /// <param name="sceneIndex">The index of the scene to load</param>
        /// <param name="newVars">Variables to pass along to the next scene</param>
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
        /// Loads the scene with the same index as sceneIndex
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
        /// Loads the scene with the same name as sceneName
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
    
        /// <summary>
        /// Loads the scene with the same name as sceneName
        /// </summary>
        /// <param name="sceneName">The name of the scene to load</param>
        /// <param name="newVars">Variables to pass along to the next scene</param>
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

        /// <summary>
        /// Used to handle scene loading while dialogue is running
        /// </summary>
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
    
        /// <summary>
        /// Gets a variable from the carried over variables
        /// </summary>
        /// <param name="variableName">The name of the variable to get</param>
        /// <typeparam name="T">The type of variable to get</typeparam>
        /// <returns></returns>
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