using UnityEngine;

[CreateAssetMenu]
public class EnemyAI : ScriptableObject
{
    public new string name;

    public delegate void AIMethod(Battler battlerToUse, Party usableParty, Battle caller);
    public AIMethod aiMethod;
}
