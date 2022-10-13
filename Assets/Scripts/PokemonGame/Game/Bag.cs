using System.Collections.Generic;
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

        public void Add(Item itemToAdd)
        {
            items.Add(itemToAdd);
        }
    }
}