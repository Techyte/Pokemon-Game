using System.Collections.Generic;

namespace PokemonGame.ScriptableObjects
{
    using UnityEngine;
    using Battle;
    
    [CreateAssetMenu(fileName = "New Battle Sequence", menuName = "Pokemon Game/New Battle Sequence")]
    public class BattleSequence : ScriptableObject
    {
        [SerializeReference]
        public List<BattleEvent> sequence = new List<BattleEvent>();
 
        public BattleEventsToAdd eventToAdd; 
        
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
                case BattleEventsToAdd.Accuracy:
                    sequence.Add(new Accuracy());
                    break;
                case BattleEventsToAdd.Asleep:
                    sequence.Add(new Asleep());
                    break;
                case BattleEventsToAdd.Confusion:
                    sequence.Add(new Confusion());
                    break;
                case BattleEventsToAdd.PrimaryEffect:
                    sequence.Add(new PrimaryEffect());
                    break;
                case BattleEventsToAdd.TypeResistance:
                    sequence.Add(new TypeResistance());
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
            Accuracy,
            Asleep,
            Confusion,
            PrimaryEffect,
            TypeResistance
        }
    }
}