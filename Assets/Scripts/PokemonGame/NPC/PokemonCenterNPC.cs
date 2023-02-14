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

        public override void CallTag(string tagKey, string tagValue)
        {
            switch (tagKey)
            {
                case "Heal": 
                    PartyManager.Instance.HealAll();
                    break;
            }
        }
    }
}