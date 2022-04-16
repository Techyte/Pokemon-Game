using UnityEngine;

[CreateAssetMenu]
public class Type : ScriptableObject
{
    public new string name;
    public Type[] strongAgainst;
    public Type[] weakAgainst;
}
