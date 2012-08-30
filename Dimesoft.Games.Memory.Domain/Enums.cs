namespace Dimesoft.Games.Memory.Domain
{

    public enum CardState
    {
        Initial,
        NoMatch,
        Flipped,
        Match
    }

    public enum GameMode
    {
        NotStarted = 0,
        Loading = 1,
        InProgress = 2,
        Paused = 3,
        Completed = 4
    }
}