﻿using Resistance.Helpers;

namespace Resistance
{
    public static class Replic
    {
        #region General
        public const string PublicChatOnly = "Эта команда работает только в групповом чате";
        public const string PrivateChatOnly = "Эта команда работает только в приватном чате";
        public const string Ready = "Я включился";
        public const string FastPing = "Понг";
        public const string Ping = "Задержка {0} секунд";
        public const string ServerError = "Ошибка сервера";
        public const string CommandNotFound = "Неизвестная команда";
        #endregion

        #region StartGame
        public const string GameOpened =
            "Игра началась, нажмите /join чтобы присоединиться, номер вашего чата {0}";
        public const string NoOpenGame = "В этом чате нет активной игры";
        public const string ChatIsPlaying = "Этот чат уже играет";
        public const string ChatIsWaiting = "Набор игроков уже открыт";
        public const string GameHasBegun = "Игра началась, удачного полёта!\nВаш капитан - {0}";
        public const string NotEnoughPlayers = "Не достаточно игроков";
        #endregion

        #region JoinGame
        public const string NoAnyJoins = "Вы не присоединились ни к одной игре /howtostart";
        public const string PrivateIsPlaying = "Вы уже играете";
        public const string PrivateConfirm = "Нажмите /accept чтобы присоединиться к игре {0}";
        public const string PrivateJoinSuccess = "Вы подключились к игре {0}";

        public const string PublicConfirm =
            "{0}, напишите боту @" + Rules.BotName + " команду /accept и подвердите участие";
        public const string PublicJoinSuccess = "{0} подключился к игре {1}";
        public const string PublicIsPlaying = "{0} уже играет";
        public const string PublicHasJoined = "{0} уже подключился";
        #endregion

        #region Roles
        public const string OtherRedPlayers = "Остальные шпионы: {0}\n\n";
        public const string YouAreCaptain = "Вы являетесь капитаном, тщательно подбирайте экипаж будущих миссий.";
        public const string YouAreRed = "Вы являетесь шпионом. Ваша задача - втереться в доверие к капитану и " +
                                        "сорвать 3 миссии, не выдав при этом себя, или же заставить сопротивление " +
                                        "сместить 5 капитанов.\n\n";
        public const string YouAreBlue = "Вы являетесь членом сопротивления. Ваша задача - вычислить шпионов, " +
                                         "заполучить доверие вашего капитана и успешно выполнить 3 миссии. " +
                                         "Но будьте бдительны, ваш капитан сам может оказаться шпионом.\n\n";
        #endregion

        #region Missions

        public const string YourMissions = "Ваши миссии:\n";
        public const string MissionDescription = "Миссия {0} для {1} человек {2}- {3}";
        public const string Easy = "(упрощённая) ";
        public const string AvailableMissions = "Доступные миссии:\n";
        public const string MissionIsAvailable = "не начата";
        public const string MissionStarted = "начата";
        public const string MissionFailed = "провалена";
        public const string MissionSucceeded = "выполнена";

        #endregion
    }
}
