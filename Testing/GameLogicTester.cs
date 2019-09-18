using System.Linq;
using Resistance.Enums;
using Resistance.Helpers;
using Xunit;

namespace Resistance.Testing
{
    public class GameLogicTester
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

        [Theory]
        [InlineData(5, 0, 2), InlineData(5, 1, 3), InlineData(5, 2, 2), 
         InlineData(5, 3, 3), InlineData(5, 4, 3)]

        [InlineData(6, 0, 2), InlineData(6, 1, 3), InlineData(6, 2, 3),
         InlineData(6, 3, 4), InlineData(6, 4, 4)]

        [InlineData(7, 0, 2), InlineData(7, 1, 3), InlineData(7, 2, 3),
         InlineData(7, 3, 4), InlineData(7, 4, 4)]

        [InlineData(8, 0, 3), InlineData(8, 1, 4), InlineData(8, 2, 4),
         InlineData(8, 3, 5), InlineData(8, 4, 5)]

        [InlineData(9, 0, 3), InlineData(9, 1, 4), InlineData(9, 2, 4),
         InlineData(9, 3, 5), InlineData(9, 4, 5)]

        [InlineData(10, 0, 3), InlineData(10, 1, 4), InlineData(10, 2, 4),
         InlineData(10, 3, 5), InlineData(10, 4, 5)]

        public void ValidateMissionPlayerCount(int playerCount, int missionNum, int expected)
        {
            var act = Rules.GetMissionPlayerCount(playerCount, missionNum);
            Assert.Equal(expected, act);
        }
    }
}
