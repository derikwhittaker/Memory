using System.Threading.Tasks;
using Dimesoft.Games.Memory.Common;
using Dimesoft.Games.Memory.Domain;
using Dimesoft.Games.Memory.Domain.Data;
using Dimesoft.Games.Memory.Managers;

namespace Dimesoft.Games.Memory.ViewModels
{
    public class GeneralSettingsViewModel : BaseViewModel
    {
        private readonly IStorageManager _storageManager;
        private GamePreferences _gamePreferences;
        private bool _playAudio;
        private bool _isEasySetting;
        private bool _isMediumSetting;
        private bool _isHardSetting;

        public GeneralSettingsViewModel(IStorageManager storageManager)
        {
            _storageManager = storageManager;
   
            SetupPreferencesAsync();
        }

        private async void SetupPreferencesAsync()
        {
            if (_gamePreferences == null)
            {
                _gamePreferences = await _storageManager.Preferences();
            }

            PlayAudio = _gamePreferences.PlayAudio;

            switch (_gamePreferences.DefaultLevel)
            {
                case LevelConstants.EasyLevel:
                    IsEasySetting = true;
                    break;

                case LevelConstants.MediumLevel:
                    IsMediumSetting = true;
                    break;

                case LevelConstants.HardLevel:
                    IsHardSetting = true;
                    break;
            }
        }

        public async Task UpdatePreferencesAsync()
        {
            if ( _gamePreferences == null )
            {
                _gamePreferences = await _storageManager.Preferences();
            }

            _gamePreferences.PlayAudio = PlayAudio;

            if ( IsEasySetting )
            {
                _gamePreferences.DefaultLevel = LevelConstants.EasyLevel;
            }
            else if ( IsMediumSetting )
            {
                _gamePreferences.DefaultLevel = LevelConstants.MediumLevel;
            }
            else if (IsHardSetting)
            {
                _gamePreferences.DefaultLevel = LevelConstants.HardLevel;
            }

            _storageManager.SavePreferencesAsync(_gamePreferences);
            
        }

        public bool PlayAudio
        {
            get { return _playAudio; }
            set
            {
                _playAudio = value; 
                OnPropertyChanged("PlayAudio");
            }
        }

        public bool IsEasySetting
        {
            get { return _isEasySetting; }
            set
            {
                _isEasySetting = value; 
                OnPropertyChanged("IsEasySetting");
            }
        }

        public bool IsMediumSetting
        {
            get { return _isMediumSetting; }
            set
            {
                _isMediumSetting = value; 
                OnPropertyChanged("IsMediumSetting");
            }
        }

        public bool IsHardSetting
        {
            get { return _isHardSetting; }
            set
            {
                _isHardSetting = value; 
                OnPropertyChanged("IsHardSetting");
            }
        }
    }
}
