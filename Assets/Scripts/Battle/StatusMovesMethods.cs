using UnityEngine;

namespace PokemonGame.Battle
{
    public class StatusMovesMethods
    {
        public static AllStatusEffects allStatusEffects;

        public Battler Toxic(Battler target)
        {
            target.statusEffect = allStatusEffects.effects["Poisoned"];
            target.speed = 4;
            Debug.Log(target.statusEffect);
            Debug.Log("Used Toxic on " + target.name);
            return target;
        }

        public Battler Ember(Battler target)
        {
            Debug.Log("Used Ember on " + target.name);
            return target;
        }

        public Battler RazorLeaf(Battler target)
        {
            Debug.Log("Used Razor Leaf on " + target.name);
            return target;
        }

        public Battler Tackle(Battler target)
        {
            Debug.Log("Used Tackle Leaf on " + target.name);
            return target;
        }
    }
}