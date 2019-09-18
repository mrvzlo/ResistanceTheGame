using System;
using Xunit.Sdk;

namespace Resistance.Helpers
{
    public class Rules
    {
        public const string BotName = "ResistanceGame_Bot";
        public const int MinPlayerCount = 1; // 5-10

        public static int GetRedPlayerCount(int playerCount) => (playerCount + 2) / 3;

        public static int GetMissionPlayerCount(int playerCount, int missionNum)
        {
            var count = (missionNum + 1) / 2 + (playerCount + 1) / 3;
            if (playerCount == 5 && missionNum > 1) count--;
            return count;
        }

        public static bool MissionIsEasy(int totalPlayerCount, int missionNum) => 
            missionNum == 4 && totalPlayerCount > 6;
    }
}
