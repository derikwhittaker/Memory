using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dimesoft.Games.Memory.Common;
using Dimesoft.Games.Memory.Domain.Data;
using Dimesoft.Games.Memory.Domain.Models;
using Dimesoft.Games.Memory.Shared;
using Dimesoft.Games.Memory.ViewModels;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using XAMLMetroAppIsolatedStorageHelper;

namespace Dimesoft.Games.Memory.Managers
{
    public interface IStorageManager
    {
        Task SavePreferencesAsync(GamePreferences gamePreferences);
        Task<GamePreferences> Preferences();

        Task AddGameResultAsync(GameResultDTO result);
        Task AddUserAsync(UserDTO result);

        Task<IList<GameResultDTO>> TopLeaderBoardResultsAsync();
        Task<IList<GameResultDTO>> LeaderBoardResultsAsync(string category, string level);
        Task<IList<UserDTO>> Users();
        Task CreateGuestSilhouette();
    }

    public class StorageManager : IStorageManager
    {
        public const string DimesoftMemoryFileName = "Dimesoft.Memory";
        public const string UsersStorageFileName = "Dimesoft.Memory.Users";
        public const string PreferencesStorageFileName = "Dimesoft.Memory.Prefernces";

        private static LeaderBoardStorageContainer _leaderBoardStorageContainer = null;
        private static UserStorageContainer _userStorageContainer = null;
        private static GamePreferences _gamePreferences = null;
        private static StorageHelper<LeaderBoardStorageContainer> _leaderBoardStorageHelper;
        private static StorageHelper<UserStorageContainer> _usersStorageHelper;
        private static StorageHelper<GamePreferences> _preferencesStorageHelper;
        

        static StorageManager()
        {
            _leaderBoardStorageHelper = new StorageHelper<LeaderBoardStorageContainer>(StorageType.Local);
            _usersStorageHelper = new StorageHelper<UserStorageContainer>(StorageType.Local);
            _preferencesStorageHelper = new StorageHelper<GamePreferences>(StorageType.Local);
            
        }
        
        public async Task<GamePreferences> Preferences()
        {
            if (_gamePreferences == null)
            {
                _gamePreferences = await _preferencesStorageHelper.LoadASync(PreferencesStorageFileName);
            }

            if ( _gamePreferences == null )
            {
                _gamePreferences = new GamePreferences();
            }

            return _gamePreferences;
        }

        public async Task SavePreferencesAsync(GamePreferences gamePreferences)
        {
            _gamePreferences = gamePreferences;

            _preferencesStorageHelper.SaveASync(gamePreferences, PreferencesStorageFileName);
        }

        #region Leader Boards

        public async Task<IList<GameResultDTO>> TopLeaderBoardResultsAsync()
        {
            var results = new List<GameResultDTO>();

            if (_leaderBoardStorageContainer == null)
            {
                _leaderBoardStorageContainer = await _leaderBoardStorageHelper.LoadASync(DimesoftMemoryFileName);
            }

            // do this check here because it is possible the Load will return a null if nothing is found
            if (_leaderBoardStorageContainer != null)
            {
                results = _leaderBoardStorageContainer.Results                    
                    .OrderBy(x => x.Attempts)
                    .Take(5).ToList();
            }

            return results;
        }

        public async Task<IList<GameResultDTO>> LeaderBoardResultsAsync(string category, string level)
        {
            var results = new List<GameResultDTO>();

            if (_leaderBoardStorageContainer == null)
            {
                _leaderBoardStorageContainer = await _leaderBoardStorageHelper.LoadASync(DimesoftMemoryFileName);
            }

            // do this check here because it is possible the Load will return a null if nothing is found
            if (_leaderBoardStorageContainer != null)
            {
                results = _leaderBoardStorageContainer.Results
                    .Where(x => x.GameCategory == category && x.GameLevel == level)
                    .OrderByDescending(x => x.GameTime)
                    .ToList();
            }
                                               
            return results;
        }

        public async Task AddGameResultAsync(GameResultDTO result)
        {
            if (_leaderBoardStorageContainer == null)
            {
                _leaderBoardStorageContainer = new LeaderBoardStorageContainer();
            }

            _leaderBoardStorageContainer.Results.Add(result);

            _leaderBoardStorageHelper.SaveASync(_leaderBoardStorageContainer, DimesoftMemoryFileName);
        }

        #endregion

        #region Users

        public async Task<IList<UserDTO>> Users()
        {
            try
            {
                var results = new List<UserDTO>();

                if ( _userStorageContainer == null )
                {
                    _userStorageContainer = await _usersStorageHelper.LoadASync(UsersStorageFileName);    
                }

                if ( _userStorageContainer != null )
                {
                    results = _userStorageContainer.Users.ToList();
                }

                return results;
            }
            catch (Exception e)
            {
                var v = e.Message;
                throw;
            }
        }

        public async Task AddUserAsync(UserDTO user)
        {
            if (_userStorageContainer == null)
            {
                _userStorageContainer = new UserStorageContainer();
            }

            if ( user.Id == 0 )
            {
                var lastUser = _userStorageContainer.Users.OrderBy(x => x.Id).LastOrDefault() ?? new UserDTO();

                user.Id = lastUser.Id + 1;

                if ( user.ImageBytes == null || !user.ImageBytes.Any())
                {
                    user.ImageBytes = await GetGuestSilhouetteImageBytesAsync();
                }

                _userStorageContainer.Users.Add(user);
            }
            else
            {
                var foundUser = _userStorageContainer.Users.FirstOrDefault(x => x.Id == user.Id);

                if (foundUser == null )
                {
                    _userStorageContainer.Users.Add(user);
                }
                else
                {
                    var index = _userStorageContainer.Users.IndexOf(foundUser);
                    _userStorageContainer.Users[index] = user;
                }
            }

            _usersStorageHelper.SaveASync(_userStorageContainer, UsersStorageFileName);
        }

        #endregion

        public async Task CreateGuestSilhouette()
        {
            if ( !DoesGuestSilhouetteExist() )
            {
                await CreateNewGuestSilhouetteAsync();
            }
        }

        public bool DoesGuestSilhouetteExist()
        {
            var usersTask =  Users();

            Task.WaitAny(usersTask);

            var users = usersTask.Result;

            if (users == null) { return false; }

            return users.Any();
        }

        private async Task CreateNewGuestSilhouetteAsync()
        {
            try
            {
                var bytes = await GetGuestSilhouetteImageBytesAsync();

                var guestUser = new UserDTO
                {
                    Id = 1,
                    UserName = UserConstants.GuestSilhouetteUserName,
                    ImageName = UserConstants.GuestSilhouetteImageName,
                    ImageBytes = bytes
                };

                AddUserAsync(guestUser);

            }
            catch (Exception e)
            {
                var m = e.Message;
                throw;
            }

        }

        private async Task<byte[]> GetGuestSilhouetteImageBytesAsync()
        {
            var packageLocation = Windows.ApplicationModel.Package.Current.InstalledLocation;
            var assetsFolder = await packageLocation.GetFolderAsync("Assets");
            var systemFolder = await assetsFolder.GetFolderAsync("System");
            var guestSilhouetteImage = await systemFolder.GetFileAsync(UserConstants.GuestSilhouetteImageName);

            var bytes = await new StorageFileHelper().ToArrayAsync(guestSilhouetteImage);
            return bytes;
        }
    }
}
