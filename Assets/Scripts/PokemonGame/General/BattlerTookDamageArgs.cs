namespace PokemonGame.General
{
    public class BattlerTookDamageArgs
    {
        public DamageSource source;

        public BattlerTookDamageArgs(DamageSource source)
        {
            this.source = source;
        }
    }
}