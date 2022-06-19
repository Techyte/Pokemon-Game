using PokemonGame.Battle;
using PokemonGame.Dialogue;
using UnityEngine;

namespace PokemonGame.Game
{
    public class PokemonCenterNPC : DialogueTrigger
    {
        [SerializeField] private BattlerTemplate charmander;
        [SerializeField] private BattlerTemplate bulbasaur;
        [SerializeField] private BattlerTemplate squirtle;
        
        public override void CallTag(string TagKey, string TagValue)
        {
            switch (TagKey)
            {
                case "chosenPokemon":
                    Debug.Log("About to choose a pokemon");
                    ChosePokemon(TagValue);
                    break;
            }
            
            base.CallTag(TagKey, TagValue);
        }

        private void ChosePokemon(string TagValue)
        {
            Debug.Log("Chosen Pokemon is: " + TagValue);
            switch (TagValue)
            {
                case "Charmander":
                    PartyManager.Singleton.AddPokemon(new Battler(charmander, 5, null, "Charmander", null, null, null, null));
                    break;
                case "Squirtle":
                    PartyManager.Singleton.AddPokemon(new Battler(squirtle, 5, null, "Squirtle", null, null, null, null));
                    break;
                case "Bulbasaur":
                    PartyManager.Singleton.AddPokemon(new Battler(bulbasaur, 5, null, "Bulbasaur", null, null, null, null));
                    break;
            }
        }
    }   
}
