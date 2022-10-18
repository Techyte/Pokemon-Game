using PokemonGame.ScriptableObjects;
using UnityEngine;

public class Registry
{
    public static AllItems GetAllItesmReference()
    {
        return Resources.FindObjectsOfTypeAll<AllItems>()[0];
    }
    
    public static AllAis GetAllAismReference()
    {
        return Resources.FindObjectsOfTypeAll<AllAis>()[0];
    }
    
    public static AllMoves GetAllMovesReference()
    {
        return Resources.FindObjectsOfTypeAll<AllMoves>()[0];
    }
    
    public static AllStatusEffects GetAllStatusEffectsReference()
    {
        return Resources.FindObjectsOfTypeAll<AllStatusEffects>()[0];
    }
}
