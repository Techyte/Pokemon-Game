using System.Collections.Generic;
using PokemonGame.Dialogue;
using PokemonGame.ScriptableObjects;
using UnityEngine;

namespace PokemonGame.Game
{
    public class Bag : MonoBehaviour
    {
        private static Bag _singleton;

        public static Bag singleton
        {
            get => _singleton;
            private set
            {
                if (_singleton == null)
                    _singleton = value;
                else if (_singleton != value)
                {
                    Destroy(value);
                }
            }
        }

        [SerializeField] private KeyCode bagKey;
        [Space]
        [SerializeField] private GameObject BagObject;
        [SerializeField] private PlayerMovement _movement;

        [SerializeField] private bool bagState;

        private void Awake()
        {
            singleton = this;
            DontDestroyOnLoad(this);
        }
        
        private List<Item> items;

        private void Start()
        {
            items = new List<Item>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(bagKey) && !DialogueManager.GetInstance().dialogueIsPlaying)
            {
                ToggleBag();
            }
            
            Debug.Log(Cursor.visible);
            Debug.Log(Cursor.lockState);
        }

        private void ToggleBag()
        {
            bagState = !bagState;
            
            _movement.canMove = !bagState;
            BagObject.SetActive(bagState);
            Cursor.visible = bagState;
            Cursor.lockState = bagState ? CursorLockMode.None : CursorLockMode.Locked;
            
            Debug.Log("Opened bag");
            
        }

        /// <summary>
        /// Adds an item to the bag
        /// </summary>
        /// <param name="itemToAdd">The item to be added</param>
        /// <param name="amount">Amount of items to add</param>
        public void Add(Item itemToAdd, int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                items.Add(itemToAdd);   
            }
        }
    }
}