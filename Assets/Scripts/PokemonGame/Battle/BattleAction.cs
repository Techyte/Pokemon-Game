using System.Collections.Generic;

namespace PokemonGame.Battle
{
    public class BattleAction
    {
        public BattleActionType Type;
        public List<object> Variables;

        public BattleAction(BattleActionType type, List<object> variables)
        {
            Type = type;
            Variables = variables;
        }
        
        public BattleAction(BattleActionType type)
        {
            Type = type;
            Variables = new List<object>();
        }

        public BattleAction()
        {
            Variables = new List<object>();
        }
    }
    
    public class TurnItem
    {
        public TurnItemType Type;
        public List<object> Variables;

        public TurnItem(TurnItemType type, List<object> variables)
        {
            Type = type;
            Variables = variables;
        }
        
        public TurnItem(TurnItemType type)
        {
            Type = type;
            Variables = new List<object>();
        }

        public TurnItem()
        {
            Variables = new List<object>();
        }
    }

    public enum BattleActionType
    {
        Move,
        Switch,
        Item,
        Run
    }
    
    public enum TurnItemType
    {
        StartDelay,
        PlayerMove,
        PlayerSwapBecauseFainted,
        PlayerSwap,
        PlayerLevelUp,
        PlayerEvolved,
        PlayerItem,
        EndBattle,
        StartOfTurnEffects,
        EndOfTurnEffects,
        PlayerParalysed,
        PlayerAsleep,
        CatchAttempt,
        Run,
    }
}