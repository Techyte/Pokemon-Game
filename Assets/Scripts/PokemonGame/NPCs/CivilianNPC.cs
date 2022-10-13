using UnityEngine;

namespace PokemonGame.NPCs
{
    public class CivilianNPC : NPC
    {
        [SerializeField] private TextAsset TextAsset;

        public override void OnPlayerInteracted()
        {
            StartDialogue(TextAsset);
        }
    }
}