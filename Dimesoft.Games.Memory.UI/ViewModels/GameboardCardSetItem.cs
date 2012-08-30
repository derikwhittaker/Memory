using System;
using Dimesoft.Games.Memory.Common;
using Dimesoft.Games.Memory.Domain;
using Dimesoft.Games.Memory.Domain.Models;

namespace Dimesoft.Games.Memory.ViewModels
{
    public class GameboardCardSetItem : BaseViewModel
    {
        private SetDTO _set;
        public GameboardCardSetItem ( SetDTO set )
        {
            _set = set;
        }

        private CardState _cardState = CardState.Initial;
        public CardState CardState {
            get { return _cardState; }
            set 
            {
                _cardState = value;

                OnPropertyChanged("CardState");
                OnPropertyChanged("Image");
                OnPropertyChanged("Name");
            }
        }
        
        private string _key = Guid.NewGuid().ToString();
        public string Key
        {
            get{ return _key; }
        }

        public string SetKey
        {
            get { return _set.SetKey.ToString(); }
        }

        public string Name 
        { 
            get {
                switch (CardState)
                {
                    case CardState.Initial:                        
                        return _set.Name + " Initial";  

                    case CardState.Match:
                        return _set.Name + " Match";  

                    case CardState.Flipped:
                        return _set.Name + " Filpped";  

                    case CardState.NoMatch:
                    default:
                        return _set.Name + " NoMatch";  
                }
                              
            } 
            set
            {
                OnPropertyChanged("Name");
            }
        }

        public string LevelName
        {
            get { return _set.SetLevelName; }
        }

        public string Image
        {
            get 
            {
                switch (CardState)
                {
                    case CardState.Match:
                    case CardState.Flipped:
                        return _set.MatchImagePath;

                    case CardState.NoMatch:
                    case CardState.Initial:
                    default:
                        return _set.NoMatchImagePath;
                }
            }
        }

    }
}