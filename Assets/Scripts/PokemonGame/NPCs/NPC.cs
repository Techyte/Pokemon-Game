using PokemonGame.Dialogue;
using UnityEngine;

namespace PokemonGame.NPCs
{
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

        public virtual void OnPlayerInteracted()
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