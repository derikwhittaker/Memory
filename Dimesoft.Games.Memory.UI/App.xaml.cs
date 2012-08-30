using System.Threading.Tasks;
using Dimesoft.Games.Memory.Data;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Dimesoft.Games.Memory.Domain.Factories;
using Dimesoft.Games.Memory.Managers;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Dimesoft.Games.Memory.Views;

// The Split Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234228

namespace Dimesoft.Games.Memory
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// An overview of the Split Application design will be linked to in future revisions of
    /// this template.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;            
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            
            var rootFrameType = typeof(Dashboard);
            var rootFrameArguments = new Dictionary<string, string>();

            if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
            {
                //TODO: Load state from previously suspended application
            }
            else if ( !string.IsNullOrEmpty( args.Arguments ))
            {
                // easy button here, we only pin from one screen
                rootFrameType = typeof(GameBoardPage);
                
                // remove boardName
                var clearnArgs = args.Arguments.Replace("GameboardBoardPage=", "");
                var temp = clearnArgs.Split('|');
                
                rootFrameArguments.Add("BoardId", temp[0]);
                rootFrameArguments.Add("BoardLevel", temp[1]);
                rootFrameArguments.Add("UserId", temp[2]);

                //"GameboardBoardPage=Shapes|Medium"
            }

            // Create a Frame to act navigation context and navigate to the first page,
            // configuring the new page by passing required information as a navigation
            // parameter
            var rootFrame = new Frame();
            rootFrame.Navigate(rootFrameType, rootFrameArguments);

            // Place the frame in the current Window and ensure that it is active
            Window.Current.Content = rootFrame;
            Window.Current.Activate();

            await SetupLiveTiles();
        }

        private async Task SetupLiveTiles()
        {
            var storageManager = new StorageManager();
            var optionsFactory = new OptionsFactory();

            var pinManager = new PinManager(storageManager, optionsFactory);
            pinManager.UpdateMainLiveTile();
            pinManager.UpdateSecondaryTiles();
            // async (board, level) => await storageManager.LeaderBoardResultsAsync(board, level)
        }  

        protected override void OnActivated(IActivatedEventArgs args)
        {
            base.OnActivated(args);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        void OnSuspending(object sender, SuspendingEventArgs e)
        {
            //TODO: Save application state and stop any background activity

            var x = sender;
        }

        
    }
}
