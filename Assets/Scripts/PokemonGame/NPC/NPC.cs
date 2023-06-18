namespace PokemonGame.NPC
{

    using System;
    using Dialogue;
    using Game;
    using UnityEngine;

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
        private Player _player;

        private void Awake()
        {
            _playerInRange = false;
            visualCue.SetActive(false);
            DialogueFinished += StopPlayerLooking;
        }

        private void StopPlayerLooking(object o, EventArgs e)
        {
            _player.StopLooking();
        }

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
            else
            {
                visualCue.SetActive(false);
            }
            OverrideUpdate();
        }

        protected virtual void OverrideUpdate()
        {
            
        }

        private void OnValidate()
        {
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
            _player.LookAtTarget(transform.position);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(interactable)
            {
                if (other.gameObject.CompareTag("Player"))
                {
                    _playerInRange = true;
                    _player = other.GetComponent<Player>();
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
                    _player = null;
                }
            }
        }
    }
}