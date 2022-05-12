using UnityEngine;

namespace PokemonGame.Battle
{
    [CreateAssetMenu(order = 3, fileName = "New Status Effect", menuName = "Pokemon Game/New Status Effect")]
    public class StatusEffect : ScriptableObject
    {
        public new string name;

        public delegate void Effect(Battler target);
        public Effect effect;
    }

}