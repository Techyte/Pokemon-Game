using PokemonGame.Game;
using UnityEngine;
using PokemonGame.ScriptableObjects;

namespace PokemonGame.Battle
{
    /// <summary>
    /// Contains all of the logic for every move
    /// </summary>
    [CreateAssetMenu(fileName = "New Status Move Methods", menuName = "All/New Status Move Methods")]
    public class StatusMovesMethods : ScriptableObject
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