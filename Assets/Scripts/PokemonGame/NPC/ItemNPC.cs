using PokemonGame.Game;
using PokemonGame.ScriptableObjects;
using UnityEngine;
using PokemonGame;

namespace PokemonGame.NPCs
{
    public class ItemNPC : NPC
    {
        [SerializeField] private TextAsset TextAsset;

        public override void OnPlayerInteracted()
        {
            StartDialogue(TextAsset);
        }

        public override void CallTag(string TagKey, string TagValue)
        {
            switch (TagKey)
            {
                case "giveItem":
                    Bag.singleton.Add(AllItems.items[TagValue]);
                    break;
            }
        }
    }
}