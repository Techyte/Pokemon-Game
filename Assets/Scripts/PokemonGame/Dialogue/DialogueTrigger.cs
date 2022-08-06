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

        protected void StartDialogue(TextAsset inkJson)
        {
            if (!DialogueManager.GetInstance().dialogueIsPlaying)
            {
                DialogueManager.GetInstance().currentTrigger = this;
                DialogueManager.GetInstance().EnterDialogueMode(inkJson);
                if (DialogueCalledHandlerMethods!=0)
                {
                    _DialogueWasCalled.Invoke(gameObject, EventArgs.Empty);     
                } 
            }
        }

        public void EndDialogue()
        {
            if (DialogueFinishedHandlerMethods!=0)
            {
                _DialogueFinished.Invoke(gameObject, EventArgs.Empty);   
            }
        }

        public virtual void CallTag(string TagKey, string TagValue)
        {
            
        }
    }
}
