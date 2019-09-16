using System.Linq;
using Resistance.Enums;
using Resistance.Helpers;
using Xunit;

namespace Resistance.Testing
{
    public class TeamSplitTester
    {
        [Fact]
        public void ValidateTeamSplit()
        {
            for (var i = 0; i < 1000; i++)
                for (var playerCount = 5; playerCount <= 10; playerCount++)
                {
                    var roles = RandomHelper.GetPlayerRoles(playerCount);
                    var expected = Rules.GetRedPlayerCount(playerCount);
                    var act = roles.Count(x => x == Role.Red);
                    Assert.Equal(expected, act);
                }
        }
    }
}
