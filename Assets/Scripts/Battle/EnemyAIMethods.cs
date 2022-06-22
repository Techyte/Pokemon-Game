using UnityEngine;

namespace PokemonGame.Battle
{
    [CreateAssetMenu(fileName = "New Enemy AI Methods", menuName = "All/New Enemy AI Mehods")]
    public class EnemyAIMethods : ScriptableObject
    {
        public void DefaultAI(Battler battlerToUse, Party usableParty, Battle caller)
        {
            int moveCount = 0;
            for (int i = 0; i < battlerToUse.moves.Length; i++)
            {
                if (battlerToUse.moves[i] != null)
                {
                    moveCount++;
                }
            }

            int moveToDo = Random.Range(0, moveCount);

            caller.enemyMoveToDo = battlerToUse.moves[moveToDo];
        }
    }
}