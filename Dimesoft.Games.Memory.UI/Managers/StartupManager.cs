using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace Dimesoft.Games.Memory.Managers
{

    public class StartupManager
    {

        public void InitStartup()
        {
            var storageManager = new StorageManager();

            storageManager.CreateGuestSilhouette();
            
        }
    }
}
