using UnityEngine;

namespace PokemonGame.NPC.Base
{
    public class CivilianNPC : NPC
    {
        [SerializeField] private TextAsset TextAsset;

        protected override void OnPlayerInteracted()
        {
            StartDialogue(TextAsset);
        }
    }
}