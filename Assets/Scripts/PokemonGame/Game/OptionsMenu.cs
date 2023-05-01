using System;
using PokemonGame.Dialogue;
using UnityEngine.SceneManagement;

namespace PokemonGame.Game
{
    using UnityEngine;

    public class OptionsMenu : MonoBehaviour
    {
        [SerializeField] private KeyCode bagKey;
        [Space]
        [SerializeField] private PlayerMovement movement;
        
        [SerializeField] private bool state;
        [SerializeField] private GameObject menuObject;
        [SerializeField] private GameObject defaultMenuObject;

        [SerializeField] private MenuBase[] menuObjects;

        private void Awake()
        {
            SceneManager.sceneLoaded += SceneManagerOnSceneLoaded;
        }

        public void OpenSeparateMenu(int menuIndex)
        {
            defaultMenuObject.SetActive(false);
            menuObjects[menuIndex].Open();
        }

        public void BackButton()
        {
            defaultMenuObject.SetActive(true);
            
            for (int i = 0; i < menuObjects.Length; i++)
            {
                menuObjects[i].Close();
            }
        }

        private void SceneManagerOnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            if (SceneManager.GetActiveScene().name != "Battle")
            {
                movement = FindObjectOfType<PlayerMovement>();
            }
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(bagKey) && !DialogueManager.instance.dialogueIsPlaying)
            {
                ToggleMenu();
            }
        }
        
        private void ToggleMenu()
        {
            state = !state;
            
            movement.canMove = !state;
            menuObject.SetActive(state);
            Cursor.visible = state;
            Cursor.lockState = state ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }   
}
