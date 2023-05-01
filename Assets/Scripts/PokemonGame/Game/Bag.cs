using System.Collections.Generic;
using PokemonGame.ScriptableObjects;
using PokemonGame.Game;
using UnityEngine;

namespace PokemonGame.General
{
    public class Bag : MenuBase
    {
        public static Bag Instance;
        
        [SerializeField] private GameObject itemDisplayHolder;
        [SerializeField] private GameObject itemDisplayGameObject;
        
        private ItemType _currentSortingType;
        
        private Dictionary<Item, BagItemData> _items = new Dictionary<Item, BagItemData>();
        
        private void Awake()
        {
            Instance = this;
        }

        private void OnEnable()
        {
            UpdateBagUI();
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
            foreach (ItemDisplay child in itemDisplayHolder.transform.GetComponentsInChildren<ItemDisplay>()) {
                Destroy(child.gameObject);
            }
            
            List<BagItemData> sortedItems = new List<BagItemData>();
            foreach (BagItemData item in _items.Values)
            {
                if (item.item.type == _currentSortingType)
                {
                    sortedItems.Add(item);
                }
            }
            
            foreach (BagItemData itemToShow in sortedItems)
            {
                ItemDisplay display = Instantiate(itemDisplayGameObject, Vector3.zero, Quaternion.identity,
                    itemDisplayHolder.transform).GetComponent<ItemDisplay>();
                display.NameText.text = itemToShow.item.name + " " + "x" +itemToShow.amount;
                display.TextureImage.sprite = itemToShow.item.sprite;
                display.DescriptionText.text = itemToShow.item.description;
            }
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
                bool wasFound = false;
                foreach (BagItemData itemData in _items.Values)
                {
                    if (itemData.item == itemToAdd)
                    {
                        itemData.amount++;
                        wasFound = true;
                    }
                }
                if(!wasFound)
                {
                    _items.Add(itemToAdd, new BagItemData(itemToAdd));
                }
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