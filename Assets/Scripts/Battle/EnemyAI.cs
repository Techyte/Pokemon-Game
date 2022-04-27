using UnityEngine;

[CreateAssetMenu(fileName = "EnemyAI", menuName = "ScriptableObjects/EnemyAI/EnemyAI")]
public class EnemyAI : ScriptableObject
{
    public void EnemyTurn(Battler battlerToUse, Party usableParty, Battle caller)
    {
        int moveCount = 0;
        for(int i = 0; i < battlerToUse.moves.Length; i++)
        {
            if (battlerToUse.moves[i] != null)
            {
                moveCount++;
            }
        }

        int MoveToDo = Random.Range(0, moveCount);

        caller.DoMoveOnPlayer(battlerToUse.moves[MoveToDo]);
    }
}
