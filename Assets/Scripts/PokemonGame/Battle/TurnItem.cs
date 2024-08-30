namespace PokemonGame.Battle
{
    public enum TurnItem
    {
        StartDelay,
        PlayerMove,
        OpponentMove,
        PlayerSwapBecauseFainted,
        PlayerSwap,
        OpponentSwap,
        PlayerItem,
        OpponentItem,
        EndBattlePlayerWin,
        EndBattleOpponentWin,
        StartOfTurnStatusEffects,
        EndOfTurnStatusEffects,
        PlayerParalysed,
        OpponentParalysed,
        CatchAttempt,
    }   
}