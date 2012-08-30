using System.Collections.ObjectModel;
using Dimesoft.Games.Memory.ViewModels;
using Dimesoft.Games.Memory.Domain.Factories;

namespace Dimesoft.Games.Memory.Data
{
    public class SampleMemoryCardDataSource
    {

        public SampleMemoryCardDataSource()
        {

            //var board = new ColorFactory().BuildEasyBoard();
            var board = new ColorFactory().BuildMediumBoard();
            //var board = new ColorFactory().BuildHardBoard();

            foreach (var set in board.Sets)
            {
                // add each one 2x because they are pairs
                MemoryCards.Add(new GameboardCardSetItem(set));
                MemoryCards.Add(new GameboardCardSetItem(set));
            }
        }

        private ObservableCollection<GameboardCardSetItem> _memoryCards = new ObservableCollection<GameboardCardSetItem>();
        public ObservableCollection<GameboardCardSetItem> MemoryCards
        {
            get { return this._memoryCards; }
        }

    }
}
