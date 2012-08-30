using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Dimesoft.Games.Memory.Domain.Common;
using Dimesoft.Games.Memory.Domain.Models;

namespace Dimesoft.Games.Memory.Domain.Factories
{
    public class LetterFactory : ICategoryFactory
    {
        private string _noMatchImagePath = "../Assets/Category/BackOfCard.png";

        public LetterFactory()
        {
            PossibleItems = new List<SetDTO>
            {
                new SetDTO{Name = "A", NoMatchImagePath = _noMatchImagePath, MatchImagePath = "../Assets/Category/Letters/A.png" },
                new SetDTO{Name = "B", NoMatchImagePath = _noMatchImagePath, MatchImagePath = "../Assets/Category/Letters/B.png" },
                new SetDTO{Name = "C", NoMatchImagePath = _noMatchImagePath, MatchImagePath = "../Assets/Category/Letters/C.png" },
                new SetDTO{Name = "D", NoMatchImagePath = _noMatchImagePath, MatchImagePath = "../Assets/Category/Letters/D.png" },
                new SetDTO{Name = "E", NoMatchImagePath = _noMatchImagePath, MatchImagePath = "../Assets/Category/Letters/E.png" },
                new SetDTO{Name = "F", NoMatchImagePath = _noMatchImagePath, MatchImagePath = "../Assets/Category/Letters/F.png" },
                new SetDTO{Name = "G", NoMatchImagePath = _noMatchImagePath, MatchImagePath = "../Assets/Category/Letters/G.png" },
                new SetDTO{Name = "H", NoMatchImagePath = _noMatchImagePath, MatchImagePath = "../Assets/Category/Letters/H.png" },
                new SetDTO{Name = "I", NoMatchImagePath = _noMatchImagePath, MatchImagePath = "../Assets/Category/Letters/I.png" },
                new SetDTO{Name = "J", NoMatchImagePath = _noMatchImagePath, MatchImagePath = "../Assets/Category/Letters/J.png" },
                new SetDTO{Name = "K", NoMatchImagePath = _noMatchImagePath, MatchImagePath = "../Assets/Category/Letters/K.png" },
                new SetDTO{Name = "L", NoMatchImagePath = _noMatchImagePath, MatchImagePath = "../Assets/Category/Letters/L.png" },
                new SetDTO{Name = "M", NoMatchImagePath = _noMatchImagePath, MatchImagePath = "../Assets/Category/Letters/M.png" },
                new SetDTO{Name = "N", NoMatchImagePath = _noMatchImagePath, MatchImagePath = "../Assets/Category/Letters/N.png" },
                new SetDTO{Name = "O", NoMatchImagePath = _noMatchImagePath, MatchImagePath = "../Assets/Category/Letters/O.png" },
                new SetDTO{Name = "P", NoMatchImagePath = _noMatchImagePath, MatchImagePath = "../Assets/Category/Letters/P.png" },
                new SetDTO{Name = "Q", NoMatchImagePath = _noMatchImagePath, MatchImagePath = "../Assets/Category/Letters/Q.png" },
                new SetDTO{Name = "R", NoMatchImagePath = _noMatchImagePath, MatchImagePath = "../Assets/Category/Letters/R.png" },
                new SetDTO{Name = "S", NoMatchImagePath = _noMatchImagePath, MatchImagePath = "../Assets/Category/Letters/S.png" },
                new SetDTO{Name = "T", NoMatchImagePath = _noMatchImagePath, MatchImagePath = "../Assets/Category/Letters/T.png" },
                new SetDTO{Name = "U", NoMatchImagePath = _noMatchImagePath, MatchImagePath = "../Assets/Category/Letters/U.png" },
                new SetDTO{Name = "V", NoMatchImagePath = _noMatchImagePath, MatchImagePath = "../Assets/Category/Letters/V.png" },
                new SetDTO{Name = "W", NoMatchImagePath = _noMatchImagePath, MatchImagePath = "../Assets/Category/Letters/W.png" },
                new SetDTO{Name = "X", NoMatchImagePath = _noMatchImagePath, MatchImagePath = "../Assets/Category/Letters/X.png" },
                new SetDTO{Name = "Y", NoMatchImagePath = _noMatchImagePath, MatchImagePath = "../Assets/Category/Letters/Y.png" },
                new SetDTO{Name = "Z", NoMatchImagePath = _noMatchImagePath, MatchImagePath = "../Assets/Category/Letters/Z.png" },
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
            var setCategory = new SetCategoryDTO { Key = OptionsKeys.Letters, Name = CategoryConstants.LetterCategoryName, LevelName = levelName };

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
