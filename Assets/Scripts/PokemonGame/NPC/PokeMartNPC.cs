using PokemonGame.Dialogue;
using PokemonGame.NPC;
using UnityEngine;

public class PokeMartNPC : NPC
{
    [SerializeField] private TextAsset startPokeMartDialogue;

    protected override void OnPlayerInteracted()
    {
        QueDialogue(startPokeMartDialogue);
        base.OnPlayerInteracted();
    }

    private void OnEnable()
    {
        DialogueManager.instance.DialogueChoice += OnDialogueChoice;
    }

    private void OnDisable()
    {
        DialogueManager.instance.DialogueChoice -= OnDialogueChoice;
    }

    private void OnDialogueChoice(object sender, DialogueChoiceEventArgs e)
    {
        if (e.trigger == this)
        {
            if (e.choiceIndex == 0)
            {
                // Shop
                Debug.Log("We want to shop");
            }
            else
            {
                Debug.Log("We want to sell");
                // Sell
            }
        }
    }
}
