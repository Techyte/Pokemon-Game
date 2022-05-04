using UnityEngine;

public class EnemyAIMethods : MonoBehaviour
{
    public static void DefaultAI(Battler battlerToUse, Party usableParty, Battle caller)
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
