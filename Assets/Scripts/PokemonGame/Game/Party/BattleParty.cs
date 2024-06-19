﻿using System;
using PokemonGame.General;
using UnityEngine;

namespace PokemonGame.Game.Party
{
    [Serializable]
    public class BattleParty : Party
    {
        public event EventHandler PartyAllDefeated;

        /// <summary>
        /// Check if the battle party is defeated, calls defeated event
        /// </summary>
        public void CheckDefeatedStatus()
        {
            Debug.Log("checking defeated status");
            if (DefeatedCount() >= Count)
            {
                Debug.Log("party all defeated");
                PartyAllDefeated?.Invoke(this, EventArgs.Empty);
            }
        }

        protected override void OnSet()
        {
            // CheckDefeatedStatus();
        }

        public override Battler this[int i]
        {
            get
            {
                return party[i]; 
            }
            set
            {
                party[i] = value;
                // CheckDefeatedStatus();
            }
        }

        public BattleParty(Party party)
        {
            this.party = party.party;
        }
    }
}