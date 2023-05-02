using System.Collections.Generic;
using PokemonGame.ScriptableObjects;

namespace PokemonGame.Game
{
    public static class Bag
    {
        private static Dictionary<Item, BagItemData> _items = new Dictionary<Item, BagItemData>();
        
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

        public static Dictionary<Item, BagItemData> GetItems()
        {
            return _items;
        }
    }
}