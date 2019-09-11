using Resistance.Enums;

namespace Resistance.Entities
{
    public class Player : Key
    {
        public virtual Game Game { get; set; }
        public virtual int? GameId { get; set; }
        public virtual long TelegramId { get; set; }
        public virtual Role Role { get; set; }
        public virtual string Name { get; set; }
        public virtual bool IsLeader { get; set; }
        public virtual bool HasAccepted { get; set; }
    }
}
