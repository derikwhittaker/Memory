using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dimesoft.Games.Memory.Domain.Models;

namespace Dimesoft.Games.Memory.Domain.Factories
{
    
    public interface ICategoryFactory
    {
        SetCategoryDTO BuildEasyBoard();

        SetCategoryDTO BuildMediumBoard();

        SetCategoryDTO BuildHardBoard();
        
        IList<SetDTO> PossibleItems { get; }

    }
}
