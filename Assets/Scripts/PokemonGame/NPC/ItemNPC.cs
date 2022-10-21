using PokemonGame.Game;
using PokemonGame.ScriptableObjects;
using UnityEngine;

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
                    string[] secondaryValues = TagValue.Split('.');
                    if (Registry.GetItem(secondaryValues[0], out Item item))
                    {
                        Bag.Add(item, int.Parse(secondaryValues[1]));
                    }
                    break;
            }
        }
    }
}