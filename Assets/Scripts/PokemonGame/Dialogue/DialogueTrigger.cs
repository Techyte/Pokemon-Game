using System;
using UnityEngine;

namespace PokemonGame.Dialogue
{
    /// <summary>
    /// Can be extended by a class to create something that triggers dialogue
    /// </summary>
    public class DialogueTrigger : MonoBehaviour
    {
        private int DialogueCalledHandlerMethods;
        private event EventHandler _DialogueWasCalled;
        public event EventHandler DialogueWasCalled
        {
            add
            {
                _DialogueWasCalled += value;
                DialogueCalledHandlerMethods++;

            }
            remove
            {
                _DialogueWasCalled -= value;
                DialogueCalledHandlerMethods--;
            }
        }

        private int DialogueFinishedHandlerMethods;
        private event EventHandler _DialogueFinished;
        public event EventHandler DialogueFinished
        {
            add
            {
                _DialogueFinished += value;
                DialogueFinishedHandlerMethods++;
            }
            remove
            {
                _DialogueFinished -= value;
                DialogueFinishedHandlerMethods--;
            }
        }

        /// <summary>
        /// Starts and INK Dialogue sequence from a text asset
        /// </summary>
        /// <param name="inkJson">The text asset that the dialogue sequence draws from</param>
        protected void StartDialogue(TextAsset inkJson)
        {
            if (!DialogueManager.instance.dialogueIsPlaying)
            {
                DialogueManager.instance.currentTrigger = this;
                DialogueManager.instance.EnterDialogueMode(inkJson);
                if (DialogueCalledHandlerMethods!=0)
                {
                    _DialogueWasCalled.Invoke(gameObject, EventArgs.Empty);     
                } 
            }
        }

        /// <summary>
        /// Ends the dialogue and calls the Dialogue Finished event
        /// </summary>
        public void EndDialogue()
        {
            if (DialogueFinishedHandlerMethods!=0)
            {
                _DialogueFinished.Invoke(gameObject, EventArgs.Empty);   
            }
        }

        /// <summary>
        /// Inheritors override this to handle tags
        /// </summary>
        /// <param name="TagKey">The tag key</param>
        /// <param name="TagValue">The tag value</param>
        public virtual void CallTag(string TagKey, string TagValue)
        {
            
        }
    }
}
