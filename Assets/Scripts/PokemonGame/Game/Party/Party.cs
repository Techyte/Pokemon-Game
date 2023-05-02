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

        public Party(Party partyOrigin)
        {
            party = new List<Battler>();
            for (int i = 0; i < partyOrigin.Count; i++)
            {
                party.Add(Battler.CreateCopy(partyOrigin[i]));
            }
        }

        public Party()
        {
            party = new List<Battler>();
        }
        
        /// <summary>
        /// The actual list of battlers
        /// </summary>
        public List<Battler> party
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
                
                OnSet();
            }
        }

        protected virtual void OnSet()
        {
            
        }

        public int DefeatedCount()
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

        public Party Copy()
        {
            return new Party(this);
        }

        public void Add(Battler battlerToAdd)
        {
            party.Add(battlerToAdd);
        }
        
        public virtual Battler this[int i]
        {
            get
            {
                return party[i];
            }
            set
            {
                party[i] = value;
            }
        }
        
        [SerializeField] private List<Battler> PartyList;
    }   
}