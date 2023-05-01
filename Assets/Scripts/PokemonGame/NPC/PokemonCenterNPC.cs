namespace PokemonGame.NPC
{
    using Game.Party;
    using UnityEngine;

    public class PokemonCenterNPC : NPC
    {
        [SerializeField] private TextAsset textAsset;
        [SerializeField] private string test;

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
                    PartyManager.Instance.HealAll();
                    //DialogueManager.instance.SetGlobalVariable("pokemon", test);
                    break;
            }
        }
    }
}