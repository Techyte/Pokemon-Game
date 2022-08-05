using System;
using UnityEngine;
using UnityEngine.Events;

namespace PokemonGame
{
    /// <summary>
    /// A move that is used in battles to fight
    /// </summary>
    [CreateAssetMenu(order = 2, fileName = "New Move", menuName = "Pokemon Game/New Move")]
    public class Move : ScriptableObject
    {
        public new string name;
        public Type type;
        public int damage;
        public MoveCategory category;

        public UnityEvent<MoveMethodEventArgs> MoveMethodEvent;

        /// <summary>
        /// Calls the associated function in StatusMoveMethods.cs
        /// </summary>
        /// <param name="e">The MoveMethodArgs that can be used to store additional information to be parsed onto the method</param>
        public void MoveMethod(MoveMethodEventArgs e)
        {
            try
            {
                MoveMethodEvent.Invoke(e);
            }
            catch
            {
                Debug.LogWarning($"{name}s effect does not have a function associated with it");
            }
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