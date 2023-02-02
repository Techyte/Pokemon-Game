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

        [Header("Interactable")]
        public bool interactable = true;

        private bool _playerInRange;
        
        private void Update()
        {
            if(interactable)
            {
                if (_playerInRange)
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
        }

        private void OnValidate()
        {
            Debug.Log("validate");
            if (visualCue == null)
            {
                Debug.Log("interact cue not there");
                visualCue = Instantiate(Resources.Load<GameObject>(@"Pokemon Game\NPC\Interact Cue"), transform);
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
            _playerInRange = false;
            visualCue.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(interactable)
            {
                if (other.gameObject.CompareTag("Player"))
                {
                    _playerInRange = true;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if(interactable)
            {
                if (other.gameObject.CompareTag("Player"))
                {
                    _playerInRange = false;
                }
            }
        }
    }
}