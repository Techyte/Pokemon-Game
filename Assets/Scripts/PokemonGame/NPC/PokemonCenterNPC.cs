using PokemonGame.Game;
using PokemonGame.Game.Party;
using UnityEngine;

namespace PokemonGame.NPC
{
    public class PokemonCenterNPC : Base.NPC
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
                    PartyManager.Instance.HealAll();
                    break;
            }
        }
    }
}
