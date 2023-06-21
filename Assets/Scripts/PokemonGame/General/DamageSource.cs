namespace PokemonGame.General
{
    public abstract class DamageSource
    {
        public static DamageSource Empty => new EmptyDamageSource();
    }
}