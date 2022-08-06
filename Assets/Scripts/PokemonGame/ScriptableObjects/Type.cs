using UnityEngine;

namespace PokemonGame.ScriptableObjects
{
    [CreateAssetMenu(order = 3, fileName = "New Type", menuName ="Pokemon Game/New Type")]
    public class Type : ScriptableObject
    {
        public new string name;
        public Type[] strongAgainst;
        public Type[] cantHit;
        public Type[] weakAgainst;
    }

}