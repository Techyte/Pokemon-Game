using UnityEngine;

namespace PokemonGame.Battle
{
    public class StatusEffectsMethods : MonoBehaviour
    {
        public Battler Healthy(Battler target)
        {
            //Debug.Log(target.name + " was healthy");
            return target;
        }

        public Battler Poisoned(Battler target)
        {
            target.currentHealth -= target.maxHealth / 16;

            Debug.Log(target.name + " was hurt by poison");
            return target;
        }
    }
}
