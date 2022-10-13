using PokemonGame.Game;
using UnityEngine;
using PokemonGame.ScriptableObjects;

namespace PokemonGame.NPCs
{
    public class PokemonCenterNPC : NPC
    {
        [SerializeField] private BattlerTemplate charmander;
        [SerializeField] private BattlerTemplate bulbasaur;
        [SerializeField] private BattlerTemplate squirtle;

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
                case "Charmander": 
                    PartyManager.singleton.AddBattler(Battler.Init(charmander, 5, null, "Charmander", null, null, null, null, true));
                    break;
                case "Squirtle":
                    PartyManager.singleton.AddBattler(Battler.Init(squirtle, 5, null, "Squirtle", null, null, null, null, true));
                    break;
                case "Bulbasaur":
                    PartyManager.singleton.AddBattler(Battler.Init(bulbasaur, 5, null, "Bulbasaur", null, null, null, null, true));
                    break;
            }
        }
    }   
}
