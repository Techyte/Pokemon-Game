using UnityEngine;
using TMPro;
using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace PokemonGame.Dialogue
{
    public class DialogueManager : MonoBehaviour
    {
        [SerializeField] private GameObject dialoguePanel;
        [SerializeField] private TextMeshProUGUI dialogueTextDisplay;
        [SerializeField] private GameObject[] choices;
        [SerializeField] private int currentChoices;
        private Story currentStory;
        private TextMeshProUGUI[] choicesText;
        public bool dialogueIsPlaying { get; private set; }

        private static DialogueManager instance;

        private void Awake()
        {
            if(instance != null)
            {
                Debug.LogWarning("Found more than one Dialogue Manager in the scene!");
            }
            instance = this;
        }

        private void Update()
        {
            if (!dialogueIsPlaying)
                return;

            currentChoices = currentStory.currentChoices.Count;

            bool hasChoices;

            if (currentChoices > 0)
            {
                hasChoices = true;
            }
            else
            {
                hasChoices = false;
            }

            if (Input.GetKeyDown(KeyCode.Space) && !hasChoices)
            {
                ContinueStory();
            }
        }

        public static DialogueManager GetInstance()
        {
            return instance;
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

        public void EnterDialogueMode(TextAsset inkJson)
        {
            currentStory = new Story(inkJson.text);
            dialogueIsPlaying = true;
            dialoguePanel.SetActive(true);

            ContinueStory();
        }

        private IEnumerator ExitDialogueMode()
        {
            yield return new WaitForSeconds(0.2f);

            dialogueIsPlaying = false;
            dialoguePanel.SetActive(false);
            dialogueTextDisplay.text = "";
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

                switch (tagKey)
                {
                    case "chosenPokemon":
                        Debug.Log("speaker = " + tagValue);
                        break;
                    default:
                        Debug.LogWarning("Tag cam in but is not currently being handled");
                        break;
                }
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

        public void MakeChoice(int choiceIndex)
        {
            currentStory.ChooseChoiceIndex(choiceIndex);
            ContinueStory();
        }
    }
}
