using System.Diagnostics;
using UnityEngine;

namespace PokemonGame.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Pokemon Game/New Item")]
    public class Item : ScriptableObject
    {
        public new string name;
        public Sprite sprite;
        public ItemType type;
        public string description;
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