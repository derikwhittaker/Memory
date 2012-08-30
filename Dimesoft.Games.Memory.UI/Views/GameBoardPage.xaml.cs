using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dimesoft.Games.Memory.Common;
using Dimesoft.Games.Memory.Domain;
using Dimesoft.Games.Memory.Domain.Factories;
using Dimesoft.Games.Memory.Domain.Models;
using Dimesoft.Games.Memory.Managers;
using Dimesoft.Games.Memory.ViewModels;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace Dimesoft.Games.Memory.Views
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class GameBoardPage : Dimesoft.Games.Memory.Common.LayoutAwarePage
    {
        public GameBoardPage()
        {
            this.InitializeComponent();
            DataContext = new GameBoardViewModel();

        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {

            RunAnimation("FadeInBackgroundStoryBoard", async () =>
                                                           {
                                                               var parm = e.Parameter as Dictionary<string, string>;

                                                               var boardId = parm[GameBoardSettingConstants.BoardId];
                                                               var boardLevel = parm[GameBoardSettingConstants.BoardLevel];
                                                               var userId = int.Parse(parm[GameBoardSettingConstants.UserId]);

                                                               await SetupBoard(boardId, boardLevel, userId);                                                               
                                                           });

        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            RunAnimation("FadeOutBackgroundStoryBoard", () => base.OnNavigatedFrom(e));
        }


        private async Task SetupBoard(string boardId, string boardLevel, int userId )
        {
            var optionsFactory = new OptionsFactory();
            var categoryFactory = optionsFactory.SelectedCategory(boardId);
            var setCategoryDTO = new SetCategoryDTO();

            switch (boardLevel)
            {
                case LevelConstants.EasyLevel:
                    setCategoryDTO = categoryFactory.BuildEasyBoard();
                    break;

                case LevelConstants.MediumLevel:
                    setCategoryDTO = categoryFactory.BuildMediumBoard();
                    break;

                case LevelConstants.HardLevel:
                    setCategoryDTO = categoryFactory.BuildHardBoard();
                    break;
            }

            ((GameBoardViewModel)DataContext).Init(Frame, setCategoryDTO, userId);

        }

        private void MemoryCardClicked(object sender, ItemClickEventArgs e)
        {
            var clicked = e.ClickedItem as GameboardCardSetItem;

            ((GameBoardViewModel)DataContext).SelectedMemoryCard = clicked;
            
        }

        private void RunAnimation(string storyboardName, Action completedAction)
        {
            var storyBoard = this.Resources[storyboardName] as Storyboard;

            if (storyBoard != null)
            {

                storyBoard.Completed += (s, o) =>
                                            {
                                                if (completedAction != null)
                                                {
                                                    completedAction.Invoke();
                                                }
                                            };

                storyBoard.Begin();
                
            }
        }

    }
}
