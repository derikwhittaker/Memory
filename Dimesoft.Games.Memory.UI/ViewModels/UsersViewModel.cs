using System.IO;
using Dimesoft.Games.Memory.Common;
using Dimesoft.Games.Memory.Domain.Models;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Dimesoft.Games.Memory.ViewModels
{
    public class UsersViewModel : BaseViewModel
    {
        private ImageSource _image;
        private bool _selected;
        private readonly UserDTO _user;
        
        public UsersViewModel( UserDTO user )
        {
            _user = user;
        }

        public string UserName
        {
            get { return _user.UserName; }
            set { 
                _user.UserName = value;
                OnPropertyChanged("UserName");
            }
        }

        public int Id
        {
            get { return _user.Id; }
        }

        public bool Selected
        {
            get { return _selected; }
            set
            {
                _selected = value;
                OnPropertyChanged("Selected");
            }
        }

        public ImageSource Image
        {
            get
            {
                if (_image == null && _user.ImageBytes != null )
                {
                    var stream = new MemoryStream(_user.ImageBytes);
                    var randomAccessStream = new MemoryRandomAccessStream(stream);

                    randomAccessStream.FlushAsync();

                    var bi = new BitmapImage();
                    bi.ImageFailed += (s, o) =>
                    {
                        var m = 0;
                    };

                    bi.SetSource(randomAccessStream);

                    _image = bi;
                }

                return _image;
            }
        }

        public byte[] ImageBytes
        {
            get { return _user.ImageBytes; }
            set
            {
                _user.ImageBytes = value;

                OnPropertyChanged("Image");
            }
        }

        public UserDTO AsDTO()
        {
            return _user;
        }
    }
}