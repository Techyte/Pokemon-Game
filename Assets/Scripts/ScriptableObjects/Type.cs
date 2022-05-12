using UnityEngine;

namespace PokemonGame
{
    [CreateAssetMenu(order = 2, fileName = "New Type", menuName ="Pokemon Game/New Type")]
    public class Type : ScriptableObject
    {
        public new string name;
        public Type[] strongAgainst;
        public Type[] cantHit;
        public Type[] weakAgainst;
    }

}