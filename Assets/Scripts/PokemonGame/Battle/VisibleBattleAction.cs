using System.Collections.Generic;

namespace PokemonGame.Battle
{
    public class VisibleBattleAction
    {
        public VisibleBattleActionType Type;
        public List<object> Variables;

        public VisibleBattleAction(VisibleBattleActionType type, List<object> variables)
        {
            Type = type;
            Variables = variables;
        }
        
        public VisibleBattleAction(VisibleBattleActionType type)
        {
            Type = type;
            Variables = new List<object>();
        }

        public VisibleBattleAction()
        {
            Variables = new List<object>();
        }
    }

    public enum VisibleBattleActionType
    {
        Move,
        Switch,
        Item,
        Run
    }
}