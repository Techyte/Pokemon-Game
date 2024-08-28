using System;
using PokemonGame.Battle;
using PokemonGame.General;
using UnityEngine.Events;

namespace PokemonGame.ScriptableObjects
{
    using UnityEngine;


    [CreateAssetMenu(fileName = "New Item", menuName = "Pokemon Game/New Item")]
    public class Item : ScriptableObject
    {
        public new string name;
        public Sprite sprite;
        public ItemType type;
        public string description;
        public int cost;
        public int sellPrice;
        public bool useInBattle;
        [ConditionalHide("useInBattle", 1)] public bool lockedTarget;
        [ConditionalHide("lockedTarget", 1)] public bool playerParty;
        [ConditionalHide("lockedTarget", 1)] public int targetIndex;
        public bool heldItem;
    
        public UnityEvent<ItemMethodEventArgs> ItemMethodEvent;

        public void ItemMethod(ItemMethodEventArgs e)
        {
            ItemMethodEvent?.Invoke(e);
        }
    }
    
    /// <summary>
    /// Arguments that can be given to a MoveMethod to give it additional information
    /// </summary>
    public class ItemMethodEventArgs : EventArgs
    {
        public Battler target;
        public Item item;

        public ItemMethodEventArgs(Battler target, Item item)
        {
            this.target = target;
            this.item = item;
        }
    }

    public enum ItemType : int
    {
        Item = 1,
        PokeBall,
        BattleItem,
        Medicine,
        TM,
        TR,
        Berry,
        KeyItem,
        HeldItem,
    }

    public enum ItemUseType : int
    {
        Battle = 1,
        Held
    }
}