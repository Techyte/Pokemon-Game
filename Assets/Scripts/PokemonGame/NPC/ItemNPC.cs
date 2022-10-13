using PokemonGame.Game;
using PokemonGame.ScriptableObjects;
using UnityEngine;

namespace PokemonGame.NPCs
{
    public class ItemNPC : NPC
    {
        [SerializeField] private TextAsset TextAsset;
        [SerializeField] private Item itemToGive;

        public override void OnPlayerInteracted()
        {
            StartDialogue(TextAsset);
        }

        public override void CallTag(string TagKey, string TagValue)
        {
            switch (TagKey)
            {
                case "giveItem":
                    InventoryManager.singleton.Add(itemToGive);
                    break;
            }
        }
    }
}