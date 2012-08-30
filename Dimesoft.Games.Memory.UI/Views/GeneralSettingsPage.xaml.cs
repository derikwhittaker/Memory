using Dimesoft.Games.Memory.Managers;
using Dimesoft.Games.Memory.ViewModels;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Dimesoft.Games.Memory.Views
{
    public sealed partial class GeneralSettingsPage : UserControl
    {
        public GeneralSettingsPage()
        {
            this.InitializeComponent();

            DataContext = new GeneralSettingsViewModel( new StorageManager());            
        }


    }
}
