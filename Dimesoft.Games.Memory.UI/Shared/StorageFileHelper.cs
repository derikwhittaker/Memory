using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Dimesoft.Games.Memory.Shared
{
    public class StorageFileHelper
    {
        public async Task<byte[]> ToArrayAsync(StorageFile storageFile)
        {
            var stream = await storageFile.OpenReadAsync();

            using (var dataReader = new DataReader(stream))
            {
                var bytes = new byte[stream.Size];
                await dataReader.LoadAsync((uint)stream.Size);
                dataReader.ReadBytes(bytes);

                return bytes;
            }
        }
    }

}
