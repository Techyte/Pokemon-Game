namespace PokemonGame.NPC
{
    using Game.Party;
    using General;
    using ScriptableObjects;
    using UnityEngine;

    public class ProffessorOakNPC : NPC
    {
        [SerializeField] private BattlerTemplate charmander;
        [SerializeField] private BattlerTemplate bulbasaur;
        [SerializeField] private BattlerTemplate squirtle;
        
        [SerializeField] private TextAsset textAsset;
        
        protected override void OnPlayerInteracted()
        {
            StartDialogue(textAsset);
            base.OnPlayerInteracted();
        }
        
        public override void CallTag(string tagKey, string[] tagValues)
        {
            switch (tagKey)
            {
                case "chosenPokemon":
                    switch (tagValues[0])
                    {
                        case "Charmander": 
                            PartyManager.AddBattler(Battler.Init(charmander, 5, StatusEffect.Healthy, "Charmander", null, true));
                            break;
                        case "Squirtle":
                            PartyManager.AddBattler(Battler.Init(squirtle, 5, StatusEffect.Healthy, "Squirtle", null, true));
                            break;
                        case "Bulbasaur":
                            PartyManager.AddBattler(Battler.Init(bulbasaur, 5, StatusEffect.Healthy, "Bulbasaur", null, true));
                            break;   
                    }
                    break;
            }
        }
    }
}