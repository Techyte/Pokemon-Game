namespace PokemonGame.ScriptableObjects
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "New Ability", menuName = "Pokemon Game/New Ability")]
    public class Ability : ScriptableObject
    {
        public new string name;
    }
}