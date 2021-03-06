using UnityEngine;

namespace PokemonGame.Battle
{
    [CreateAssetMenu(fileName = "New Enemy AI Methods", menuName = "All/New Enemy AI Methods")]
    public class EnemyAIMethods : ScriptableObject
    {
        public static void DefaultAI(object sender, AIMethodEventArgs e)
        {
            int moveCount = 0;
            for (int i = 0; i < e.battlerToUse.moves.Length; i++)
            {
                if (e.battlerToUse.moves[i] != null)
                {
                    moveCount++;
                }
            }

            int moveToDo = Random.Range(0, moveCount);

            Battle.Singleton.enemyMoveToDo = e.battlerToUse.moves[moveToDo];
            //Battle.Singleton.SetEnemyMove(moveToDo);
        }
    }
}