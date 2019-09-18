using Resistance.Entities;
using Resistance.Enums;

namespace Resistance.Models
{
    public class PlayerRole
    {
        public long TgId { get; }
        public Role Role{ get; }
        public bool IsCaptain { get; }
        public string Username { get; }

        public PlayerRole(Player player)
        {
            TgId = player.TelegramId;
            Username = player.Name;
            Role = player.Role;
            IsCaptain = player.IsLeader;
        }
    }
}
