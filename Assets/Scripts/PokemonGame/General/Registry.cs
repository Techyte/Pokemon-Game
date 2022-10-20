using PokemonGame.ScriptableObjects;
using UnityEngine;

public class Registry
{
    public static AllItems GetAllItemsReference()
    {
        return Resources.Load<AllItems>("Pokemon Game/Items/All Items");
    }
    
    public static AllAis GetAllAismReference()
    {
        return Resources.Load<AllAis>("Pokemon Game/Ais/All Ais");
    }
    
    public static AllMoves GetAllMovesReference()
    {
        return Resources.Load<AllMoves>("Pokemon Game/Moves/All Moves");
    }
    
    public static AllStatusEffects GetAllStatusEffectsReference()
    {
        return Resources.Load<AllStatusEffects>("Pokemon Game/Status Effects/All Status Effects");
    }
}
