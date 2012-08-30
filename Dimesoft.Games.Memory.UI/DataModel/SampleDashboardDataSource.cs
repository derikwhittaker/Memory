using System.Collections.Generic;
using System.Collections.ObjectModel;
using Dimesoft.Games.Memory.Domain;
using Dimesoft.Games.Memory.Domain.Models;

namespace Dimesoft.Games.Memory.DataModel
{
    public class SampleDashboardDataSource 
    {

        public SampleDashboardDataSource()
        {

            Users = new List<UserDTO>
                       {
                           new UserDTO{Id = 1, UserName = "Guest"},
                           new UserDTO{Id = 2, UserName = "Derik"},
                           new UserDTO{Id = 3, UserName = "Brendan"}
                       };

            var dashboardGroups = new List<DashboardOptionDTO>
                                      {
                                          new DashboardOptionDTO(0, OptionsKeys.Animals, "Animals", "Group Subtitle: 1", "../Assets/Category/AnimalsHomeTile.png"),
                                          new DashboardOptionDTO(1, OptionsKeys.Colors, "Colors", "Group Subtitle: 1", "../Assets/Category/ColorsHomeTile.png"),
                                          new DashboardOptionDTO(2, OptionsKeys.Shapes, "Shapes", "Group Subtitle: 1", "../Assets/Category/ShapesHomeTile.png"),
                                          new DashboardOptionDTO(5, OptionsKeys.Letters, "Letters", "Group Subtitle: 1", "../Assets/Category/LettersHomeTile.png")
                                      };

            _itemGroups = new ObservableCollection<DashboardOptionDTO>(dashboardGroups);
        }

        private IList<UserDTO> _user;
        public IList<UserDTO> Users
        {
            get { return _user; }
            set { _user = value; }
        }


        private ObservableCollection<DashboardOptionDTO> _itemGroups = new ObservableCollection<DashboardOptionDTO>();
        public ObservableCollection<DashboardOptionDTO> DashboardGroups
        {
            get { return _itemGroups; }
        }
    }
}
