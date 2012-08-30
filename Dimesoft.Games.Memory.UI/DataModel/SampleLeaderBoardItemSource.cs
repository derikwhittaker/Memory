using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dimesoft.Games.Memory.Domain.Models;
using Dimesoft.Games.Memory.ViewModels;

namespace Dimesoft.Games.Memory.Data
{
    public class SampleLeaderBoardItemSource
    {
        public SampleLeaderBoardItemSource()
        {
            LeaderBoardItems.Add(new GameLeaderBoardItem(new GameResultDTO { Attempts = 12, PlayerName = "Derik", GameDate = DateTime.Now, GameTime = new TimeSpan(0, 1, 23) }));
            LeaderBoardItems.Add(new GameLeaderBoardItem(new GameResultDTO { Attempts = 12, PlayerName = "Derik", GameDate = DateTime.Now, GameTime = new TimeSpan(0, 1, 23) }));
            LeaderBoardItems.Add(new GameLeaderBoardItem(new GameResultDTO { Attempts = 12, PlayerName = "Derik", GameDate = DateTime.Now, GameTime = new TimeSpan(0, 1, 23) }));
            LeaderBoardItems.Add(new GameLeaderBoardItem(new GameResultDTO { Attempts = 12, PlayerName = "Derik", GameDate = DateTime.Now, GameTime = new TimeSpan(0, 1, 23) }));
            LeaderBoardItems.Add(new GameLeaderBoardItem(new GameResultDTO { Attempts = 12, PlayerName = "Derik", GameDate = DateTime.Now, GameTime = new TimeSpan(0, 1, 23) }));
            LeaderBoardItems.Add(new GameLeaderBoardItem(new GameResultDTO { Attempts = 12, PlayerName = "Derik", GameDate = DateTime.Now, GameTime = new TimeSpan(0, 1, 23) }));
        }

        private ObservableCollection<GameLeaderBoardItem> _leaderBoardItems = new ObservableCollection<GameLeaderBoardItem>();
        public ObservableCollection<GameLeaderBoardItem> LeaderBoardItems
        {
            get { return _leaderBoardItems; }
            set
            {
                _leaderBoardItems = value;
            }
        }

    }
}
