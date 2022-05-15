using UnityEngine;

public class PokemonCenterNPC : NPC
{
    public override void OnInteract()
    {
        Debug.Log("Overided OnInteract");
        base.OnInteract();
    }
}
