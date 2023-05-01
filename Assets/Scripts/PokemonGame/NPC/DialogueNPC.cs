using UnityEngine;

namespace PokemonGame.NPC
{
    public class DialogueNPC : NPC
    {
        [SerializeField] private TextAsset textAsset;

        protected override void OnPlayerInteracted()
        {
            StartDialogue(textAsset);
            base.OnPlayerInteracted();
        }
    }   
}
