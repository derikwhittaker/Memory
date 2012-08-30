using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dimesoft.Games.Memory.Domain;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Dimesoft.Games.Memory.Converters
{
    public class GameBoardGridStyleConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null) { return null; }

            var styleName = "";
            switch (value.ToString())
            {
                case LevelConstants.EasyLevel:
                    styleName = "GameboardEasyStyle";
                    break;

                case LevelConstants.HardLevel:
                    styleName = "GameboardHardStyle";
                    break;

                case LevelConstants.MediumLevel:
                default:
                    styleName = "GameboardMediumStyle";
                    break;
            }

            var style = App.Current.Resources[styleName] as Style;

            return style;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
