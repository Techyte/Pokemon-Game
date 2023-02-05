using PokemonGame.Game;
using PokemonGame.General;
using PokemonGame.ScriptableObjects;
using UnityEngine;

namespace PokemonGame.NPC
{
    public class ItemNPC : Base.NPC
    {
        [SerializeField] private TextAsset textAsset;

        protected override void OnPlayerInteracted()
        {
            StartDialogue(textAsset);
            base.OnPlayerInteracted();
        }

        public override void CallTag(string tagKey, string tagValue)
        {
            switch (tagKey)
            {
                case "giveItem":
                    string[] secondaryValues = tagValue.Split('.');
                    if (Registry.GetItem(secondaryValues[0], out Item item))
                    {
                        Bag.Instance.Add(item, int.Parse(secondaryValues[1]));
                    }
                    break;
            }
        }
    }
}