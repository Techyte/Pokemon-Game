using System.Collections.Generic;
using UnityEngine;

namespace PokemonGame
{
    [CreateAssetMenu]
    public class BattlerTemplate : ScriptableObject
    {
        public new string name;
        public Type primaryType;
        public Type secondaryType;
        public List<Move> moves;
        public Sprite texture;
        [Space]
        [Header("Stats")]
        public int baseHealth;
        public int baseAttack;
        public int baseDefense;
        public int baseSpecialAttack;
        public int baseSpecialDefense;
        public int baseSpeed;
    }

}