using System;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using PokemonGame.Game;
using UnityEngine.SceneManagement;

namespace PokemonGame.Dialogue
{
    public class DialogueStartedEventArgs : EventArgs
    {
        public DialogueTrigger trigger;
        public TextAsset textAsset;

        public DialogueStartedEventArgs(DialogueTrigger trigger, TextAsset textAsset)
        {
            this.trigger = trigger;
            this.textAsset = textAsset;
        }
    }
    
    /// <summary>
    /// Manages all dialogue in the game, if dialogue is wanted in a scene, must have an instance inside of said scene
    /// </summary>
    public class DialogueManager : MonoBehaviour
    {
        public static DialogueManager instance;
        
        [SerializeField] private GameObject dialoguePanel;
        [SerializeField] private TextMeshProUGUI dialogueTextDisplay;
        [SerializeField] private GameObject[] choices;
        [SerializeField] private int currentChoices;
        private Story currentStory;
        private TextMeshProUGUI[] choicesText;
        public bool dialogueIsPlaying { get; private set; }

        public DialogueTrigger currentTrigger;

        [SerializeField] private PlayerMovement _movement;

        public event EventHandler<DialogueStartedEventArgs> DialogueStarted;

        private bool isInBattle => SceneManager.GetActiveScene().name == "Battle";
        
        private void Awake()
        {
            instance = this;
            
            SceneManager.sceneLoaded += SceneManagerOnSceneLoaded;
        }

        private void SceneManagerOnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            string name = SceneManager.GetActiveScene().name;
            if (name != "Battle" || name == "Boot")
            {
                _movement = FindObjectOfType<PlayerMovement>();
            }
        }

        private void Update()
        {
            if (!dialogueIsPlaying)
                return;

            currentChoices = currentStory.currentChoices.Count;

            bool hasChoices;

            if (currentChoices > 0)
                hasChoices = true;
            else
                hasChoices = false;

            if (Input.GetKeyDown(KeyCode.Space) && !hasChoices)
            {
                ContinueStory();
            }
        }

        private void Start()
        {
            dialogueIsPlaying = false;
            dialoguePanel.SetActive(false);
            choicesText = new TextMeshProUGUI[choices.Length];
            int index = 0;
            foreach (GameObject choice in choices)
            {
                choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
                index++;
            }
        }

        /// <summary>
        /// Starts a new conversation
        /// </summary>
        /// <param name="inkJson">The TextAsset with the information about the conversation</param>
        public void EnterDialogueMode(TextAsset inkJson, DialogueTrigger trigger)
        {
            DialogueStarted?.Invoke(this, new DialogueStartedEventArgs(trigger, inkJson));
            
            currentTrigger = trigger;
            if(!isInBattle)
                _movement.canMove = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            currentStory = new Story(inkJson.text);
            dialogueIsPlaying = true;
            dialoguePanel.SetActive(true);

            ContinueStory();
        }

        private IEnumerator ExitDialogueMode()
        {
            yield return new WaitForSeconds(0.2f);

            if(!isInBattle)
                _movement.canMove = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            dialogueIsPlaying = false;
            dialoguePanel.SetActive(false);
            dialogueTextDisplay.text = "";
            currentTrigger.EndDialogue();
        }

        private void ContinueStory()
        {
            if (currentStory.canContinue)
            {
                dialogueTextDisplay.text = currentStory.Continue();
                DisplayChoices();

                HandleTags(currentStory.currentTags);
            }
            else
            {
                StartCoroutine(ExitDialogueMode());
            }
        }

        private void HandleTags(List<string> currentTags)
        {
            foreach(string tag in currentTags)
            {
                string[] splitTag = tag.Split(':');
                if(splitTag.Length != 2)
                {
                    Debug.LogError("Tag could not be appropriately parsed: " + tag);
                }
                string tagKey = splitTag[0].Trim();
                string tagValue = splitTag[1].Trim();
                
                currentTrigger.CallTag(tagKey, tagValue);
            }
        }

        private void DisplayChoices()
        {
            List<Choice> currentChoices = currentStory.currentChoices;

            if (currentChoices.Count > choices.Length)
            {
                Debug.LogError("More choices were given than the UI can support either add more choice buttons are take away choices. Number of choices given: "
                    + currentChoices.Count);
            }

            int index = 0;
            foreach(Choice choice in currentChoices)
            {
                choices[index].gameObject.SetActive(true);
                choicesText[index].text = choice.text;
                index++;
            }

            for(int i = index; i < choices.Length; i++)
            {
                choices[i].gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Makes the choice that the player wants using the index of the choice
        /// </summary>
        /// <param name="choiceIndex">The choicer index of the player wants to make</param>
        public void MakeChoice(int choiceIndex)
        {
            currentStory.ChooseChoiceIndex(choiceIndex);
            ContinueStory();
        }
    }
}
