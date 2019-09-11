using System.Data;
using Resistance.Helpers;

namespace Resistance
{
    public static class Replic
    {
        public const string PublicChatOnly = "Эта команда работает только в групповом чате";
        public const string PrivateChatOnly = "Эта команда работает только в приватном чате";
        public const string Ready = "Я включился";
        public const string FastPing = "Понг";
        public const string Ping = "Задержка {0} секунд";
        public const string ServerError = "Ошибка сервера";

        public const string GameOpened =
            "Игра началась, нажмите /join чтобы присоединиться, номер вашего чата {0}";
        public const string NoOpenGame = "В этом чате нет активной игры";
        public const string ChatIsPlaying = "Этот чат уже играет";
        public const string ChatIsWaiting = "Набор игроков уже открыт";
        public const string GameHasBegun = "Игра началась, удачного полёта";
        public const string NotEnoughPlayers = "Не достаточно игроков";

        public const string NoAnyJoins = "Вы не присоединились ни к одной игре /howtostart";
        public const string PrivateIsPlaying = "Вы уже играете";
        public const string PrivateConfirm = "Нажмите /accept чтобы присоединиться к игре {0}";
        public const string PrivateJoinSuccess = "Вы подключились к игре {0}";

        public const string PublicConfirm =
            "{0}, напишите боту @" + Rules.BotName + " команду /accept и подвердите участие";
        public const string PublicJoinSuccess = "{0} подключился к игре {1}";
        public const string PublicIsPlaying = "{0} уже играет";
        public const string PublicHasJoined = "{0} уже подключился";

        public const string YouAreRed = "";
        public const string YouAreBlue = "";
    }
}
