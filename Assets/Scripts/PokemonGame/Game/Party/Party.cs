using System.Collections;
using UnityEngine;

namespace PokemonGame.Game.Party
{
    using System.Collections.Generic;
    using PokemonGame;

    /// <summary>
    /// A collection of 6 <see cref="Battler"/>
    /// </summary>
    [System.Serializable]
    public class Party
    {
        public int Count => party.Count;
        
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
            }
        }

        public void Add(Battler battlerToAdd)
        {
            party.Add(battlerToAdd);
        }
        
        public Battler this[int i]
        {
            get => party[i];
            set => party[i] = value;
        }
        
        [SerializeField] private List<Battler> PartyList;
    }
}