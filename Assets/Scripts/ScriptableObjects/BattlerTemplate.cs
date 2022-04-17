using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BattlerTemplate : ScriptableObject
{
    public new string name;
    public Type primaryType;
    public Type secondaryType;
    public List<Move> moves;
    public Sprite texture;
    public float maxHealth;
}
