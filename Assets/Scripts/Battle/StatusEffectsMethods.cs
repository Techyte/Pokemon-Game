using UnityEngine;

public class StatusEffectsMethods
{
    [SerializeField]
    private Battle battle;

    public void Poisoned(Battler target)
    {
        target.currentHealth -= target.maxHealth / 16;

        Debug.Log("Was hurt by Poison");
    }
}
