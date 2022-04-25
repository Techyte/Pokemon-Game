using UnityEngine;

public class StatusMoves : MonoBehaviour
{
    [SerializeField]
    private Battle battle;

    public StatusEffect poisoned;
    public void Toxic()
    {
        battle.apponentBattler.statusEffect = poisoned;
        Debug.Log("Used Toxic");
    }
}
