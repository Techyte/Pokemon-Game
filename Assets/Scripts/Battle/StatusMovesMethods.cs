using UnityEngine;

namespace PokemonGame.Battle
{
    public class StatusMovesMethods
    {
        public static AllStatusEffects allStatusEffects;

        public void Toxic(Battler target)
        {
            target.statusEffect = allStatusEffects.effects["Poisoned"];
            Debug.Log("Used Toxic on " + target.name);
        }

        public void Ember(Battler target)
        {
            Debug.Log("Used Ember on " + target.name);
        }

        public void RazorLeaf(Battler target)
        {
            Debug.Log("Used Razor Leaf on " + target.name);
        }

        public void Tackle(Battler target)
        {
            Debug.Log("Used Tackle Leaf on " + target.name);
        }
    }
}