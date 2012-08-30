

using System;
using System.Windows.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Dimesoft.Games.Memory.Domain.Models
{
    public class DashboardOptionDTO
    {
        private static Uri _baseUri = new Uri("ms-appx:///");

        public DashboardOptionDTO( int ordinal, string uniqueId, String title, String subtitle, String imagePath)
        {
            Ordinal = ordinal;
            UniqueId = uniqueId;
            Title = title;
            Subtitle = subtitle;
            ImagePath = imagePath;
        }

        public int Ordinal { get; set; }
        public string UniqueId { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }

        private ImageSource _image = null;
        public ImageSource Image
        {
            get
            {
                if (_image == null && ImagePath != null)
                {
                    _image = new BitmapImage(new Uri(DashboardOptionDTO._baseUri, ImagePath));
                }
                return this._image;
            }

            set
            {
                ImagePath = null;
            }
        }


    }
}
