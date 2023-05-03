using PokemonGame.Battle;

namespace PokemonGame.ScriptableObjects
{
    using System;
    using General;
    using UnityEngine;
    using UnityEngine.Events;
    
    /// <summary>
    /// A move that is used in battles to fight
    /// </summary>
    [CreateAssetMenu(order = 2, fileName = "New Move", menuName = "Pokemon Game/New Move")]
    public class Move : ScriptableObject
    {
        public new string name;
        public Type type;
        public int damage;
        public int basePP;
        public MoveCategory category;
    
        public UnityEvent<MoveMethodEventArgs> MoveMethodEvent;
    
        /// <summary>
        /// Calls the associated function in StatusMoveMethods.cs
        /// </summary>
        /// <param name="e">The MoveMethodArgs that can be used to store additional information to be parsed onto the method</param>
        public void MoveMethod(MoveMethodEventArgs e)
        {
            MoveMethodEvent?.Invoke(e);
        }
    }
    
    /// <summary>
    /// The category of move, Physical, Special or Status
    /// </summary>
    public enum MoveCategory
    {
        Physical,
        Special,
        Status
    }
    
    /// <summary>
    /// Arguments that can be given to a MoveMethod to give it additional information
    /// </summary>
    public class MoveMethodEventArgs : EventArgs
    {
        public Battler target;
        public Battler attacker;
        public Move move;
        public ExternalBattleData battleData;

        public MoveMethodEventArgs(Battler attacker, Battler target, Move move, ExternalBattleData battleData)
        {
            this.target = target;
            this.attacker = attacker;
            this.move = move;
            this.battleData = battleData;
        }
    }

    public struct MovePPData
    {
        public int MaxPP;
        public int CurrentPP;

        public MovePPData(int maxPP, int currentPP)
        {
            MaxPP = maxPP;
            CurrentPP = currentPP;
        }
    }
}