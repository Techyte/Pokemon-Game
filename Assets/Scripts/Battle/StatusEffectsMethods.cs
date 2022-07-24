using UnityEngine;

namespace PokemonGame.Battle
{
    public class StatusEffectsMethods
    {
        public static void Healthy(object sender, StatusEffectEventArgs args)
        {
            //Debug.Log(target.name + " was healthy");
        }

        public static void Poisoned(Battler target)
        {
            target.currentHealth -= target.maxHealth / 16;

            Debug.Log(target.name + " was hurt by poison");
        }
    }
}
