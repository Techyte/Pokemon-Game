namespace PokemonGame.Battle
{
    using General;
    using UnityEngine;
    using ScriptableObjects;

    /// <summary>
    /// Contains all of the logic for every move
    /// </summary>
    [CreateAssetMenu(fileName = "New Moves Methods", menuName = "All/New Moves Methods")]
    public class MovesMethods : ScriptableObject
    {
        public void Toxic(MoveMethodEventArgs e)
        {
            if (Registry.GetStatusEffect("Poisoned", out StatusEffect effect))
            {
                e.target.statusEffect = effect;   
            }
            Debug.Log("Used Toxic on " + e.target.name);
        }

        public void Ember(MoveMethodEventArgs e)
        {
            Debug.Log("Used Ember on " + e.target.name);
        }

        public void RazorLeaf(MoveMethodEventArgs e)
        {
            Debug.Log("Used Razor Leaf on " + e.target.name);
        }

        public void Tackle(MoveMethodEventArgs e)
        {
            Debug.Log("Used Tackle Leaf on " + e.target.name);
        }
    }   
}