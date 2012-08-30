using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dimesoft.Games.Memory.Common
{
    public class BaseViewModel : Dimesoft.Games.Memory.Common.BindableBase
    {
        public Uri BaseUri = new Uri("ms-appx:/");
        //public Uri BaseUri = new Uri("ms-resource://Metro.LL.LearningToUseShareTargetContract/Files/MainPage.xaml");
        //public Uri BaseUri = new Uri("ms-resource://");

        private string _pageTitle = string.Empty;
        public string PageTitle
        {
            get { return _pageTitle; }
            set
            {
                _pageTitle = value;

                OnPropertyChanged("PageTitle");
            }
        }

    }
}
