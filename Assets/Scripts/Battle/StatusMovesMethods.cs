using UnityEngine;

public class StatusMovesMethods
{
    [SerializeField] AllStatusEffects allStatusEffects;

    public void Toxic(Battler target)
    {
        target.statusEffect = allStatusEffects.Poisoned;
        Debug.Log("Used Toxic on " + target.name);
    }
}
