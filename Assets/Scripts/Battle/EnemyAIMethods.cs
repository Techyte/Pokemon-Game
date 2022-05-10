using UnityEngine;

namespace PokemonGame.Battle
{
    public class EnemyAIMethods : MonoBehaviour
    {
        public void DefaultAI(Battler battlerToUse, Party usableParty, Battle caller)
        {
            Debug.Log("Default AI was called");
            int moveCount = 0;
            for (int i = 0; i < battlerToUse.moves.Length; i++)
            {
                if (battlerToUse.moves[i] != null)
                {
                    moveCount++;
                }
            }

            int MoveToDo = Random.Range(0, moveCount);

            caller.enemyMoveToDo = battlerToUse.moves[MoveToDo];
        }
    }
}