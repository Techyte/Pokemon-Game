namespace PokemonGame.ScriptableObjects
{
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu(order = 1, fileName = "New Battler Template", menuName = "Pokemon Game/New Battler Template")]
    public class BattlerTemplate : ScriptableObject
    {
        public new string name;
        public BasicType primaryType;
        public BasicType secondaryType;
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
        public int baseExpYield;
    }
}