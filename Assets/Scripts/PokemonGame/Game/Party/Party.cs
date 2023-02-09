using System;

namespace PokemonGame.Game.Party
{
    using General;

    using UnityEngine;
    using System.Collections.Generic;

    /// <summary>
    /// A collection of 6 <see cref="Battler"/>
    /// </summary>
    [Serializable]
    public class Party
    {
        public int Count => PartyCount();
        
        /// <summary>
        /// The actual list of battlers
        /// </summary>
        private List<Battler> party
        {
            get
            {
                return PartyList;
            }
            set
            {
                PartyList = value;
                
                if(PartyList.Count > 6)
                {
                    for (int i = 0; i < PartyList.Count; i++)
                    {
                        if (i > 6)
                            PartyList.Remove(PartyList[i]);
                    }
                }
                
                CheckDefeatedStatus();
            }
        }

        public event EventHandler PartyAllDefeated;

        // Experimental function, WIP DO NOT USE (COULD CAUSE STACK OVERFLOW LOL)
        private void CheckDefeatedStatus()
        {
            if (DefeatedCount() == Count)
            {
                PartyAllDefeated?.Invoke(this, EventArgs.Empty);
            }
        }

        private int DefeatedCount()
        {
            var partyCount = 0;

            for (int i = 0; i < party.Count; i++)
            {
                if (party[i])
                {
                    if(party[i].isFainted)
                    {
                        partyCount++;
                    }
                }
            }

            return partyCount;
        }

        private int PartyCount()
        {
            var partyCount = 0;

            for (int i = 0; i < party.Count; i++)
            {
                if (party[i])
                {
                    partyCount++;
                }
            }

            return partyCount;
        }

        public void Add(Battler battlerToAdd)
        {
            party.Add(battlerToAdd);
        }
        
        public Battler this[int i]
        {
            get => party[i];
            set
            {
                party[i] = value;
                Debug.Log("updating party");
                CheckDefeatedStatus();
            }
        }
        
        [SerializeField] private List<Battler> PartyList;
    }   
}