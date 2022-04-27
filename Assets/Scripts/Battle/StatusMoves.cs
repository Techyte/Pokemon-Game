using UnityEngine;

public class StatusMoves : MonoBehaviour
{
    [SerializeField]
    private Battle battle;

    public StatusEffect poisoned;
    public void Toxic(Battler target)
    {
        target.statusEffect = poisoned;
        Debug.Log("Used Toxic");
    }
}
