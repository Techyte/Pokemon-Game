using System;
using System.Collections.Generic;
using UnityEngine;

namespace PokemonGame.ScriptableObjects
{
    /// <summary>
    /// A collection of every item in the game
    /// </summary>
    [CreateAssetMenu(fileName = "New All Items", menuName = "All/New All Items")]
    public class AllItems : ScriptableObject, ISerializationCallbackReceiver
    {
        public List<string> _keys = new List<string>( );
        public List<Item> _values = new List<Item>();

        /// <summary>
        /// The list of every item
        /// </summary>
        public Dictionary<string, Item> items = new Dictionary<string, Item>();

        public Item itemToAdd;

        /// <summary>
        /// Attempts to get an item from the register and handles errors
        /// </summary>
        /// <param name="ItemName">The name of the item that you want to fetch</param>
        /// <param name="item">tThe outputted item</param>
        /// <returns></returns>
        public bool GetItem(string ItemName, out Item item)
        {
            item = null;
            if (items.TryGetValue(ItemName, out Item itemToReturn))
            {
                item = itemToReturn;
                return true;
            }
            
            Debug.LogWarning("Item was not present in the register");
            return false;
        }

        /// <summary>
        /// Adds an item to to the register
        /// </summary>
        /// <param name="itemToAdd">The item you want to add</param>
        public void AddItem(Item itemToAdd)
        {
            if (!items.ContainsKey(itemToAdd.name))
            {
                items.Add(itemToAdd.name, itemToAdd);
            }
            else
            {
                Debug.LogWarning("Item is already in the list, please do not try and add it again");
            }
        }
        
        public void OnBeforeSerialize()
        {
            _keys.Clear();
            _values.Clear();

            foreach (var kvp in items)
            {
                _keys.Add(kvp.Key);
                _values.Add(kvp.Value);
            }
        }

        public void OnAfterDeserialize()
        {
            items = new Dictionary<string, Item>();

            for (int i = 0; i != Math.Min(_keys.Count, _values.Count); i++)
                items.Add(_keys[i], _values[i]);
        }
    }
}