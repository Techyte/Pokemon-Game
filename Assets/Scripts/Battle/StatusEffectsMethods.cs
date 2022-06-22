using UnityEngine;

namespace PokemonGame.Battle
{
    [CreateAssetMenu(fileName = "New Status Effect Methods", menuName = "All/New Status Effect Mehods")]
    public class StatusEffectsMethods : ScriptableObject
    {
        public void Healthy(Battler target)
        {
            //Debug.Log(target.name + " was healthy");
        }

        public void Poisoned(Battler target)
        {
            target.currentHealth -= target.maxHealth / 16;

            Debug.Log(target.name + " was hurt by poison");
        }
    }
}
