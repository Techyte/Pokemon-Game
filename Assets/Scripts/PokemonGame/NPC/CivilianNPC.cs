namespace PokemonGame.NPC
{
    using UnityEngine;

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