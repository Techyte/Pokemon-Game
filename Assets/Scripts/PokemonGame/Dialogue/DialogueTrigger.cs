using System.Collections.Generic;

namespace PokemonGame.Dialogue
{
    using System;
    using UnityEngine;

    /// <summary>
    /// Can be extended by a class to create something that triggers dialogue
    /// </summary>
    public class DialogueTrigger : MonoBehaviour
    {
        public bool allowsGlobalTags = true;
        [HideInInspector] public bool dialogueIsRunning;

        public event EventHandler DialogueWasCalled;
        public event EventHandler DialogueFinished;

        private bool toldNotToStartYet;

        /// <summary>
        /// Loads an INK Dialogue sequence from a text asset
        /// </summary>
        /// <param name="inkJson">The text asset that the dialogue sequence draws from</param>
        /// <param name="autostart">Automatically start the dialogue on load, on by default</param>
        protected void QueDialogue(TextAsset textAsset, bool autostart, Dictionary<string, string> variables = null)
        {
            if (autostart)
            {
                DialogueManager.instance.QueDialogue(textAsset, this, true, variables);
                DialogueWasCalled?.Invoke(gameObject, EventArgs.Empty);
                dialogueIsRunning = true;   
            }
            else
            {
                DialogueManager.instance.QueDialogue(textAsset, this, false);
                toldNotToStartYet = true;
            }
        }
        
        /// <summary>
        /// Starts an INK Dialogue sequence only if we have already loaded one
        /// </summary>
        protected void StartDialogue()
        {
            if (!DialogueManager.instance.dialogueIsPlaying)
            {
                if (toldNotToStartYet)
                {
                    toldNotToStartYet = false;
                    DialogueManager.instance.StartDialogue(this);
                    DialogueWasCalled?.Invoke(gameObject, EventArgs.Empty);
                    dialogueIsRunning = true;   
                }
            }
        }

        public void SetDialogueVariables(Dictionary<string, string> variables)
        {
            DialogueManager.instance.SetDialogueVariables(variables);
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
        /// <param name="tagValues">The tag values</param>
        public virtual void CallTag(string tagKey, string[] tagValues)
        {
            
        }
    }   
}