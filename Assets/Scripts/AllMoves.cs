using UnityEngine;
using System.Collections.Generic;

public class AllMoves : ScriptableObject
{
    public Dictionary<string, Move> moves = new Dictionary<string, Move>();

    public List<string> myString;

    public Move MoveToAdd;
}
