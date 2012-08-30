using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dimesoft.Games.Memory.Domain.Models;

namespace Dimesoft.Games.Memory.Domain.Factories
{
    public interface IOptionsFactory
    {
        IList<DashboardOptionDTO> DashboardOptions();
        ICategoryFactory SelectedCategory(string optionsKey);
        string SecondaryTileForCategory(string categoryKey);
    }

    public class OptionsFactory : IOptionsFactory
    {

        public IList<DashboardOptionDTO> DashboardOptions()
        {
            var matchingAnimalsOption = new DashboardOptionDTO(0, OptionsKeys.Animals, "Animals", "Group Subtitle: 1", "../Assets/Category/AnimalsHomeTile.png");
            var matchingColorsOption = new DashboardOptionDTO(1, OptionsKeys.Colors, "Colors", "Group Subtitle: 1", "../Assets/Category/ColorsHomeTile.png");
            var matchingShapesOption = new DashboardOptionDTO(2, OptionsKeys.Shapes, "Shapes", "Group Subtitle: 1", "../Assets/Category/ShapesHomeTile.png");
            var matchingVehicalsOption = new DashboardOptionDTO(3, OptionsKeys.Vehicle, "Vehicles", "Group Subtitle: 1", "Assests/Category/Colors/white.png");
            var matchingSignsOption = new DashboardOptionDTO(4, OptionsKeys.Signs, "Signs", "Group Subtitle: 1", "Assets/DarkGray.png");
            var matchingLettersOption = new DashboardOptionDTO(5, OptionsKeys.Letters, "Letters", "Group Subtitle: 1", "../Assets/Category/LettersHomeTile.png");
            var surpriseMeOption = new DashboardOptionDTO(6, OptionsKeys.SurpriseMe, "Surprise Me", "Group Subtitle: 1", "Assets/DarkGray.png");

            return new List<DashboardOptionDTO>
                        {
                            matchingAnimalsOption,
                            matchingColorsOption,
                            matchingLettersOption,
                            matchingShapesOption,
                            //matchingSignsOption,
                            //matchingVehicalsOption,
                            //surpriseMeOption
                        };
        }

        public ICategoryFactory SelectedCategory(string optionsKey)
        {
            switch (optionsKey)
            {
                case OptionsKeys.Animals:
                    return new AnimalFactory();

                case OptionsKeys.Colors:
                    return new ColorFactory();

                case OptionsKeys.Shapes:
                    return new ShapeFactory();

                case OptionsKeys.Vehicle:
                    return new ColorFactory();

                case OptionsKeys.Signs:
                    return new ColorFactory();

                case OptionsKeys.Letters:
                    return new LetterFactory();
                    
                default:
                    return new ColorFactory();
            }

            
        }

        public string SecondaryTileForCategory(string categoryKey)
        {
            switch (categoryKey)
            {
                case OptionsKeys.Animals:
                    return "Assets/AnimalsSecondaryTile.png";

                case OptionsKeys.Colors:
                    return "Assets/ColorsSecondaryTile.png";

                case OptionsKeys.Shapes:
                    return "Assets/ShapesSecondaryTile.png";

                case OptionsKeys.Vehicle:
                    return "Assets/ShapesSecondaryTile.png";

                case OptionsKeys.Signs:
                    return "Assets/ShapesSecondaryTile.png";

                case OptionsKeys.Letters:
                    return "Assets/LettersSecondaryTile.png";

                default:
                    return "Assets/150x150PinTile.png";
            }
        }

    }
}
