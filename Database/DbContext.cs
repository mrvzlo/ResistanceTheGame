using System.Data.Entity;
using Resistance.Entities;

namespace Resistance.Database
{
    public class GameContext : DbContext
    {
        public GameContext(): base("GameContext") { }
        public DbSet<Game> Games { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Mission> Missions { get; set; }
    }
}