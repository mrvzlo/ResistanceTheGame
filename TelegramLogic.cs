using System;
using System.IO;
using Resistance.Database;
using Resistance.Entities;
using Resistance.Enums;

namespace Resistance
{
    public class TelegramLogic
    {
        private readonly Repository Repository;
        public TelegramLogic() => Repository = new Repository();

        public void CheckPlayer(long userId, string username)
        {
            var player = Repository.GetPlayer(userId);
            if (player == null)
                player = new Player { TelegramId = userId, Name = username, GameId = null };
            else if (player.Name != username)
                player.Name = username;
            else return;

            Repository.SavePlayer(player);
        }

        public string Ping(DateTime messageDateTime)
        {
            var secs = DateTime.UtcNow.Subtract(messageDateTime).TotalSeconds;
            return secs < 0.5 ? Replic.FastPing : string.Format(Replic.Ping, secs);
        }

        public string SendBigMessage(Command bigMessageType) =>
            File.ReadAllText($"{bigMessageType.ToString()}.txt");

        public Command? GetCommandFromString(string message)
        {
            foreach (Command command in Enum.GetValues(typeof(Command)))
            {
                if (StringHelper.EqualsAny(message, command.ToCommand()))
                    return command;

                if (StringHelper.BeginsWithAny(message, command.ToCommand()))
                    return command;
            }

            return null;
        }
    }
}
