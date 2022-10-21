using PokemonGame.Game;
using UnityEngine;
using PokemonGame.ScriptableObjects;

namespace PokemonGame.NPCs
{
    public class PokemonCenterNPC : NPC
    {
        [SerializeField] private TextAsset TextAsset;

        public override void OnPlayerInteracted()
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
