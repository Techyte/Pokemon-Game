using System;
using UnityEngine;
using UnityEngine.Events;
using Type = PokemonGame.ScriptableObjects.Type;

namespace PokemonGame
{
    /// <summary>
    /// A move that is used in battles to fight
    /// </summary>
    public interface Move
    {
        public new string name;
        public Type type;
        public int damage;
        public MoveCategory category;

        /// <summary>
        /// Calls the associated function in StatusMoveMethods.cs
        /// </summary>
        /// <param name="e">The MoveMethodArgs that can be used to store additional information to be parsed onto the method</param>
        public void MoveMethod()
        {
            
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
        public MoveMethodEventArgs(Battler target)
        {
            this.target = target;
        }
        
        public Battler target;
    }

}