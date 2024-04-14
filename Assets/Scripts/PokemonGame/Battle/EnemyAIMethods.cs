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
            int moveToDo = Random.Range(0, e.battlerToUse.moves.Count);

            Battle.Singleton.enemyMoveToDo = e.battlerToUse.moves[moveToDo];
        }
    }   
}