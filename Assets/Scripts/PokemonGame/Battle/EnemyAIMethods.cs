using UnityEngine;
using PokemonGame.ScriptableObjects;

namespace PokemonGame.Battle
{
    /// <summary>
    /// Contains all of the logic for every AI
    /// </summary>
    [CreateAssetMenu(fileName = "New Enemy AI Methods", menuName = "All/New Enemy AI Methods")]
    public class EnemyAIMethods : ScriptableObject
    {
        public void DefaultAI(AIMethodEventArgs e)
        {
            int moveCount = 0;
            for (int i = 0; i < e.battlerToUse.moves.Count; i++)
            {
                if (e.battlerToUse.moves[i] != null)
                {
                    moveCount++;
                }
            }

            int moveToDo = Random.Range(0, moveCount);

            Battle.Singleton.enemyMoveToDo = e.battlerToUse.moves[moveToDo];
        }
    }
}