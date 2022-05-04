using UnityEngine;

public class StatusEffectsMethods : MonoBehaviour
{
    [SerializeField]
    private Battle battle;

    public void Poisoned(Battler target)
    {
        target.currentHealth -= target.maxHealth / 16;

        Debug.Log(target.name + " was hurt by poison");
    }
}
