namespace PokemonGame.Game.Party
{
    using General;

    using UnityEngine;

    /// <summary>
    /// Manages the players party
    /// </summary>
    public class PartyManager : MonoBehaviour
    {
        public static PartyManager Instance;
        
        [SerializeField] private Party _playerParty;

        private void Awake()
        {
            Instance = this;
            for (int i = 0; i < _playerParty.Count; i++)
            {
                _playerParty[i] = Battler.CreateCopy(_playerParty[i]);
            }
        }

        /// <summary>
        /// Add a battler to the player party
        /// </summary>
        /// <param name="battlerToAdd">The battler that you add to the party</param>
        public void AddBattler(Battler battlerToAdd)
        {
            _playerParty.Add(battlerToAdd);
        }

        /// <summary>
        /// Change the player party without simply adding one
        /// </summary>
        /// <param name="party">The new party</param>
        public void UpdatePlayerParty(Party party)
        {
            _playerParty = party;
        }

        public void HealAll()
        {
            Debug.Log("healed all");
            
            for (int i = 0; i < _playerParty.Count; i++)
            {
                _playerParty[i].currentHealth = _playerParty[i].maxHealth;
            }
        }

        public Party GetParty()
        {
            return _playerParty;
        }
    }   
}