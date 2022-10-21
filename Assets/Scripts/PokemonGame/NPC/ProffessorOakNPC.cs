using PokemonGame.Game;
using PokemonGame.ScriptableObjects;
using UnityEngine;

namespace PokemonGame.NPCs
{
    public class ProffessorOakNPC : NPC
    {
        [SerializeField] private BattlerTemplate charmander;
        [SerializeField] private BattlerTemplate bulbasaur;
        [SerializeField] private BattlerTemplate squirtle;
        
        [SerializeField] private TextAsset TextAsset;
        
        public override void OnPlayerInteracted()
        {
            StartDialogue(TextAsset);
        }
        
        public override void CallTag(string TagKey, string TagValue)
        {
            switch (TagKey)
            {
                case "Charmander": 
                    PartyManager.AddBattler(Battler.Init(charmander, 5, null, "Charmander", null, null, null, null, true));
                    break;
                case "Squirtle":
                    PartyManager.AddBattler(Battler.Init(squirtle, 5, null, "Squirtle", null, null, null, null, true));
                    break;
                case "Bulbasaur":
                    PartyManager.AddBattler(Battler.Init(bulbasaur, 5, null, "Bulbasaur", null, null, null, null, true));
                    break;
            }
        }
    }   
}