using System.Collections.Generic;
using PokemonGame.Dialogue;
using PokemonGame.ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PokemonGame.Game
{
    public class Bag : MonoBehaviour
    {
        [SerializeField] private KeyCode bagKey;
        [Space]
        [SerializeField] private GameObject BagObject;
        [SerializeField] private PlayerMovement _movement;
        [SerializeField] private GameObject itemDisplayerHolder;
        [SerializeField] private GameObject itemDisplayGameObject;

        [SerializeField] private bool bagState;

        private ItemType _currentSortingType;
        
        private static Dictionary<Item, BagItemData> items = new Dictionary<Item, BagItemData>();

        private void Awake()
        {
            SceneManager.sceneLoaded += SceneManagerOnSceneLoaded;
        }

        private void SceneManagerOnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            if (SceneManager.GetActiveScene().name != "Battle")
            {
                _movement = FindObjectOfType<PlayerMovement>();
            }
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

            List<BagItemData> sortedItems = new List<BagItemData>();
            foreach (BagItemData item in items.Values)
            {
                if (item.item.type == _currentSortingType)
                {
                    sortedItems.Add(item);
                }
            }

            foreach (BagItemData itemToShow in sortedItems)
            {
                ItemDisplay display = Instantiate(itemDisplayGameObject, Vector3.zero, Quaternion.identity,
                    itemDisplayerHolder.transform).GetComponent<ItemDisplay>();

                display.NameText.text = itemToShow.item.name + " " + "x" +itemToShow.amount;
                display.TextureImage.sprite = itemToShow.item.sprite;
                display.DescriptionText.text = itemToShow.item.description;
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(bagKey) && !DialogueManager.instance.dialogueIsPlaying)
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
        public static void Add(Item itemToAdd, int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                bool wasFound = false;
                foreach (BagItemData itemData in items.Values)
                {
                    if (itemData.item == itemToAdd)
                    {
                        itemData.amount++;
                        wasFound = true;
                    }
                }
                if(!wasFound)
                {
                    items.Add(itemToAdd, new BagItemData(itemToAdd));
                }
            }
        }
    }

    public class BagItemData
    {
        public readonly Item item;
        public int amount;

        public BagItemData(Item item)
        {
            this.item = item;
            amount = 1;
        }
    }
}