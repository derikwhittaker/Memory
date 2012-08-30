using Dimesoft.Games.Memory.Common;
using Dimesoft.Games.Memory.Domain.Models;

namespace Dimesoft.Games.Memory.ViewModels
{
    public class GameLeaderBoardItem : BaseViewModel
    {
        
        public GameLeaderBoardItem( GameResultDTO gameResult)
        {
            _gameResult = gameResult;
        }

        public string PlayerName 
        { 
            get { return _gameResult.PlayerName; }
            set { _gameResult.PlayerName = value; }
        }

        public int PlayerId
        {
            get { return _gameResult.PlayerId; }
            set { _gameResult.PlayerId = value; }
        }

        public string GameTime
        {
            get { return _gameResult.GameTime.ToString(); } 
        }

        public string GameDate 
        {
            get { return _gameResult.GameDate.ToString("MM/dd/yy"); } 
        }

        public string Attempts 
        { 
            get 
            {
                return _gameResult.Attempts.ToString(); 
            } 
        }

        private GameResultDTO _gameResult;
        public GameResultDTO GameResult
        {
            get { return _gameResult; }
            private set { _gameResult = value; }
        }
    }
}