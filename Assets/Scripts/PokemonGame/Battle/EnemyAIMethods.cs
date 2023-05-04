namespace PokemonGame.Battle
{
    using UnityEngine;
    using ScriptableObjects;

    /// <summary>
    /// Contains all of the logic for every AI
    /// </summary>
    [CreateAssetMenu(fileName = "New Enemy AI Methods", menuName = "All/New Enemy AI Methods")]
    public class EnemyAIMethods : ScriptableObject
    {
        public void DefaultAI(AIMethodEventArgs e)
        {
            int moveCount = 0;
            foreach (var battler in e.battlerToUse.moves)
            {
                if (battler != null)
                {
                    moveCount++;
                }
            }

            int moveToDo = Random.Range(0, moveCount);

            Battle.Singleton.enemyMoveToDo = e.battlerToUse.moves[moveToDo];
            Battle.Singleton.enemyMoveToDoIndex = moveToDo;
        }
    }   
}