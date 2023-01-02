using PokemonGame.Dialogue;
using UnityEngine;

namespace PokemonGame.NPC.Base
{
    /// <summary>
    /// Base class of all NPCs, contains functionality for detecting the player nearby and having an OnPlayerInteract() override method
    /// </summary>
    public class NPC : DialogueTrigger
    {
        [Header("Visual Cue")]
        [SerializeField] private GameObject visualCue;
        [Space]

        [SerializeField] private bool playerInRange;
        
        private void Update()
        {
            if (playerInRange)
            {
                visualCue.SetActive(true);
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    OnPlayerInteracted();
                }
            }
            else
            {
                visualCue.SetActive(false);
            }
        }

        /// <summary>
        /// Called when the player interacts with the NPC
        /// </summary>
        protected virtual void OnPlayerInteracted()
        {
            
        }

        private void Awake()
        {
            playerInRange = false;
            visualCue.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.CompareTag("Player"))
                playerInRange = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if(other.gameObject.CompareTag("Player"))
                playerInRange = false;
        }
    }
}