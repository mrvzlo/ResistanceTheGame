using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Resistance.Database;
using Resistance.Enums;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace Resistance
{
    class Program
    {
        #region Start point

        private static readonly TelegramBotClient MyBot;
        private static int _lastMsgId;
        private static long _admin;
        private static readonly GameLogic GameCore = new GameLogic();
        private static readonly TelegramLogic TgCore = new TelegramLogic();

        static Program()
        {
            var settings = File.ReadAllLines("settings.txt");
            var token = settings[0];
            MyBot = new TelegramBotClient(token);
            _admin = Convert.ToInt64(settings[1]);
        }

        static void Main(string[] args)
        {
            MyBot.OnMessage += GetMessage;
            MyBot.StartReceiving();
            Say(Replic.Ready, _admin);
            Console.ReadLine();
            MyBot.StopReceiving();
        }

        private static void GetMessage(object sender, MessageEventArgs e)
        {
            var msg = e.Message;
            if (msg == null) return;
            if (_lastMsgId == msg.MessageId) return;
            if (msg.Type != Telegram.Bot.Types.Enums.MessageType.Text) return;
            var fullName = $"{msg.From.FirstName} {msg.From.LastName}".Trim();
            var username = string.IsNullOrWhiteSpace(fullName)
                ? $"@{msg.From.Username}" : fullName;
            Interpretator(msg.Text, msg.Chat.Id, msg.From.Id, username, msg.Date.ToUniversalTime());
            _lastMsgId = msg.MessageId;
        }

        #endregion

        private static TaskStatus Interpretator(string message, long chat, long from, string username, DateTime dateTime)
        {
            var publicChat = chat < 0;
            TgCore.CheckPlayer(from, username);
            var command = TgCore.GetCommandFromString(message);
            if (command.GetCommandType() == CommandType.PrivateOnly && publicChat)
                return Say(Replic.PrivateChatOnly, chat);
            if (command.GetCommandType() == CommandType.PublicOnly && !publicChat)
                return Say(Replic.PublicChatOnly, chat);

            var reply = string.Empty;
            switch (command)
            {
                case Command.Ping:
                    reply = TgCore.Ping(dateTime);
                    break;
                case Command.Start:
                    reply = ShowUnacceptedJoin(chat);
                    break;
                case Command.HowToStart:
                    reply = TgCore.SendBigMessage(Command.HowToStart);
                    break;
                case Command.Join:
                    reply = JoinGame(chat, from, username);
                    break;
                case Command.Accept:
                    reply = AcceptJoin(chat, username);
                    break;
                case Command.Rules:
                    reply = TgCore.SendBigMessage(Command.Rules);
                    break;
                case Command.Resistance:
                    reply = OpenGame(chat);
                    break;
                case Command.ForceStart:
                    reply = StartGame(chat);
                    break;
                case Command.Missions:
                    reply = AvailableMissions(chat);
                    break;
                case Command.Choose:
                    break;
                default:
                    reply = Replic.CommandNotFound;
                    break;
            }

            Say(reply, chat);
            return TaskStatus.Faulted;
        }

        private static TaskStatus Say(string s, long id)
        {
            if (string.IsNullOrWhiteSpace(s)) return TaskStatus.Canceled;
            var result = MyBot.SendTextMessageAsync(id, s);
            return result.Status;
        }

        #region Before the game

        private static string OpenGame(long chatId)
        {
            var gameNum = $"№{-chatId:X}";
            var result = GameCore.OpenGame(chatId);
            switch (result)
            {
                case Response.OpeningStatus.Success:
                    return string.Format(result.GetDescription(), gameNum);
                default:
                    return result.GetDescription();
            }
        }

        private static string JoinGame(long chatId, long userId, string username)
        {
            var result = GameCore.JoinGame(chatId, userId, username);
            switch (result)
            {
                case Response.JoiningStatus.PublicConfirm:
                case Response.JoiningStatus.PlayerAlreadyPlaying:
                case Response.JoiningStatus.PlayerAlreadyJoined:
                    return string.Format(result.GetDescription(), username);
                default:
                    return result.GetDescription();
            }
        }

        private static string ShowUnacceptedJoin(long userId)
        {
            var result = GameCore.ShowGameToJoin(userId, out var chatId);
            var gameNum = $"№{-chatId:X}";
            switch (result)
            {
                case Response.JoiningStatus.PrivateConfirm:
                    return string.Format(result.GetDescription(), gameNum);
                default:
                    return result.GetDescription();
            }
        }

        private static string AcceptJoin(long userId, string username)
        {
            var result = GameCore.AcceptJoin(userId, username, out var chatId);
            var gameNum = $"№{-chatId:X}";
            switch (result)
            {
                case Response.JoiningStatus.ConfirmationSuccess:
                    Say(string.Format(Replic.PublicJoinSuccess, username, gameNum), chatId);
                    return string.Format(result.GetDescription(), gameNum);
                case Response.JoiningStatus.PlayerAlreadyPlaying:
                case Response.JoiningStatus.PlayerAlreadyJoined:
                    return string.Format(result.GetDescription(), username);
                default:
                    return result.GetDescription();
            }
        }

        private static string StartGame(long chatId)
        {
            var result = GameCore.StartGame(chatId, out var players);
            if (result != Response.StartStatus.Success)
                return result.GetDescription();

            var redTeam = players.Where(x => x.Role == Role.Red).Select(x => x.Username).ToList();
            foreach (var player in players)
            {
                var msg = player.Role == Role.Red ? Replic.YouAreRed : Replic.YouAreBlue;
                if (player.Role == Role.Red)
                {
                    var otherPlayers = redTeam.Where(x => !string.Equals(x, player.Username, StringComparison.Ordinal));
                    msg += string.Format(Replic.OtherRedPlayers, string.Join(", ", otherPlayers));
                }

                if (player.IsCaptain)
                    msg += $"{Replic.YouAreCaptain}\n{AvailableMissionCommands(chatId, player.TgId)}";

                Say(msg, player.TgId);
            }

            var captainName = players.Single(x => x.IsCaptain).Username;
            return $"{string.Format(result.GetDescription(), captainName)}\n{AvailableMissions(chatId)}";
        }

        #endregion

        #region Missions

        private static string AvailableMissions(long chatId)
        {
            var result = GameCore.GetChatMissions(chatId, false, out var missions);
            if (!result) return Replic.NoOpenGame;
            return Replic.YourMissions + string.Join("\n", missions.Select(x => x.ToString()));
        }

        private static string AvailableMissionCommands(long chatId, long capId)
        {
            var result = GameCore.GetChatMissions(chatId, true, out var missions, capId);
            if (!result) return null;
            return Replic.AvailableMissions + string.Join("\n", missions.Where(x => x.IsAvailable)
                       .Select(x => $"{Command.Choose.ToCommand()}_{x.Num + 1}"));
        }

        #endregion
    }
}
