using UnityEngine;
using PokemonGame.ScriptableObjects;

namespace PokemonGame.Battle
{
    /// <summary>
    /// Contains all of the logic for every status effect
    /// </summary>
    [CreateAssetMenu(fileName = "New All Status Effects", menuName = "All/New All Status Effect Methods")]
    public class StatusEffectsMethods : ScriptableObject
    {
        public void Healthy(StatusEffectEventArgs args)
        {
            //Debug.Log(args.battler.name + " was healthy");
        }

        public void Poisoned(StatusEffectEventArgs args)
        {
            args.battler.TakeDamage(args.battler.maxHealth / 16);

            //Debug.Log(args.battler.name + " was hurt by poison");
        }
    }
}
