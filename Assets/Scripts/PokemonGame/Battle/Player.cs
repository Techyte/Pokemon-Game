using System.Collections.Generic;
using PokemonGame.Game.Party;

namespace PokemonGame.Battle
{
    public class Player
    {
        public int Id;
        public string Name;
        public BattleParty Party;
        public List<int> ActiveBattlers;
        public int Team;

        public bool Local;
    }
}