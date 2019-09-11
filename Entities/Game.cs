using System.Collections.Generic;
using Resistance.Enums;

namespace Resistance.Entities
{
    public class Game : Key
    {
        public virtual long ChatId { get; set; }
        public virtual GameStatus Status { get; set; }
        public virtual string Missions { get; set; }
        public virtual int? CurrentMission { get; set; }

        public virtual ICollection<Player> Players { get; set; }
    }
}
