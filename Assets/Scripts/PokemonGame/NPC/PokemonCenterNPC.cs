namespace PokemonGame.NPC
{
    using Game.Party;
    using UnityEngine;

    public class PokemonCenterNPC : NPC
    {
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
                case "Heal": 
                    PartyManager.HealAll();
                    break;
            }
        }
    }
}