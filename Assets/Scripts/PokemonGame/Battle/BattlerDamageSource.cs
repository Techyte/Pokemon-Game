using System.Collections.Generic;
using PokemonGame.General;

namespace PokemonGame.Battle
{
    public class BattlerDamageSource : DamageSource
    {
        public Battler sourceBattler;
        public List<Battler> battlersThatParticipated;

        public BattlerDamageSource(Battler sourceBattler, List<Battler> battlersThatParticipated)
        {
            this.sourceBattler = sourceBattler;
            this.battlersThatParticipated = battlersThatParticipated;
        }
    }
}