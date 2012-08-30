using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Dimesoft.Games.Memory.Domain.Common;
using Dimesoft.Games.Memory.Domain.Models;

namespace Dimesoft.Games.Memory.Domain.Factories
{
    public class AnimalFactory : ICategoryFactory
    {
        private string _noMatchImagePath = "../Assets/Category/BackOfCard.png";

        public AnimalFactory()
        {
            PossibleItems = new List<SetDTO>
            {
                new SetDTO{Name = "Ape",        NoMatchImagePath = _noMatchImagePath, MatchImagePath = "../Assets/Category/Animals/Ape.png" },
                new SetDTO{Name = "Elephant",   NoMatchImagePath = _noMatchImagePath, MatchImagePath = "../Assets/Category/Animals/Elephant.png" },
                new SetDTO{Name = "Elk",        NoMatchImagePath = _noMatchImagePath, MatchImagePath = "../Assets/Category/Animals/Elk.png" },
                new SetDTO{Name = "Gator",      NoMatchImagePath = _noMatchImagePath, MatchImagePath = "../Assets/Category/Animals/Gator.png" },
                new SetDTO{Name = "Giraffe",    NoMatchImagePath = _noMatchImagePath, MatchImagePath = "../Assets/Category/Animals/Giraffe.png" },
                new SetDTO{Name = "Hippo",      NoMatchImagePath = _noMatchImagePath, MatchImagePath = "../Assets/Category/Animals/Hippo.png" },
                new SetDTO{Name = "Lion",       NoMatchImagePath = _noMatchImagePath, MatchImagePath = "../Assets/Category/Animals/Lion.png" },
                new SetDTO{Name = "Osterage",   NoMatchImagePath = _noMatchImagePath, MatchImagePath = "../Assets/Category/Animals/Osterage.png" },
                new SetDTO{Name = "Rhino",      NoMatchImagePath = _noMatchImagePath, MatchImagePath = "../Assets/Category/Animals/Rhino.png" },
                new SetDTO{Name = "Snake",      NoMatchImagePath = _noMatchImagePath, MatchImagePath = "../Assets/Category/Animals/Snake.png" },
                new SetDTO{Name = "Tiger",      NoMatchImagePath = _noMatchImagePath, MatchImagePath = "../Assets/Category/Animals/Tiger.png" },
                new SetDTO{Name = "Zebra",      NoMatchImagePath = _noMatchImagePath, MatchImagePath = "../Assets/Category/Animals/Zebra.png" },
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
            var setCategory = new SetCategoryDTO { Key = OptionsKeys.Animals, Name = CategoryConstants.AnimalsCategoryName, LevelName = levelName };

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
