namespace PokemonGame.Game
{
    /// <summary>
    /// Manages the players party
    /// </summary>
    public static class PartyManager
    {
        private static Party _playerParty;

        /// <summary>
        /// Add a battler to the player party
        /// </summary>
        /// <param name="battlerToAdd">The battler that you add to the party</param>
        public static void AddBattler(Battler battlerToAdd)
        {
            _playerParty.party.Add(battlerToAdd);
        }

        /// <summary>
        /// Change the player party without simply adding one
        /// </summary>
        /// <param name="party">The new party</param>
        public static void UpdatePlayerParty(Party party)
        {
            _playerParty = party;
        }

        public static void HealAll()
        {
            foreach (var battler in _playerParty.party)
            {
                battler.currentHealth = battler.maxHealth;
            }
        }
    }   
}
