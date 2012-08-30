using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dimesoft.Games.Memory.Domain.Models
{
    public class SetCategoryDTO
    {
        public SetCategoryDTO()
        {
            Sets = new List<SetDTO>();
        }

        public void Populate(IList<SetDTO> sets)
        {
            Sets = sets;
        }

        public IList<SetDTO> Sets { get; private set; }

        public string Name { get; set; }

        public string Key { get; set; }

        public string LevelName { get; set; }
    }
}
