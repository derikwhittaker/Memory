using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Dimesoft.Games.Memory.Domain.Common;
using Dimesoft.Games.Memory.Domain.Models;

namespace Dimesoft.Games.Memory.Domain.Factories
{
    public class ShapeFactory : ICategoryFactory
    {
        private string _noMatchImagePath = "../Assets/Category/BackOfCard.png";

        public ShapeFactory()
        {
            PossibleItems = new List<SetDTO>
            {
                new SetDTO{Name = "Circle",                 NoMatchImagePath = _noMatchImagePath, MatchImagePath = "../Assets/Category/Shapes/Circle.png" },
                new SetDTO{Name = "Cresent",                NoMatchImagePath = _noMatchImagePath, MatchImagePath = "../Assets/Category/Shapes/Cresent.png" },
                new SetDTO{Name = "Curvilinear Triangle",   NoMatchImagePath = _noMatchImagePath, MatchImagePath = "../Assets/Category/Shapes/CurvilinearTriangle.png" },
                new SetDTO{Name = "Diamond",                NoMatchImagePath = _noMatchImagePath, MatchImagePath = "../Assets/Category/Shapes/Diamond.png" },
                new SetDTO{Name = "Ellipse",                NoMatchImagePath = _noMatchImagePath, MatchImagePath = "../Assets/Category/Shapes/Ellipse.png" },
                new SetDTO{Name = "Hexigon",                NoMatchImagePath = _noMatchImagePath, MatchImagePath = "../Assets/Category/Shapes/Hexigon.png" },
                new SetDTO{Name = "Octogon",                NoMatchImagePath = _noMatchImagePath, MatchImagePath = "../Assets/Category/Shapes/Octogon.png" },
                new SetDTO{Name = "Oval",                   NoMatchImagePath = _noMatchImagePath, MatchImagePath = "../Assets/Category/Shapes/Oval.png" },
                new SetDTO{Name = "Parralelogram",          NoMatchImagePath = _noMatchImagePath, MatchImagePath = "../Assets/Category/Shapes/Parralelogram.png" },
                new SetDTO{Name = "Pentagon",               NoMatchImagePath = _noMatchImagePath, MatchImagePath = "../Assets/Category/Shapes/Pentagon.png" },
                new SetDTO{Name = "Quadrefoil",             NoMatchImagePath = _noMatchImagePath, MatchImagePath = "../Assets/Category/Shapes/Quadrefoil.png" },
                new SetDTO{Name = "Rectangle",              NoMatchImagePath = _noMatchImagePath, MatchImagePath = "../Assets/Category/Shapes/Rectangle.png" },
                new SetDTO{Name = "Square",                 NoMatchImagePath = _noMatchImagePath, MatchImagePath = "../Assets/Category/Shapes/Square.png" },
                new SetDTO{Name = "Trapezoid",              NoMatchImagePath = _noMatchImagePath, MatchImagePath = "../Assets/Category/Shapes/Trapezoid.png" },
                new SetDTO{Name = "Triangle",               NoMatchImagePath = _noMatchImagePath, MatchImagePath = "../Assets/Category/Shapes/Triangle.png" },
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
            var setCategory = new SetCategoryDTO { Key = OptionsKeys.Shapes, Name = CategoryConstants.ShapesCategoryName, LevelName = levelName };

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
