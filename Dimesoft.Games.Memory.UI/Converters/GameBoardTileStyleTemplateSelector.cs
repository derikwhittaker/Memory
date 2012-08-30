using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dimesoft.Games.Memory.Domain;
using Dimesoft.Games.Memory.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Dimesoft.Games.Memory.Converters
{
    public class GameBoardTileStyleTemplateSelector : DataTemplateSelector
    {
        protected override Windows.UI.Xaml.DataTemplate SelectTemplateCore(object item, Windows.UI.Xaml.DependencyObject container)
        {
            var cardItem = item as GameboardCardSetItem;
            var styleName = "";
            switch (cardItem.LevelName)
            {
                case LevelConstants.EasyLevel:
                    styleName = "GameboardCardTileEasyModeDataTemplate";      
                    break;

                case LevelConstants.HardLevel:
                    styleName = "GameboardCardTileHardModeDataTemplate";
                    break;

                case LevelConstants.MediumLevel:                    
                default:
                    styleName = "GameboardCardTileMediumModeDataTemplate";
                    break;
            }

            var template = App.Current.Resources[styleName] as DataTemplate;

            return template;
        }
    }

    public class GameBoardTileFilledStyleTemplateSelector : DataTemplateSelector
    {
        protected override Windows.UI.Xaml.DataTemplate SelectTemplateCore(object item, Windows.UI.Xaml.DependencyObject container)
        {
            var cardItem = item as GameboardCardSetItem;
            var styleName = "";
            switch (cardItem.LevelName)
            {
                case LevelConstants.EasyLevel:
                    styleName = "GameboardCardTileEasyFilledDataTemplate";
                    break;

                case LevelConstants.HardLevel:
                    styleName = "GameboardCardTileHardFilledDataTemplate";
                    break;

                case LevelConstants.MediumLevel:
                default:
                    styleName = "GameboardCardTileMediumFilledDataTemplate";
                    break;
            }

            var template = App.Current.Resources[styleName] as DataTemplate;

            return template;
        }
    }

    public class GameBoardTileSnappedStyleTemplateSelector : DataTemplateSelector
    {
        protected override Windows.UI.Xaml.DataTemplate SelectTemplateCore(object item, Windows.UI.Xaml.DependencyObject container)
        {
            var cardItem = item as GameboardCardSetItem;
            var styleName = "";
            switch (cardItem.LevelName)
            {
                case LevelConstants.EasyLevel:
                    styleName = "GameboardCardTileEasySnappedDataTemplate";
                    break;

                case LevelConstants.HardLevel:
                    styleName = "GameboardCardTileHardSnappedDataTemplate";
                    break;

                case LevelConstants.MediumLevel:
                default:
                    styleName = "GameboardCardTileMediumSnappedDataTemplate";
                    break;
            }

            var template = App.Current.Resources[styleName] as DataTemplate;

            return template;
        }
    }
}
