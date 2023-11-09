using PokemonGame.General;

namespace PokemonGame.Dialogue
{
    using System;
    using UnityEngine;
    using TMPro;
    using Ink.Runtime;
    using System.Collections;
    using System.Collections.Generic;
    using Game;
    using UnityEngine.SceneManagement;
    
    /// <summary>
    /// Manages all dialogue in the game, if dialogue is wanted in a scene, must have an instance inside of said scene
    /// </summary>
    public class DialogueManager : MonoBehaviour
    { 
        public static DialogueManager instance;
        
        [SerializeField] private GameObject dialoguePanel;
        [SerializeField] private TextMeshProUGUI dialogueTextDisplay;
        [SerializeField] private GameObject[] choices;
        [SerializeField] private int currentChoicesAmount;
        [SerializeField] private int maxCharAmount;
        private Story _currentStory;
        [SerializeField] private TextAsset globalsInkFile;
        private TextMeshProUGUI[] _choicesText;
        // TODO: get rid of this god awful global variable system, like jesus christ
        private DialogueVariables _dialogueVariables;
        private DialogueMethods _dialogueMethods;

        public bool dialogueIsPlaying { get; private set; }
        public DialogueTrigger currentTrigger;
        [SerializeField] private PlayerMovement movement;
        
        public event EventHandler<DialogueStartedEventArgs> DialogueStarted;
        
        private bool isInBattle => SceneManager.GetActiveScene().name == "Battle";

        private string[] tempNextLines;
        private int currentTempIndex;

        private bool wasToldToNotStart;
        
        private void Awake()
        {
            instance = this;
            
            SceneManager.sceneLoaded += SceneManagerOnSceneLoaded;
    
            _dialogueVariables = new DialogueVariables(globalsInkFile);
            _dialogueMethods = new DialogueMethods();
        }
        
        private void SceneManagerOnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            string name = SceneManager.GetActiveScene().name;
            if (name != "Battle" || name == "Boot")
            {
                movement = FindObjectOfType<PlayerMovement>();
            }
        }
        
        private void Update()
        {
            if (!dialogueIsPlaying)
                return;
            currentChoicesAmount = _currentStory.currentChoices.Count;
            var hasChoices = currentChoicesAmount > 0;

            if (tempNextLines != null)
            {
                if (currentTempIndex != tempNextLines.Length-1)
                {
                    hasChoices = false;
                }
            }
            
            if (Input.GetKeyDown(KeyCode.C) && !hasChoices)
            {
                ContinueStory();
            }
        }
        
        private void Start()
        {
            dialogueIsPlaying = false;
            dialoguePanel.SetActive(false);
            _choicesText = new TextMeshProUGUI[choices.Length];
            
            int index = 0;
            foreach (GameObject choice in choices)
            {
                _choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
                index++;
            }
    
            for (int i = 0; i < choices.Length; i++)
            {
                _choicesText[i] = choices[i].GetComponentInChildren<TextMeshProUGUI>();
            }
        }
        
        /// <summary>
        /// Starts a new conversation
        /// </summary>
        /// <param name="inkJson">The TextAsset with the information about the conversation</param>
        /// <param name="trigger">The DialogueTrigger that triggered this conversation</param>
        /// <param name="autostart">Automatically start the dialogue on load, on by default</param>
        public void LoadDialogueMode(TextAsset inkJson, DialogueTrigger trigger, bool autostart = true)
        {
            DialogueStarted?.Invoke(this, new DialogueStartedEventArgs(trigger, inkJson));
            
            currentTrigger = trigger;
            if(!isInBattle)
                movement.canMove = false;
            _currentStory = new Story(inkJson.text);
            _dialogueVariables.StartListening(_currentStory);
            if (autostart)
            {
                dialogueIsPlaying = true;
                dialoguePanel.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                ContinueStory();
            }
            else
                wasToldToNotStart = true;
        }

        /// <summary>
        /// Start a conversation, will only work if you loaded a conversation but did not start it. If not it will do nothing and give you a warning
        /// </summary>
        public void StartDialogue()
        {
            if (wasToldToNotStart)
            {
                wasToldToNotStart = false;
                dialogueIsPlaying = true;
                dialoguePanel.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                ContinueStory();
            }
            else
            {
                Debug.LogWarning("There is nothing to start!");
            }
        }

        public void SetDialogueVariables(Dictionary<string, string> variables)
        {
            foreach (var variable in variables)
            {
                _currentStory.variablesState[variable.Key] = variable.Value;
            }
        }
        
        private IEnumerator ExitDialogueMode()
        {
            yield return new WaitForSeconds(0.2f);
            _dialogueVariables.StopListening(_currentStory);
            if(!isInBattle)
            {
                movement.canMove = true;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            dialogueIsPlaying = false;
            dialoguePanel.SetActive(false);
            dialogueTextDisplay.text = "";
            currentTrigger.EndDialogue();
        }
        
        private void ContinueStory()
        {
            StopAllCoroutines();

            if (tempNextLines == null)
            {
                if (_currentStory.canContinue)
                {
                    string next = _currentStory.Continue();

                    if (next.Length >= maxCharAmount)
                    {
                        string[] newNextLines = next.SplitIntoParts(maxCharAmount);
                        
                        Debug.Log(newNextLines[0]);

                        tempNextLines = newNextLines;
                        StartCoroutine(DisplayText(newNextLines[currentTempIndex]));
                        HandleTags(_currentStory.currentTags);
                    }
                    else
                    {
                        StartCoroutine(DisplayText(next));
                        StartCoroutine(DisplayChoices());
                        HandleTags(_currentStory.currentTags);
                    }   
                }
                else
                {
                    StartCoroutine(ExitDialogueMode());
                }
            }
            else
            {
                if (currentTempIndex >= tempNextLines.Length - 1)
                {
                    // exhausted our leftovers
                    tempNextLines = null;
                    currentTempIndex = 0;
                    
                    if(_currentStory.canContinue)
                    {
                        StartCoroutine(DisplayText(_currentStory.Continue()));
                        StartCoroutine(DisplayChoices());
                        HandleTags(_currentStory.currentTags);
                    }
                    else
                    {
                        StartCoroutine(ExitDialogueMode());
                    }
                }
                else
                {
                    currentTempIndex++;
                    StartCoroutine(DisplayText(tempNextLines[currentTempIndex]));
                    if(currentTempIndex == tempNextLines.Length-1)
                    {
                        StartCoroutine(DisplayChoices());
                    }
                }
            }
        }
        
        private IEnumerator DisplayText(string nextSentence)
        {
            dialogueTextDisplay.text = "";
            foreach (char letter in nextSentence)
            {
                dialogueTextDisplay.text += letter;
                yield return null;
            }
        }
        
        private void HandleTags(List<string> currentTags)
        {
            foreach(string tag in currentTags)
            {
                string[] splitTag = tag.Split(':');
    
                string tagKey = "";
                string tagValue = "";
                string[] tagValues = null;

                if(splitTag.Length > 2)
                {
                    Debug.LogError("Tag could not be appropriately parsed: " + tag);
                }
                else if(splitTag.Length == 2)
                {
                    tagKey = splitTag[0].Trim();
                    tagValue = splitTag[1].Trim();

                    tagValues = tagValue.Split('.');
                }
                else
                {
                    // only the key is provided
                    tagKey = tag;
                }

                if (currentTrigger.allowsGlobalTags)
                {
                    _dialogueMethods.HandleGlobalTag(tagKey, tagValues);
                }
                
                currentTrigger.CallTag(tagKey, tagValues);
            }
        }
        
        private IEnumerator DisplayChoices()
        {
            List<Choice> currentChoices = _currentStory.currentChoices;
            
            if (currentChoices.Count > choices.Length)
            {
                Debug.LogError("More choices were given than the UI can support either add more choice buttons are take away choices. Number of choices given: "
                    + currentChoices.Count);
            }
            
            int index = 0;
            
            foreach(Choice choice in currentChoices)
            {
                choices[index].gameObject.SetActive(true);
                _choicesText[index].text = choice.text;
                index++;
            }
            
            for(int i = index; i < choices.Length; i++)
            {
                choices[i].gameObject.SetActive(false);
                yield return null;
            }
        }
        
        /// <summary>
        /// Makes the choice that the player wants using the index of the choice
        /// </summary>
        /// <param name="choiceIndex">The choicer index of the player wants to make</param>
        public void MakeChoice(int choiceIndex)
        {
            _currentStory.ChooseChoiceIndex(choiceIndex);
            ContinueStory();
        }
    }

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
}