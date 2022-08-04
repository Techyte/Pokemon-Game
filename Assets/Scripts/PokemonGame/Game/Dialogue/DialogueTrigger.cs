using System;
using UnityEngine;

namespace PokemonGame.Dialogue
{
    /// <summary>
    /// Can be extended by a class to create something that triggers dialogue
    /// </summary>
    public class DialogueTrigger : MonoBehaviour
    {
        public event EventHandler DialogueWasCalled;
        public event EventHandler DialogueFinished;

        [Header("Ink JSON")]
        [SerializeField] private TextAsset inkJson;

        protected void StartDialogue()
        {
            if (!DialogueManager.GetInstance().dialogueIsPlaying)
            {
                DialogueManager.GetInstance().currentTrigger = this;
                DialogueManager.GetInstance().EnterDialogueMode(inkJson);
                DialogueWasCalled.Invoke(gameObject, EventArgs.Empty);   
            }
        }

        public void EndDialogue()
        {
            DialogueFinished.Invoke(gameObject, EventArgs.Empty);
        }

        public virtual void CallTag(string TagKey, string TagValue)
        {
            
        }
    }
}
