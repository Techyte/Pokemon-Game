using System;
using UnityEngine;

namespace PokemonGame.Dialogue
{
    /// <summary>
    /// Can be extended by a class to create something that triggers dialogue
    /// </summary>
    public class DialogueTrigger : MonoBehaviour
    {
        [Header("Visual Cue")]
        [SerializeField] private GameObject visualCue;

        [SerializeField] private bool playerInRange;

        [Header("Ink JSON")]
        [SerializeField] private TextAsset inkJson;

        private void Update()
        {
            if (playerInRange && !DialogueManager.GetInstance().dialogueIsPlaying)
            {
                visualCue.SetActive(true);
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    DialogueManager.GetInstance().currentTrigger = this;
                    DialogueManager.GetInstance().EnterDialogueMode(inkJson);
                }
            }
            else
            {
                visualCue.SetActive(false);
            }
        }

        private void Awake()
        {
            playerInRange = false;
            visualCue.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == "Player")
                playerInRange = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Player")
                playerInRange = false;
        }

        public virtual void CallTag(string TagKey, string TagValue)
        {
            
        }
    }
}
