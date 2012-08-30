using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Dimesoft.Games.Memory.Domain.Common;
using Dimesoft.Games.Memory.Domain.Models;

namespace Dimesoft.Games.Memory.Domain.Factories
{
    public class ColorFactory : ICategoryFactory
    {
        private string _noMatchImagePath = "../Assets/Category/BackOfCard.png";

        public ColorFactory()
        {
            PossibleItems = new List<SetDTO>
            {
                new SetDTO{Name = "Black",      NoMatchImagePath = _noMatchImagePath, MatchImagePath = "../Assets/Category/Colors/black.png" },
                new SetDTO{Name = "Pink",       NoMatchImagePath = _noMatchImagePath, MatchImagePath = "../Assets/Category/Colors/brightpink.png" },
                new SetDTO{Name = "Red",        NoMatchImagePath = _noMatchImagePath, MatchImagePath = "../Assets/Category/Colors/red.png" },
                new SetDTO{Name = "Slate Gray", NoMatchImagePath = _noMatchImagePath, MatchImagePath = "../Assets/Category/Colors/slateGray.png" },
                new SetDTO{Name = "Orange",     NoMatchImagePath = _noMatchImagePath, MatchImagePath = "../Assets/Category/Colors/orange.png" },
                new SetDTO{Name = "Purple",     NoMatchImagePath = _noMatchImagePath, MatchImagePath = "../Assets/Category/Colors/purple.png" },
                new SetDTO{Name = "Blue",       NoMatchImagePath = _noMatchImagePath, MatchImagePath = "../Assets/Category/Colors/blue.png" },
                new SetDTO{Name = "Green",      NoMatchImagePath = _noMatchImagePath, MatchImagePath = "../Assets/Category/Colors/green.png" },
                new SetDTO{Name = "Lime",       NoMatchImagePath = _noMatchImagePath, MatchImagePath = "../Assets/Category/Colors/lime.png" },
                new SetDTO{Name = "Yellow",     NoMatchImagePath = _noMatchImagePath, MatchImagePath = "../Assets/Category/Colors/yellow.png" },
                new SetDTO{Name = "Brown",      NoMatchImagePath = _noMatchImagePath, MatchImagePath = "../Assets/Category/Colors/brown.png" },
                new SetDTO{Name = "Sea Foam",   NoMatchImagePath = _noMatchImagePath, MatchImagePath = "../Assets/Category/Colors/seaFoamBlue.png" },
            };
        }
        
        public Models.SetCategoryDTO BuildEasyBoard()
        {
            return BuildBoard(LevelConstants.EasyLevel, LevelConstants.EasyLevelCardTotal);
        }

        public Models.SetCategoryDTO BuildMediumBoard()
        {
            return BuildBoard(LevelConstants.MediumLevel, LevelConstants.MediumLevelCardTotal);
        }

        public Models.SetCategoryDTO BuildHardBoard()
        {
            return BuildBoard(LevelConstants.HardLevel, LevelConstants.HardLevelCardTotal);
        }

        private SetCategoryDTO BuildBoard(string levelName, int levelCardTotal)
        {
            PossibleItems.Shuffle();
            var items = PossibleItems.Take(levelCardTotal).ToList();
            var setCategory = new SetCategoryDTO { Key = OptionsKeys.Colors, Name = CategoryConstants.ColorCategoryName, LevelName = levelName };

            foreach (var item in items)
            {
                item.SetLevelName = levelName;
            }

            setCategory.Populate(items);

            return setCategory;
        }

        public IList<SetDTO> PossibleItems { get; private set; }
    }
}
