using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dimesoft.Games.Memory.Common;
using Dimesoft.Games.Memory.Domain;
using Dimesoft.Games.Memory.Domain.Common;
using Dimesoft.Games.Memory.Domain.Factories;
using Dimesoft.Games.Memory.Domain.Models;
using Dimesoft.Games.Memory.Managers;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Dimesoft.Games.Memory.ViewModels
{
    public class GameBoardViewModel : BaseViewModel
    {
        public const string TileIdPattern = "GameboardBoard.{0}.{1}.{2}";
        
        private IStorageManager _storageManager = new StorageManager();
        private IPinManager _pinManager;
        private AudioManager _audioManager = new AudioManager(new StorageManager());

        private IOptionsFactory _optionsFactory = new OptionsFactory();
        private DispatcherTimer _gameClockTimer;
        private IList<SetDTO> _sets;
        private GameMode _currentGameMode;
        private SetCategoryDTO _category;
        private Frame _rootFrame;

        public GameBoardViewModel()
        {
            _pinManager = new PinManager(_storageManager, _optionsFactory);
            _audioManager = new AudioManager(_storageManager);
        }
        
        public async void Init(Frame rootFrame, SetCategoryDTO category, int userId)
        {
            var users = await _storageManager.Users();
            CurrentUser = new UsersViewModel(users.FirstOrDefault(x => x.Id == userId));

            PageTitle = category.Name;
            CurrentGameBoardLevel = category.LevelName;

            _category = category;

            CreateGameboard(category.Sets);
            CreateLeaderboard(_category.Name, _category.LevelName, CurrentUser.Id);
            
            _rootFrame = rootFrame;
                        
        }

        private void CreateGameboard(IList<SetDTO> sets)
        {
            _sets = sets;

            ShuffleBoard(true);
        }

        private async void CreateLeaderboard( string category, string level, int playerId )
        {
            LeaderBoardItems.Clear();

            var results = await _storageManager.LeaderBoardResultsAsync(category, level);

            foreach (var result in results.Where(x => x.PlayerId == playerId))
            {
                LeaderBoardItems.Add(new GameLeaderBoardItem(result));
            }

            OnPropertyChanged("LeaderBoardItems");
            OnPropertyChanged("HasLeaderBoardEntries");
        }

        private void CreateMemoryCards(IList<SetDTO> sets)
        {
            var gameboardSets = new List<GameboardCardSetItem>();

            MemoryCards.Clear();

            foreach (var set in sets)
            {
                var setItemPair1 = new GameboardCardSetItem(set);
                var setItemPair2 = new GameboardCardSetItem(set);

                gameboardSets.Add(setItemPair1);
                gameboardSets.Add(setItemPair2);
            }

            gameboardSets.Shuffle();
            
            foreach (var gameboard in gameboardSets)
            {
                MemoryCards.Add(gameboard);
            }
        }

        private async void OnAskToUnPauseGame(GameboardCardSetItem selectedMatchItem)
        {
            var yesSelected = false;

            var dialog = new MessageDialog("The board is currently paused, would you like to start playing now?");
            var yesHandler = new UICommandInvokedHandler(cmd =>
                                                    {
                                                        yesSelected = true;
                                                    });

            dialog.Commands.Add(new UICommand("Yes", yesHandler));
            dialog.Commands.Add(new UICommand("No"));

            await dialog.ShowAsync();

            if (yesSelected)
            {
                Play();
                GameCardSelected(selectedMatchItem);
            }
        }

        private async void OnAskToStartNewGame(string boardLevelName, Action yesAction)
        {
            var yesSelected = false;
            var dialog = new MessageDialog("Would you like to reset the board and start a new game");
            dialog.Commands.Add(new UICommand("Yes", cmd => { yesSelected = true; }));
            dialog.Commands.Add(new UICommand("No"));

            await dialog.ShowAsync();

            if (yesSelected)
            {
                yesAction.Invoke();
            }
        }

        private async void OnAskUserToLeaveTheGame(Action yesAction)
        {
            var yesSelected = false;
            var dialog = new MessageDialog("Are you sure you would like to leave this game?");
            dialog.Commands.Add(new UICommand("Yes", cmd => { yesSelected = true; }));
            dialog.Commands.Add(new UICommand("No"));

            await dialog.ShowAsync();

            if (yesSelected)
            {
                yesAction.Invoke();
            }
        }

        private async void OnTellUserTheyWon(Action yesAction, Action noAction)
        {
            var dialog = new MessageDialog("Congratulations you have won! Would you like to add your name to the Leader Board?");
            dialog.Commands.Add(new UICommand("Yes", cmd => yesAction.Invoke()));
            dialog.Commands.Add(new UICommand("No", cmd => noAction.Invoke()));

            await dialog.ShowAsync();
        }

        private void GoBack()
        {
            Action callback = () => 
            {
                if (_rootFrame != null && _rootFrame.CanGoBack)
                {
                    _rootFrame.GoBack();
                }
                else
                {
                    _rootFrame.Navigate(typeof(Dashboard));
                }

            };

            if (IsGameInProgress)
            {
                OnAskUserToLeaveTheGame(() => { callback.Invoke(); } );
            }
            else
            {
                callback.Invoke();
            }
        }

        private void GameCardSelected(GameboardCardSetItem selectedMatchItem)
        {
            if (selectedMatchItem.CardState == CardState.Match) { return; }
            if (selectedMatchItem.CardState == CardState.Flipped) { return; }

            if ( IsGameAwaitingToBeStarted )
            {
                Play();
            }
            else if (IsGamePaused)
            {
                // need to ask the user to do something special here
                OnAskToUnPauseGame(selectedMatchItem);    
                return;
            }

            _audioManager.PlayItemSelected();
            
            IsMatch(selectedMatchItem, () => _audioManager.PlaySelectedItemMatch());
        }

        private void IsMatch(GameboardCardSetItem selectedMatchItem, Action isMatchAction)
        {
            // this way we can show the front to the user
            selectedMatchItem.CardState = CardState.Flipped;
            
            var tempTimer = new DispatcherTimer { Interval = new TimeSpan(0,0,0,0,500) };
            tempTimer.Tick += (s,e) =>
            {
                tempTimer.Stop();

                if (PotentialMatchItem == null)
                {
                    PotentialMatchItem = selectedMatchItem;
                    UpdateCardInListStatus(selectedMatchItem.SetKey, CardState.Flipped);
                    SelectedMemoryCard = null;
                }
                else
                {
                    if (IsSameCardInstance(PotentialMatchItem, selectedMatchItem))
                    {
                        UpdateCardInListStatus(selectedMatchItem.Key, CardState.Initial);
                    }
                    else if (IsSameCardType(PotentialMatchItem, selectedMatchItem))
                    {
                        // we have a match
                        UpdateCardInListStatus(selectedMatchItem.Key, CardState.Match);
                        UpdateCardInListStatus(PotentialMatchItem.Key, CardState.Match);

                        // update the match stats
                        OnPropertyChanged("TotalMatches");

                        // if all cards are matches than we ROCK and can be done
                        if (IsAllMatchesFound())
                        {
                            Pause();
                            _audioManager.PlayWonGame();                            
                            CurrentGameMode = GameMode.Completed;

                            OnTellUserTheyWon(() => 
                            {
                                // open the leaderboard
                                ActiveLeaderBoardItem = new GameLeaderBoardItem(new GameResultDTO
                                                                    {
                                                                        GameCategory = _category.Key,
                                                                        GameLevel = CurrentGameBoardLevel,
                                                                        PlayerId = CurrentUser.Id,
                                                                        PlayerName = CurrentUser.UserName,
                                                                        Attempts = TotalAttempts,
                                                                        GameDate = DateTime.Now,
                                                                        GameTime = new TimeSpan(0, 0, GameDurationInSeconds)
                                                                    });

                                SaveLeaderBoardItem();
                                
                                ResetBoard();
                            }
                            ,
                            () => { ResetBoard(); });

                        }
                        else
                        {
                            isMatchAction.Invoke();
                        }
                    }
                    else
                    {
                        ResetAllNonMatches();

                        TotalAttempts = TotalAttempts + 1;
                    }

                    PotentialMatchItem = null;
                }
            };

            tempTimer.Start();

            
        }

        private bool IsAllMatchesFound()
        {
            var matches = MemoryCards.Where(x => x.CardState == CardState.Match).Count() / 2;
            var totalPossible = MemoryCards.Count / 2;

            return matches == totalPossible;
        }

        private bool IsSameCardInstance(GameboardCardSetItem instance1, GameboardCardSetItem instance2)
        {
            return instance1.Key == instance2.Key;
        }

        private bool IsSameCardType(GameboardCardSetItem instance1, GameboardCardSetItem instance2)
        {
            return instance1.SetKey == instance2.SetKey;
        }

        private void Play()
        {
            CurrentGameMode = GameMode.InProgress;
            _gameClockTimer.Start();
        }

        private void Pause()
        {
            CurrentGameMode = GameMode.Paused;

            _gameClockTimer.Stop();
        }

        private async void PinBoard()
        {
            var secondaryTilePath = _optionsFactory.SecondaryTileForCategory(_category.Key);
            var shortName = string.Format("{0}: ({1})", _category.Name, _category.LevelName);
            var description = string.Format("Memory - {0}", _category.Name);
            var pinId = string.Format(TileIdPattern, _category.Name, _category.LevelName, CurrentUser.Id);
            var deeplink = string.Format( "GameboardBoardPage={0}|{1}|{2}", _category.Key, _category.LevelName, CurrentUser.Id );

            await _pinManager.Pin(shortName, description, pinId, deeplink, secondaryTilePath, "Assets/30x30Tile.png");

            OnPropertyChanged("IsCurrentGameBoardPinned");
        }

        private async void UnpinBoard()
        {
            var pinId = string.Format(TileIdPattern, _category.Name, _category.LevelName, CurrentUser.Id);

            await _pinManager.UnPin(pinId);

            OnPropertyChanged("IsCurrentGameBoardPinned");
        }

        private void ResetAllNonMatches()
        {
            foreach (var card in MemoryCards.Where(x => x.CardState == CardState.Flipped ) )
            {
                card.CardState = CardState.Initial;
            }
        }

        private void ResetBoard()
        {
            if (IsGameInProgress)
            {
                OnAskToStartNewGame(CurrentGameBoardLevel, () => { StartNewGame(); });
            }
            else
            {
                StartNewGame();
            }
        }

        private void StartNewGame()
        {

            try
            {
                CurrentGameMode = GameMode.Loading;

                foreach (var card in MemoryCards)
                {
                    card.CardState = CardState.Initial;
                }

                MemoryCards.Shuffle();

                TotalAttempts = 0;
                GameDurationInSeconds = 0;

                SetGameTimer();

                Play();

                PotentialMatchItem = null;

                OnPropertyChanged("TotalMatches");

            }
            finally
            {
                CurrentGameMode = GameMode.NotStarted;
            }
        }

        private void SetGameTimer()
        {

            if (_gameClockTimer != null)
            {
                _gameClockTimer.Tick -= GameClockTimerTick;
            }

            _gameClockTimer = new DispatcherTimer
                                        {
                                            Interval = new TimeSpan(0, 0, 1)
                                        };

            _gameClockTimer.Tick += GameClockTimerTick;
        }

        private void SetGameBoardLevelMode(string boardLevelName)
        {
            // need to prompt the users before we do all this
            if (boardLevelName == CurrentGameBoardLevel) { return; }

            Action callback = () => 
            {
                try
                {
                    CurrentGameMode = GameMode.Loading;

                    var categoryFactory = _optionsFactory.SelectedCategory(_category.Key);

                    switch (boardLevelName)
                    {
                        case LevelConstants.EasyLevel:
                            _category = categoryFactory.BuildEasyBoard();
                            break;

                        case LevelConstants.HardLevel:
                            _category = categoryFactory.BuildHardBoard();
                            break;

                        case LevelConstants.MediumLevel:
                        default:
                            _category = categoryFactory.BuildMediumBoard();
                            break;
                    }

                    _sets = _category.Sets;

                    CurrentGameBoardLevel = boardLevelName;
                    CreateGameboard(_sets);
                    CreateLeaderboard(_category.Name, CurrentGameBoardLevel, CurrentUser.Id);

                    OnPropertyChanged("IsCurrentGameBoardPinned");

                }
                finally
                {
                    CurrentGameMode = GameMode.NotStarted;
                }
            };

            if (IsGameInProgress)
            {
                OnAskToStartNewGame(boardLevelName, callback.Invoke);
            }
            else
            {
                callback.Invoke();
            }
        }

        private void ShuffleBoard(bool force)
        {
            Action callback = () =>
                {
                    CreateMemoryCards(_sets);

                    TotalAttempts = 0;
                    GameDurationInSeconds = 0;

                    SetGameTimer();
                    
                    PotentialMatchItem = null;

                    OnPropertyChanged("TotalMatches");
                };

            if (!force)
            {
                OnAskToStartNewGame(CurrentGameBoardLevel, callback);
            }
            else
            {
                callback.Invoke();
            }
        }

        private void GameClockTimerTick(object sender, object e)
        {
            GameDurationInSeconds = GameDurationInSeconds + 1;
        }                

        private void UpdateCardInListStatus( string key, CardState state)
        {
            var match = MemoryCards.FirstOrDefault(x => x.Key == key);

            if (match == null) { return; }

            match.CardState = state;
        }

        private RelayCommand _goBackCommand;
        public RelayCommand GoBackCommand
        {
            get
            {
                return _goBackCommand ?? (_goBackCommand = new RelayCommand(GoBack));
            }
        }

        private RelayCommand _pinBoardCommand;
        public RelayCommand PinBoardCommand
        {
            get
            {
                return _pinBoardCommand ?? (_pinBoardCommand = new RelayCommand(PinBoard));
            }
        }

        private RelayCommand _unpinBoardCommand;
        public RelayCommand UnPinBoardCommand
        {
            get
            {
                return _unpinBoardCommand ?? (_unpinBoardCommand = new RelayCommand(UnpinBoard));
            }
        }

        private RelayCommand _playBoardCommand;
        public RelayCommand PlayBoardCommand
        {
            get
            {
                return _playBoardCommand ?? (_playBoardCommand = new RelayCommand(Play));
            }
        }

        private RelayCommand _pauseBoardCommand;
        public RelayCommand PauseBoardCommand
        {
            get
            {
                return _pauseBoardCommand ?? (_pauseBoardCommand = new RelayCommand(Pause));
            }
        }

        private RelayCommand _resetBoardCommand;
        public RelayCommand ResetBoardCommand
        {
            get
            {
                return _resetBoardCommand ?? (_resetBoardCommand = new RelayCommand(ResetBoard));
            }
        }

        private RelayCommand _shuffleBoardCommand;
        public RelayCommand ShuffleBoardCommand
        {
            get
            {
                return _shuffleBoardCommand ?? (_shuffleBoardCommand = new RelayCommand(() => ShuffleBoard(false)));
            }
        }

        private RelayCommand<string> _setGameBoardModeCommand;
        public RelayCommand<string> SetGameBoardModeCommand
        {
            get
            {
                return _setGameBoardModeCommand ?? (_setGameBoardModeCommand = new RelayCommand<string>(SetGameBoardLevelMode));
            }
        }

        private GameboardCardSetItem _potentialMatchItem;
        public GameboardCardSetItem PotentialMatchItem
        {
            get { return _potentialMatchItem; }

            set { _potentialMatchItem = value; }
        }

        private ObservableCollection<GameboardCardSetItem> _memoryCards = new ObservableCollection<GameboardCardSetItem>();
        public ObservableCollection<GameboardCardSetItem> MemoryCards
        {
            get { return _memoryCards; }
            set
            {
                _memoryCards = value;
                OnPropertyChanged("MemoryCards");
            }
        }

        private ObservableCollection<GameLeaderBoardItem> _leaderBoardItems = new ObservableCollection<GameLeaderBoardItem>();
        public ObservableCollection<GameLeaderBoardItem> LeaderBoardItems
        {
            get { return _leaderBoardItems; }
            set
            {
                _leaderBoardItems = value;
                OnPropertyChanged("LeaderBoardItems");
            }
        }

        public UsersViewModel CurrentUser { get; set; }

        private string _currentGameBoardMode;
        public string CurrentGameBoardLevel
        {
            get { return _currentGameBoardMode; }
            set 
            { 
                _currentGameBoardMode = value;
                OnPropertyChanged("CurrentGameBoardLevel");
            }
        }

        public string TotalMatches
        {
            get
            { 
                var matches = MemoryCards.Where(x => x.CardState == CardState.Match).Count() / 2;
                var totalPossible = MemoryCards.Count / 2;

                return string.Format("{0} / {1}", matches, totalPossible);
            }
        }       

        private int _totalAttempts = 0;
        public int TotalAttempts
        {
            get { return _totalAttempts; }
            set 
            { 
                _totalAttempts = value;

                OnPropertyChanged("TotalAttempts");
            }
        }
        
        public bool IsGamePaused
        {
            get { return CurrentGameMode == GameMode.Paused; }
        }

        public bool IsGameInProgress
        {
            get { return CurrentGameMode == GameMode.InProgress; }
        }

        public bool IsGameAwaitingToBeStarted
        {
            get { return CurrentGameMode == GameMode.NotStarted; }
        }

        public bool IsBoardBeingLoaded
        {
            get { return CurrentGameMode == GameMode.Loading; }
        
        }

        public bool IsCurrentGameBoardPinned
        {
            get
            {
                var pinId = string.Format(TileIdPattern, _category.Name, _category.LevelName, CurrentUser.Id);

                return _pinManager.IsPinned(pinId);
            }
        }

        public bool HasLeaderBoardEntries
        {
            get { return LeaderBoardItems.Count > 0; }
        }

        private bool _showScoreBoxDialog = false;
        public bool ShowScoreBoxDialog
        {
            get { return _showScoreBoxDialog; }
            set 
            { 
                _showScoreBoxDialog = value;
                OnPropertyChanged("ShowScoreBoxDialog");
            }
        }

        private int _gameDurationInSeconds = 0;
        public int GameDurationInSeconds
        {
            get { return _gameDurationInSeconds; }
            set 
            {     
                _gameDurationInSeconds = value;

                OnPropertyChanged("GameDurationInSeconds");
                OnPropertyChanged("GameDuration");
            }
        }

        private bool _allowBottomBarToShow = true;
        public bool AllowBottomBarToShow
        {
            get { return _allowBottomBarToShow; }
            set
            {
                _allowBottomBarToShow = value;
                OnPropertyChanged("AllowBottomBarToShow");
            }
        }

        private bool _allowTopBarToShow = true;
        public bool AllowTopBarToShow
        {
            get { return _allowTopBarToShow; }
            set
            {
                _allowTopBarToShow = value;
                OnPropertyChanged("AllowTopBarToShow");
            }
        }

        public string GameDuration 
        {
            get { return new TimeSpan(0,0,_gameDurationInSeconds).ToString("g") ; }
        }

        private GameboardCardSetItem _selectedMemoryCard;
        public GameboardCardSetItem SelectedMemoryCard
        {
            get { return _selectedMemoryCard; }

            set
            {
                _selectedMemoryCard = value;

                if (value != null)
                {
                    GameCardSelected(value);
                }

                OnPropertyChanged("SelectedMemoryCard");
            }
        }


        #region Score Card Logic

        private void CloseLeaderBoardEntry()
        {
            ShowScoreBoxDialog = false;
        }

        private RelayCommand _closeLeaderBoardEntryCommand;
        public RelayCommand CloseLeaderBoardEntryCommand
        {
            get
            {
                return _closeLeaderBoardEntryCommand ?? (_closeLeaderBoardEntryCommand = new RelayCommand(CloseLeaderBoardEntry));
            }
        }

        private async void SaveLeaderBoardItem()
        {
            // do something, 

            await _storageManager.AddGameResultAsync(ActiveLeaderBoardItem.GameResult);

            CreateLeaderboard(_category.Name, CurrentGameBoardLevel, CurrentUser.Id);

            CloseLeaderBoardEntry();
        }

        private RelayCommand _saveLeaderBoardItem;
        public RelayCommand SaveLeaderBoardItemCommand
        {
            get
            {
                return _saveLeaderBoardItem ?? (_saveLeaderBoardItem = new RelayCommand(SaveLeaderBoardItem) );
            }
        }

        private GameLeaderBoardItem _activeLeaderBoardItem;        

        public GameLeaderBoardItem ActiveLeaderBoardItem
        {
            get { return _activeLeaderBoardItem; }
            set 
            { 
                _activeLeaderBoardItem = value;
                OnPropertyChanged("ActiveLeaderBoardItem");
            }
        }

        public GameMode CurrentGameMode
        {
            get { return _currentGameMode; }
            set { _currentGameMode = value; }
        }

        #endregion

    }

}

