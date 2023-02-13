using System;
using PokemonGame.General;

namespace PokemonGame.Game.Party
{
    public class BattleParty : Party
    {
        public event EventHandler PartyAllDefeated;

        private void CheckDefeatedStatus()
        {
            if (DefeatedCount() == Count)
            {
                PartyAllDefeated?.Invoke(this, EventArgs.Empty);
            }
        }

        protected override void OnSet()
        {
            CheckDefeatedStatus();
        }

        public override Battler this[int i]
        {
            get
            {
                CheckDefeatedStatus();
                return party[i]; 
            }
            set
            {
                party[i] = value;
                CheckDefeatedStatus();
            }
        }

        public BattleParty(Party party)
        {
            this.party = party.party;
        }
    }
}