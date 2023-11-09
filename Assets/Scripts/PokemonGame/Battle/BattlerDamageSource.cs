using System.Collections.Generic;
using PokemonGame.General;

namespace PokemonGame.Battle
{
    public class BattlerDamageSource : DamageSource
    {
        public Battler sourceBattler;

        public BattlerDamageSource(Battler sourceBattler)
        {
            this.sourceBattler = sourceBattler;
        }
    }
}