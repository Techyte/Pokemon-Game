using System;
using UnityEngine;

namespace PokemonGame.Dialogue
{
    /// <summary>
    /// Can be extended by a class to create something that triggers dialogue
    /// </summary>
    public class DialogueTrigger : MonoBehaviour
    {
        public bool dialogueIsRunning;

        protected event EventHandler DialogueWasCalled;
        protected event EventHandler DialogueFinished;

        /// <summary>
        /// Starts and INK Dialogue sequence from a text asset
        /// </summary>
        /// <param name="inkJson">The text asset that the dialogue sequence draws from</param>
        protected void StartDialogue(TextAsset inkJson)
        {
            if (!DialogueManager.instance.dialogueIsPlaying)
            {
                DialogueManager.instance.EnterDialogueMode(inkJson, this);
                DialogueWasCalled?.Invoke(gameObject, EventArgs.Empty);
                dialogueIsRunning = true;
            }
        }

        /// <summary>
        /// Ends the dialogue and calls the Dialogue Finished event
        /// </summary>
        public void EndDialogue()
        {
            DialogueFinished?.Invoke(gameObject, EventArgs.Empty);
            dialogueIsRunning = false;
        }

        /// <summary>
        /// Inheritors override this to handle tags
        /// </summary>
        /// <param name="tagKey">The tag key</param>
        /// <param name="tagValue">The tag value</param>
        public virtual void CallTag(string tagKey, string tagValue)
        {
            
        }
    }
}
