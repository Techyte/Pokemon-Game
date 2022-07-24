using UnityEngine;

namespace PokemonGame.Battle
{
    [CreateAssetMenu(fileName = "New Status Move Methods", menuName = "All/New Status Move Mehods")]
    public class StatusMovesMethods : ScriptableObject
    {
        public static void Toxic(object sender, MoveMethodEventArgs e)
        {
            e.target.statusEffect = AllStatusEffects.effects["Poisoned"];
            Debug.Log("Used Toxic on " + e.target.name);
        }

        public static void Ember(object sender, MoveMethodEventArgs e)
        {
            Debug.Log("Used Ember on " + e.target.name);
        }

        public static void RazorLeaf(object sender, MoveMethodEventArgs e)
        {
            Debug.Log("Used Razor Leaf on " + e.target.name);
        }

        public static void Tackle(object sender, MoveMethodEventArgs e)
        {
            Debug.Log("Used Tackle Leaf on " + e.target.name);
        }
    }
}