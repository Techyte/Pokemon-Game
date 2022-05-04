using UnityEngine;

public class StatusMovesMethods : MonoBehaviour
{
    [SerializeField] AllStatusEffects allStatusEffects;

    public void Toxic(Battler target)
    {
        target.statusEffect = allStatusEffects.effects["Poisoned"];
        Debug.Log("Used Toxic on " + target.name);
    }
}
