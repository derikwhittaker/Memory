using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using Dimesoft.Games.Memory.Domain.Models;
using Dimesoft.Games.Memory.Shared;
using Dimesoft.Games.Memory.ViewModels;
using Dimesoft.Games.Memory.Views;
using Windows.Foundation;
using Windows.UI.ApplicationSettings;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Items Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234233

namespace Dimesoft.Games.Memory
{
    /// <summary>
    /// A page that displays a collection of item previews.  In the Split Application this page
    /// is used to display and select one of the available groups.
    /// </summary>
    public sealed partial class Dashboard : Dimesoft.Games.Memory.Common.LayoutAwarePage
    {
        private Popup _settingsPopup;
        private SettingsPane _settingsPane;
        public Dashboard()
        {
            this.InitializeComponent();
            
            DataContext = new DashboardViewModel(Frame);
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property provides the collection of items to be displayed.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _settingsPane = SettingsPane.GetForCurrentView();
            
            _settingsPane.CommandsRequested += (settingsPane, args) =>
                                                   {
                                                       args.Request.ApplicationCommands.Clear();

                                                       //if ( !args.Request.ApplicationCommands.Any(x => x.Id == "generalSettings") )
                                                       {
                                                           var generalCommand = new SettingsCommand("generalSettings", "Preferences", OnPreferencesCommand);

                                                           args.Request.ApplicationCommands.Add(generalCommand);   
                                                       }
                                                   };
        }

        private void OnPreferencesCommand(IUICommand command)
        {
            var asSettingsCommand = (SettingsCommand) command;
            Contract.Assume(asSettingsCommand != null);

            Window.Current.Activated += OnWindowActivated;

            _settingsPopup = new Popup
                                 {
                                     IsLightDismissEnabled = true, 
                                     Width = 346, 
                                     Height = this.ActualHeight
                                 };
            _settingsPopup.Closed += async (sender, o) =>
                                               {
                                                   var asSettingsPage = _settingsPopup.Child as GeneralSettingsPage;
                                                   Contract.Assume(asSettingsPage != null);

                                                   await ((GeneralSettingsViewModel)asSettingsPage.DataContext).UpdatePreferencesAsync();
                                                   Window.Current.Activated -= OnWindowActivated;                                 
                                                };

            var generalSettingsPage = new GeneralSettingsPage
                                          {
                                              Width = 346,
                                              Height = ActualHeight
                                          };

            _settingsPopup.Child = generalSettingsPage;
            _settingsPopup.SetValue(Canvas.LeftProperty, ActualWidth - 346);
            _settingsPopup.SetValue(Canvas.TopProperty, 0);
            _settingsPopup.IsOpen = true;

        }

        void OnWindowActivated(object sender, Windows.UI.Core.WindowActivatedEventArgs e)
        {
            if (e.WindowActivationState == Windows.UI.Core.CoreWindowActivationState.Deactivated)
            {
                if ( _settingsPopup != null )
                {
                    _settingsPopup.IsOpen = false;
                }
            }
        }
        
     
        private void Panel1Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {            
            StartExitAnimation("SlideToFirstPositionStoryBoard", ((DashboardViewModel)DataContext).DashboardGroups[0]);
            RunAnimation("ScaleTowardsFirstPosition");
            RunAnimation("FadeOutBackgroundStoryBoard");
        }

        private void Panel2Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            StartExitAnimation("SlideToSecondPositionStoryBoard", ((DashboardViewModel)DataContext).DashboardGroups[1]);
            RunAnimation("ScaleTowardsSecondPosition");
            RunAnimation("FadeOutBackgroundStoryBoard");
        }

        private void Panel3Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            StartExitAnimation("SlideToThirdPositionStoryBoard", ((DashboardViewModel)DataContext).DashboardGroups[2]);
            RunAnimation("ScaleTowardsThirdPosition");
            RunAnimation("FadeOutBackgroundStoryBoard");
        }

        private void Panel4Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            StartExitAnimation("SlideToFourthPositionStoryBoard", ((DashboardViewModel)DataContext).DashboardGroups[3]);
            RunAnimation("ScaleTowardsFourthPosition");
            RunAnimation("FadeOutBackgroundStoryBoard");
        }

        private void StartExitAnimation( string storyBoardName, DashboardOptionDTO selectedItem )
        {
            var storyBoard = this.Resources[storyBoardName] as Storyboard;

            if (storyBoard != null)
            {
                storyBoard.Completed += (s, a) =>
                                            {
                                                ((DashboardViewModel) DataContext).SelectedDashboardOption = selectedItem;                                                
                                            };
                storyBoard.Begin();
            }
        }

        private void RunAnimation(string storyboardName )
        {
            var storyBoard = this.Resources[storyboardName] as Storyboard;

            if (storyBoard != null)
            {
                storyBoard.Begin();
            }
        }

        private void Checked(object sender, RoutedEventArgs e)
        {
            var dashboardViewModel = DataContext as DashboardViewModel;
            Contract.Assume(dashboardViewModel != null);

            var asRadioButton = sender as RadioButton;
            Contract.Assume(asRadioButton != null);

            var asViewModel = asRadioButton.DataContext as UsersViewModel;
            Contract.Assume(asViewModel != null);

            foreach (var usersViewModel in dashboardViewModel.Users)
            {
                if ( usersViewModel != asViewModel )
                {
                    usersViewModel.Selected = false;
                }
                else
                {
                    usersViewModel.Selected = true;
                }
            }
        }

    }
}
