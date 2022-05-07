using UnityEngine;

namespace PokemonGame.Battle
{
    public class StatusEffectsMethods : MonoBehaviour
    {
        public void Healthy(Battler target)
        {
            Debug.Log(target.name + " was healthy");
        }

        public void Poisoned(Battler target)
        {
            target.currentHealth -= target.maxHealth / 16;

            Debug.Log(target.name + " was hurt by poison");
        }
    }
}
