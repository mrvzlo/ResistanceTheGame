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
        private static readonly GameLogic Core = new GameLogic();

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
            var groupChat = chat < 0;
            Core.CheckPlayer(from, username);

            if (message.Equals("/ping", StringComparison.OrdinalIgnoreCase))
                return Say(Ping(dateTime), chat);
            if (StringHelper.EqualsAny(message, Command.Start.ToCommand()))
                return Say(groupChat ? Replic.PrivateChatOnly : ShowUnacceptedJoin(chat), chat);
            if (StringHelper.EqualsAny(message, Command.Accept.ToCommand()))
                return Say(groupChat ? Replic.PrivateChatOnly : AcceptJoin(chat, username), chat);
            if (StringHelper.EqualsAny(message, Command.Rules.ToCommand()))
                return Say(SendBigMessage(BigMessageType.Rules), chat);
            if (StringHelper.EqualsAny(message, Command.HowToStart.ToCommand()))
                return Say(SendBigMessage(BigMessageType.HowToStart), chat);

            if (StringHelper.EqualsAny(message, Command.Resistance.ToCommand()))
                return Say(!groupChat ? Replic.PublicChatOnly : OpenGame(chat), chat);
            if (StringHelper.EqualsAny(message, Command.Join.ToCommand()))
                return Say(!groupChat ? Replic.PublicChatOnly : JoinGame(chat, from, username), chat);
            if (StringHelper.EqualsAny(message, Command.ForceStart.ToCommand()))
                return Say(!groupChat ? Replic.PublicChatOnly : StartGame(chat), chat);
            if (StringHelper.EqualsAny(message, Command.Missions.ToCommand()))
                return Say(!groupChat ? Replic.PublicChatOnly : AvailableMissions(chat), chat);

            return TaskStatus.Faulted;
        }

        private static TaskStatus Say(string s, long id)
        {
            if (string.IsNullOrWhiteSpace(s)) return TaskStatus.Canceled;
            var result = MyBot.SendTextMessageAsync(id, s);
            return result.Status;
        }

        static string Ping(DateTime messageDateTime)
        {
            var secs = DateTime.UtcNow.Subtract(messageDateTime).TotalSeconds;
            return secs < 0.5 ? Replic.FastPing : string.Format(Replic.Ping, secs);
        }

        static string SendBigMessage(BigMessageType bigMessageType) =>
            File.ReadAllText($"{bigMessageType.ToString()}.txt");


        #region Before the game

        private static string OpenGame(long chatId)
        {
            var gameNum = $"№{-chatId:X}";
            var result = Core.OpenGame(chatId);
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
            var result = Core.JoinGame(chatId, userId, username);
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
            var result = Core.ShowGameToJoin(userId, out var chatId);
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
            var result = Core.AcceptJoin(userId, username, out var chatId);
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
            var result = Core.StartGame(chatId, out var players);
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
                    msg += Replic.YouAreCaptain;

                Say(msg, player.TgId);
            }

            var captainName = players.Single(x => x.IsCaptain).Username;
            return $"{string.Format(result.GetDescription(), captainName)}\n{AvailableMissions(chatId)}";
        }

        #endregion

        #region Missions

        private static string AvailableMissions(long chatId)
        {
            var result = Core.GetChatMissions(chatId, out var missions);
            if (!result) return Replic.NoOpenGame;
            return Replic.YourMissions + string.Join("\n", missions.Select(x => x.ToString()));
        }

        #endregion
    }
}
