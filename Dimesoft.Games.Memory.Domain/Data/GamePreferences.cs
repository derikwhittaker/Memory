namespace Dimesoft.Games.Memory.Domain.Data
{
    public class GamePreferences
    {
        public GamePreferences()
        {
            PlayAudio = true;
            DefaultLevel = LevelConstants.EasyLevel;
        }

        public bool PlayAudio { get; set; }

        public string DefaultLevel { get; set; }

    }
}
