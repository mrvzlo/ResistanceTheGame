using Resistance.Enums;

namespace Resistance.Entities
{
    public class Mission : Key
    {
        public virtual int GameId { get; set; }
        public virtual int Num { get; set; }
        public virtual MissionStatus Status { get; set; }
    }
}
