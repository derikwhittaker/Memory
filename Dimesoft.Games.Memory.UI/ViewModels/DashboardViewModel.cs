using System.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Dimesoft.Games.Memory.Common;
using Dimesoft.Games.Memory.Domain.Factories;
using Dimesoft.Games.Memory.Domain.Models;
using Dimesoft.Games.Memory.Managers;
using Dimesoft.Games.Memory.Shared;
using Dimesoft.Games.Memory.Views;
using Windows.Foundation;
using Windows.Media.Capture;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Controls;

namespace Dimesoft.Games.Memory.ViewModels
{
    public class DashboardViewModel : BaseViewModel
    {
        private IStorageManager _storageManager = new StorageManager();
        private IOptionsFactory _optionsFactory = new OptionsFactory();
        private Frame _rootFrame;

        public DashboardViewModel( Frame rootFrame )
        {
            var options = _optionsFactory.DashboardOptions();
            _rootFrame = rootFrame;

            foreach (var option in options)
            {
                DashboardGroups.Add(option);    
            }

            Loadusers();

            OnPropertyChanged("DashboardGroups");
        }

        private async Task Loadusers()
        {
            var users = await _storageManager.Users();

            if ( !users.Any() )
            {
                await _storageManager.CreateGuestSilhouette();

                users = await _storageManager.Users();
            }

            Users = users.Select(x => new UsersViewModel(x)).ToList();

            SelectedUser = Users.FirstOrDefault();
            SelectedUser.Selected = true;
        }

        private void AddNewUser()
        {
            IsInAddUserMode = true;

            SelectedUser = new UsersViewModel(new UserDTO());
        }
        
        private async void CapturePicture()
        {
            var dialog = new CameraCaptureUI();
            var aspectRatio = new Size(4, 3);
            dialog.PhotoSettings.CroppedAspectRatio = aspectRatio;
            dialog.PhotoSettings.MaxResolution = CameraCaptureUIMaxPhotoResolution.HighestAvailable;
            dialog.PhotoSettings.AllowCropping = true;
            dialog.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Png;

            var file = await dialog.CaptureFileAsync(CameraCaptureUIMode.Photo);
            if (file != null)
            {
                var bytes = await new StorageFileHelper().ToArrayAsync(file);

                SelectedUser.ImageBytes = bytes;
            }
        }

        private async void SelectPicture()
        {
            var filePicker = new FileOpenPicker();
            filePicker.FileTypeFilter.Add(".png");

            filePicker.ViewMode = PickerViewMode.Thumbnail;
            filePicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;

            //filePicker.SettingsIdentifier = "picker1";
            filePicker.CommitButtonText = "Select File to Use";

            var file = await filePicker.PickSingleFileAsync();

            if ( file != null )
            {
                var bytes = await new StorageFileHelper().ToArrayAsync(file);

                SelectedUser.ImageBytes = bytes;                
            }
        }
        
        private async void SaveNewUser()
        {
            await _storageManager.AddUserAsync(SelectedUser.AsDTO());

            var users = await _storageManager.Users();

            Users = users.Select(x => new UsersViewModel(x)).ToList();

            IsInAddUserMode = false;
        }

        private ObservableCollection<DashboardOptionDTO> _itemGroups = new ObservableCollection<DashboardOptionDTO>();
        public ObservableCollection<DashboardOptionDTO> DashboardGroups
        {
            get { return _itemGroups; }
        }
        
        private DashboardOptionDTO _selectedDashboardOption;
        public DashboardOptionDTO SelectedDashboardOption
        {
            get{ return _selectedDashboardOption; }

            set 
            { 
                _selectedDashboardOption = value;

                LoadGameboardAsync();
            }
        }

        private async void LoadGameboardAsync()
        {
            var currentFrame = Windows.UI.Xaml.Window.Current;
            var frame = currentFrame.Content as Frame;
            var preferences = await _storageManager.Preferences();

            var args = new Dictionary<string, string>
                           {
                               {GameBoardSettingConstants.BoardId, _selectedDashboardOption.UniqueId},
                               {GameBoardSettingConstants.BoardLevel, preferences.DefaultLevel},
                               {GameBoardSettingConstants.UserId, SelectedUser.Id.ToString()}
                           };

            frame.Navigate(typeof (GameBoardPage), args);
        }

        private IList<UsersViewModel> _users;
        public IList<UsersViewModel> Users
        {
            get { return _users; }
            set { _users = value; OnPropertyChanged("Users"); }
        }

        private UsersViewModel _selectedUser;
        public UsersViewModel SelectedUser
        {
            get { return _selectedUser; }
            set { _selectedUser = value; OnPropertyChanged("SelectedUser"); }
        }

        private bool _isInAddUserMode;
        public bool IsInAddUserMode
        {
            get { return _isInAddUserMode; }
            set { _isInAddUserMode = value; OnPropertyChanged("IsInAddUserMode"); }
        }

        private UsersViewModel _modifyUserViewModel;
        public UsersViewModel ModifyUserViewModel
        {
            get { return _modifyUserViewModel; }
            set { _modifyUserViewModel = value; }
        }

        private RelayCommand _addNewUserCommand;
        public RelayCommand AddNewUserCommand
        {
            get { return _addNewUserCommand ?? (_addNewUserCommand = new RelayCommand(AddNewUser)); }
        }

        private RelayCommand _cancelNewUserCommand;
        public RelayCommand CancelNewUserCommand
        {
            get {return _cancelNewUserCommand ?? (_cancelNewUserCommand = new RelayCommand(() => { IsInAddUserMode = false; })) ; }
        }

        private RelayCommand _saveNewUserCommand;
        public RelayCommand SaveNewUserCommand
        {
            get { return _saveNewUserCommand ?? (_saveNewUserCommand = new RelayCommand(SaveNewUser)); }
        }

        private RelayCommand _capturePictureCommand;
        public RelayCommand CapturePictureCommand
        {
            get { return _capturePictureCommand ?? (_capturePictureCommand = new RelayCommand(CapturePicture)); }
        }

        private RelayCommand _selectPictureCommand;
        public RelayCommand SelectPictureCommand
        {
            get { return _selectPictureCommand ?? (_selectPictureCommand = new RelayCommand(SelectPicture)); }
        }

    }
}

