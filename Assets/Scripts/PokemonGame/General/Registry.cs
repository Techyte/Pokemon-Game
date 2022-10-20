using PokemonGame.ScriptableObjects;
using UnityEngine;

public class Registry
{
    /// <summary>
    /// Gets the All Items asset
    /// </summary>
    /// <returns>The All Items asset</returns>
    public static AllItems GetAllItemsReference()
    {
        return Resources.Load<AllItems>("Pokemon Game/Items/All Items");
    }
    
    /// <summary>
    /// Gets the All Ais asset
    /// </summary>
    /// <returns>The All Ais asset</returns>
    public static AllAis GetAllAisReference()
    {
        return Resources.Load<AllAis>("Pokemon Game/Ais/All Ais");
    }
    
    /// <summary>
    /// Gets the All Moves asset
    /// </summary>
    /// <returns>The All Moves asset</returns>
    public static AllMoves GetAllMovesReference()
    {
        return Resources.Load<AllMoves>("Pokemon Game/Moves/All Moves");
    }
    
    /// <summary>
    /// Gets the All Status Effects asset
    /// </summary>
    /// <returns>The All Status Effects asset</returns>
    public static AllStatusEffects GetAllStatusEffectsReference()
    {
        return Resources.Load<AllStatusEffects>("Pokemon Game/Status Effects/All Status Effects");
    }
}
