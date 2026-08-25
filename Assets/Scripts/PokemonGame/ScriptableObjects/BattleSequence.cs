using System.Collections.Generic;

namespace PokemonGame.ScriptableObjects
{
    using UnityEngine;
    using Battle;
    
    [CreateAssetMenu(fileName = "New Battle Sequence", menuName = "Pokemon Game/New Battle Sequence")]
    public class BattleSequence : ScriptableObject
    {
        public List<BattleEvent> sequence = new List<BattleEvent>();

        [HideInInspector] public BattleEventsToAdd eventToAdd;
        
        public void AddBattleEvent()
        {
            Debug.Log(sequence.Count);
            
            switch (eventToAdd)
            {
                case BattleEventsToAdd.Burn:
                    sequence.Add(new Burn());
                    break;
                case BattleEventsToAdd.ItemUsage:
                    sequence.Add(new ItemUsage());
                    break;
                case BattleEventsToAdd.Moves:
                    sequence.Add(new Moves());
                    break;
                case BattleEventsToAdd.Poison:
                    sequence.Add(new Poison());
                    break;
                case BattleEventsToAdd.Switch:
                    sequence.Add(new Switch());
                    break;
            }
        }
        
        public enum BattleEventsToAdd
        {
            AddBattleEvent,
            Burn,
            ItemUsage,
            Moves,
            Poison,
            Switch,
        }
    }
}