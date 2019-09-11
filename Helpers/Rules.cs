namespace Resistance.Helpers
{
    public class Rules
    {
        public const string BotName = "ResistanceGame_Bot";
        public static int GetRedPlayerCount(int playerCount) => (playerCount - 1) / 2;
        public const int MinPlayerCount = 1; // 5-10
    }
}
