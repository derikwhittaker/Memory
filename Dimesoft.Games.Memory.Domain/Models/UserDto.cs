using Windows.Storage.Streams;

namespace Dimesoft.Games.Memory.Domain.Models
{
    public class UserDTO
    {

        public int Id { get; set; }

        public string UserName { get; set; }

        public string ImageName { get; set; }

        public byte[] ImageBytes { get; set; }
    }
}
