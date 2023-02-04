using UnityEngine;

namespace PokemonGame.NPC.Base
{
    public class CivilianNPC : NPC
    {
        [SerializeField] private TextAsset textAsset;

        protected override void OnPlayerInteracted()
        {
            StartDialogue(textAsset);
            base.OnPlayerInteracted();
        }
    }
}