using System.ComponentModel;
using Resistance.Helpers.Attributes;

namespace Resistance.Enums
{
    public enum Command
    {
        [Publicity(CommandType.Any)] Ping,
        [Publicity(CommandType.PrivateOnly)] Start,
        [Publicity(CommandType.Any)] HowToStart,
        [Publicity(CommandType.PublicOnly)] Join,
        [Publicity(CommandType.PrivateOnly)] Accept,
        [Publicity(CommandType.Any)] Rules,
        [Publicity(CommandType.PublicOnly)] Resistance,
        [Publicity(CommandType.PublicOnly)] ForceStart,
        [Publicity(CommandType.PublicOnly)] Missions,
        [Publicity(CommandType.PrivateOnly)] Choose
    }
}
