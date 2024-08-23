namespace PokemonGame.Battle
{
    using UnityEngine;
    using ScriptableObjects;

    /// <summary>
    /// Contains all the logic for every move
    /// </summary>
    [CreateAssetMenu(fileName = "New Item Methods", menuName = "All/New Item Methods")]
    public class ItemMethods : ScriptableObject
    {
        public void Potion(ItemMethodEventArgs e)
        {
            e.target.Heal(20);
        }
    }   
}