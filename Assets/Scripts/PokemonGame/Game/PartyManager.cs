using UnityEngine;

namespace PokemonGame.Game
{
    /// <summary>
    /// Manages the players party
    /// </summary>
    public class PartyManager : MonoBehaviour
    {
        private static PartyManager _singleton;

        public static PartyManager singleton
        {
            get => _singleton;
            private set
            {
                if (_singleton == null)
                    _singleton = value;
                else if (_singleton != value)
                {
                    Destroy(value);
                }
            }
        }
        
        [SerializeField] private Party playerParty;

        private void Awake()
        {
            singleton = this;
        }

        /// <summary>
        /// Add a battler to the player party
        /// </summary>
        /// <param name="battlerToAdd">The battler that you add to the party</param>
        public void AddBattler(Battler battlerToAdd)
        {
            playerParty.party.Add(battlerToAdd);
        }

        /// <summary>
        /// Change the player party without simply adding one
        /// </summary>
        /// <param name="party">The new party</param>
        public void UpdatePlayerParty(Party party)
        {
            playerParty = party;
        }
        
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
    }   
}
