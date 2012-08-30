using System;
using System.Xml.Serialization;

namespace Dimesoft.Games.Memory.Domain.Models
{
    public class GameResultDTO
    {
        
        public string GameCategory { get; set; }

        public string GameLevel { get; set; }

        public int PlayerId { get; set; }

        public string PlayerName { get; set; }

        [XmlIgnore]
        public TimeSpan GameTime { get; set; }

        [XmlElement("GameTime")]
        public string XMLGameTime
        {
            get { return GameTime.ToString(); }
            set
            {
                if ( !string.IsNullOrEmpty(value) )
                {
                    GameTime = TimeSpan.Parse(value);    
                }
                
            }
        }

        public DateTime GameDate { get; set; }

        public int Attempts { get; set; }
    }
}
