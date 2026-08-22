using PokemonGame.Game.Party;

namespace PokemonGame.Battle
{
    using Networking;
    
    public class Player
    {
        public int Id;
        public string Name;
        public BattleParty Party;
        public int Team;

        public bool Local;

        /// <summary>
        /// only used in online battles, -1 otherwise
        /// </summary>
        public int NetworkId;

        public Player(NetworkPlayer player, int id)
        {
            Id = id;
            Name = player.Username;
            Party = new BattleParty(player.Party);
            Team = player.Team;
            NetworkId = player.Id;
            Local = BattleNetworkManager.Instance.Client.Id == player.Id; // is this player the local player, used in displaying everything correctly
        }

        public Player(int id, string name, Party party, int team, bool local)
        {
            Id = id;
            Name = name;
            Party = new BattleParty(party);
            Team = team;
            Local = local;
            NetworkId = -1;
        }
    }
}