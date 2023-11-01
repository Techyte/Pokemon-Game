namespace PokemonGame.NPC
{
    using UnityEngine;
    public class DialogueNPC : NPC
    {
        [SerializeField] private TextAsset textAsset;

        protected override void OnPlayerInteracted()
        {
            LoadDialogue(textAsset);
            base.OnPlayerInteracted();
        }
    }   
}
