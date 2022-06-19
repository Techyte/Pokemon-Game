using UnityEngine;

namespace PokemonGame.Battle
{
    [CreateAssetMenu(order = 5, fileName = "New AI", menuName = "Pokemon Game/New AI")]
    public class EnemyAI : ScriptableObject
    {
        public new string name;

        public delegate void AIMethod(Battler battlerToUse, Party usableParty, Battle caller);
        public AIMethod aiMethod;
    }

}