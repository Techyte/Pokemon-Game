using UnityEngine;

namespace PokemonGame
{
    [CreateAssetMenu(order = 1, fileName = "New Move", menuName = "Pokemon Game/New Move")]
    public class Move : ScriptableObject
    {
        public new string name;
        public Type type;
        public int damage;
        public MoveCategory category;

        public delegate void MoveMethod(Battler target);
        public MoveMethod moveMethod;
    }

    public enum MoveCategory
    {
        Physical,
        Special,
        Status
    }

}