using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dimesoft.Games.Memory.Domain.Models
{
    public class SetDTO
    {
        public SetDTO()
        {
            SetKey = Guid.NewGuid();
        }

        public Guid SetKey { get; private set; }

        public string SetLevelName { get; set; }

        public string Name { get; set; }

        public string NoMatchImagePath { get; set; }

        public string MatchImagePath { get; set; }

    }
}
