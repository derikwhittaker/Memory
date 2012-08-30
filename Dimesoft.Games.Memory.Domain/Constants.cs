using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dimesoft.Games.Memory.Domain
{
    public class CategoryConstants
    {
        public const string ColorCategoryName = "Colors";
        public const string LetterCategoryName = "Letters";
        public const string ShapesCategoryName = "Shapes";
        public const string AnimalsCategoryName = "Animals";
    }

    public class LevelConstants
    {
        public const int EasyLevelCardTotal = 6;
        public const int MediumLevelCardTotal = 8;
        public const int HardLevelCardTotal = 12;

        public const string EasyLevel = "Easy";
        public const string MediumLevel = "Medium";
        public const string HardLevel = "Hard";
    }

    public class OptionsKeys
    {
        public const string Animals = "Animals";
        public const string Colors = "Colors";
        public const string Shapes = "Shapes";
        public const string Vehicle = "MatchingVehicle";
        public const string Signs = "MatchingSigns";
        public const string Letters = "Letters";
        public const string SurpriseMe = "MatchingSurpriseMe";

    }
}
