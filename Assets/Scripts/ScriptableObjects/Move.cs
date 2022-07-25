using System;
using UnityEngine;
using UnityEngine.Events;

namespace PokemonGame
{
    [CreateAssetMenu(order = 2, fileName = "New Move", menuName = "Pokemon Game/New Move")]
    public class Move : ScriptableObject
    {
        public new string name;
        public Type type;
        public int damage;
        public MoveCategory category;

        public UnityEvent<MoveMethodEventArgs> MoveMethodEvent;

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

    public enum MoveCategory
    {
        Physical,
        Special,
        Status
    }

    public class MoveMethodEventArgs : EventArgs
    {
        public MoveMethodEventArgs(Battler target)
        {
            this.target = target;
        }
        
        public Battler target;
    }

}