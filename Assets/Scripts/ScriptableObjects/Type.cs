using UnityEngine;

namespace PokemonGame
{
    [CreateAssetMenu]
    public class Type : ScriptableObject
    {
        public new string name;
        public Type[] strongAgainst;
        public Type[] cantHit;
        public Type[] weakAgainst;
    }

}