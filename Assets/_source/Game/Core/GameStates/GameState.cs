namespace Game.Core
{
    public enum GameState
    {
        NotInited,
        WaitingForPlayers,
        /// <summary>
        /// counting down
        /// </summary>
        BeginingCountDown,
        InProgress,
        Finished,
    }

}
