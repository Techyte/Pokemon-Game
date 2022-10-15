using System.Collections.Generic;
using PokemonGame.Dialogue;
using PokemonGame.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

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
        [SerializeField] private GameObject itemDisplayerHolder;
        [SerializeField] private GameObject itemDisplayGameObject;

        [SerializeField] private bool bagState;

        private ItemType _currentSortingType;
        
        [SerializeField] private List<Item> items = new List<Item>();

        private void Awake()
        {
            singleton = this;
            DontDestroyOnLoad(this);
        }

        /// <summary>
        /// Changes the item type that the player wants to look at
        /// </summary>
        /// <param name="newType">The index of the type you want to filter</param>
        public void ChangeCurrentSortingItem(int newType)
        {
            _currentSortingType = (ItemType)newType;
            UpdateBagUI();
        }

        private void UpdateBagUI()
        { 
            foreach (ItemDisplay child in itemDisplayerHolder.transform.GetComponentsInChildren<ItemDisplay>()) {
                Destroy(child.gameObject);
            }

            List<Item> sortedItems = new List<Item>();
            foreach (Item item in items)
            {
                if (item.type == _currentSortingType)
                {
                    sortedItems.Add(item);
                }
            }

            foreach (Item itemToShow in sortedItems)
            {
                ItemDisplay display = Instantiate(itemDisplayGameObject, Vector3.zero, Quaternion.identity,
                    itemDisplayerHolder.transform).GetComponent<ItemDisplay>();

                display.NameText.text = itemToShow.name;
                display.TextureImage.sprite = itemToShow.sprite;
                display.DescriptionText.text = itemToShow.description;
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(bagKey) && !DialogueManager.GetInstance().dialogueIsPlaying)
            {
                ToggleBag();
            }
        }

        private void ToggleBag()
        {
            bagState = !bagState;
            
            _movement.canMove = !bagState;
            BagObject.SetActive(bagState);
            Cursor.visible = bagState;
            Cursor.lockState = bagState ? CursorLockMode.None : CursorLockMode.Locked;
            
            UpdateBagUI();
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