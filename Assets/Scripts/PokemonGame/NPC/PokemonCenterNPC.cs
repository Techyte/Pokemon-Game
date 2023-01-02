using PokemonGame.Game;
using UnityEngine;

namespace PokemonGame.NPC
{
    public class PokemonCenterNPC : Base.NPC
    {
        [SerializeField] private TextAsset TextAsset;

        protected override void OnPlayerInteracted()
        {
            StartDialogue(TextAsset);
        }

        public override void CallTag(string tagKey, string tagValue)
        {
            switch (tagKey)
            {
                case "chosenPokemon":
                    ChosePokemon(tagValue);
                    break;
            }
        }

        private void ChosePokemon(string tagValue)
        {
            switch (tagValue)
            {
                case "Heal": 
                    PartyManager.HealAll();
                    break;
            }
        }
    }
}
