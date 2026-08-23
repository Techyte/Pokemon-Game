using System.Collections.Generic;

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
        public void SwitchBattler(AISwitchEventArgs e)
        {
            for (int i = e.currentIndex; i < e.usableParty.Count + e.currentIndex; i++)
            {
                int index = i % e.usableParty.Count;

                if (!e.usableParty[index].isFainted)
                {
                    e.newBattlerIndex = index;
                    return;
                }
            }
        }
        
        public static void WildPokemon(Battle b, int i)
        {
            for (int j = 0; j < b.battlersEach; j++)
            {
                int moveToDo = Random.Range(0, b.activeBattlers[i][j].moves.Count);
            
                Battle.Singleton.PlayerChooseMove(i, j, moveToDo, new List<(int, int)>
                {
                    (0, 0)
                });
            }
        }
        
        public void DefaultAI(Battle b, int i)
        {
            for (int j = 0; j < b.battlersEach; j++)
            {
                int moveToDo = Random.Range(0, b.activeBattlers[i][j].moves.Count);
            
                Battle.Singleton.PlayerChooseMove(i, j, moveToDo, new List<(int, int)>
                {
                    (0, 0)
                });
            }
        }
    }   
}