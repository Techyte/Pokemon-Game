using UnityEngine;

public class StatusEffects : MonoBehaviour
{
    [SerializeField]
    private Battle battle;
    public void Poisoned()
    {
        Debug.Log(battle.apponentBattler.name + " Was hurt by Poison");
        battle.apponentBattler.currentHealth--;
    }
}
