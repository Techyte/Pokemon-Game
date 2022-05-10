using UnityEngine;

namespace PokemonGame.Battle
{
    [CreateAssetMenu]
    public class StatusEffect : ScriptableObject
    {
        public new string name;

        public delegate Battler Effect(Battler target);
        public Effect effect;
    }

}