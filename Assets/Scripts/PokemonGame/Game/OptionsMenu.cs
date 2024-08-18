using System;
using PokemonGame.Dialogue;
using UnityEngine.SceneManagement;

namespace PokemonGame.Game
{
    using UnityEngine;

    public class OptionsMenu : MonoBehaviour
    {
        public static OptionsMenu instance;
        
        [SerializeField] private KeyCode bagKey;
        [Space]
        [SerializeField] private PlayerMovement movement;

        public bool on => state;
        
        [SerializeField] private bool state;
        [SerializeField] private GameObject menuObject;
        [SerializeField] private GameObject defaultMenuObject;
        [SerializeField] private GameObject backButton;

        [SerializeField] private GameObject[] menuObjects;
        [SerializeField] private GameObject currentMenu;

        private void Awake()
        {
            SceneManager.sceneLoaded += SceneManagerOnSceneLoaded;
            instance = this;
        }

        public void OpenSeparateMenu(int menuIndex)
        {
            defaultMenuObject.SetActive(false);
            currentMenu = Instantiate(menuObjects[menuIndex], transform);
            backButton.SetActive(true);
        }

        public void BackButton()
        {
            defaultMenuObject.SetActive(true);
            
            Destroy(currentMenu);
            currentMenu = null;
            backButton.SetActive(false);
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

            if (!state)
            {
                defaultMenuObject.SetActive(true);
                backButton.SetActive(false);
            
                Destroy(currentMenu);
                currentMenu = null;
            }
            
            movement.canMove = !state;
            menuObject.SetActive(state);
            Cursor.visible = state;
            Cursor.lockState = state ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }   
}
