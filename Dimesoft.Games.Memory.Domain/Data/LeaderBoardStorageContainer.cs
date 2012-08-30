using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dimesoft.Games.Memory.Domain.Models;

namespace Dimesoft.Games.Memory.Domain.Data
{
    public class LeaderBoardStorageContainer
    {

        private List<GameResultDTO> _results = new List<GameResultDTO>();
        public List<GameResultDTO> Results
        {
            get 
            {                 
                return _results ?? new List<GameResultDTO>(); 
            }
            set { _results = value; }
        }

    }

    public class UserStorageContainer
    {
        private List<UserDTO> _users =new List<UserDTO>();
        public List<UserDTO> Users
        {
            get { return _users ?? new List<UserDTO>(); }
            set { _users = value; }
        } 
    }

}
