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

        /// <summary>
        /// Returns the amount of battlers in the party that are defeated
        /// </summary>
        /// <returns></returns>
        public int DefeatedCount()
        {
            var defeatedCount = 0;

            for (int i = 0; i < party.Count; i++)
            {
                if(party[i].isFainted)
                {
                    defeatedCount++;
                }
            }

            return defeatedCount;
        }

        /// <summary>
        /// Returns the amount of battlers in the party
        /// </summary>
        /// <returns></returns>
        private int PartyCount()
        {
            return party.Count;
        }

        /// <summary>
        /// Returns a complete copy of the party
        /// </summary>
        /// <returns></returns>
        public Party Copy()
        {
            return new Party(this);
        }

        /// <summary>
        /// Adds a battler to the party
        /// </summary>
        /// <param name="battlerToAdd">Battler to add to the party</param>
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