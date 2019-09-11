using System.Collections.Generic;
using System.Linq;
using Resistance.Entities;
using Resistance.Enums;
using Resistance.Helpers;
using Resistance.Models;

namespace Resistance.Database
{
    public class GameLogic
    {
        private readonly Repository Repository;
        public GameLogic() => Repository = new Repository();

        public void CheckPlayer(long userId, string username)
        {
            var player = Repository.GetPlayer(userId);
            if (player == null)
                player = new Player {TelegramId = userId, Name = username, GameId = null};
            else if (player.Name != username)
                player.Name = username;
            else return;

            Repository.SavePlayer(player);
        }

        public Response.OpeningStatus OpenGame(long chatId)
        {
            var game = Repository.GetChatGame(chatId);
            if (game != null && game.Status != GameStatus.Over)
            {
                return game.Status == GameStatus.WaitingForPlayers
                    ? Response.OpeningStatus.AlreadyOpen
                    : Response.OpeningStatus.AlreadyPlaying;
            }

            if (game == null)
                game = new Game { ChatId = chatId };
            game.Status = GameStatus.WaitingForPlayers;

            Repository.SaveGame(game);
            return Response.OpeningStatus.Success;
        }

        public Response.JoiningStatus JoinGame(long chatId, long userId, string username)
        {
            var player = Repository.GetPlayer(userId);
            if (player == null) return Response.JoiningStatus.Error;
            //get player game
            if (player.GameId != null)
            {
                var playerGame = Repository.GetGame(player.GameId.Value);
                if (playerGame != null && playerGame.Status != GameStatus.Over)
                    return playerGame.ChatId != chatId
                        ? Response.JoiningStatus.PlayerAlreadyJoined
                        : Response.JoiningStatus.PlayerAlreadyPlaying;
            }

            var game = Repository.GetChatGame(chatId);
            if (game == null)
                return Response.JoiningStatus.GameNotFound;
            if (game.Status == GameStatus.Over)
                return Response.JoiningStatus.ChatAlreadyPlaying;

            if (player.Name != username)
                player.Name = username;
            player.HasAccepted = false;
            player.GameId = game.Id;

            Repository.SavePlayer(player);
            return Response.JoiningStatus.PublicConfirm;
        }

        public Response.JoiningStatus ShowGameToJoin(long userId, out long chatId)
        {
            chatId = 0;
            var player = Repository.GetPlayer(userId);
            if (player == null)
                return Response.JoiningStatus.Error;
            if (player.GameId == null)
                return Response.JoiningStatus.NoAnyJoins;
            if (player.HasAccepted)
                return Response.JoiningStatus.YouAlreadyJoined;
            var game = Repository.GetGame(player.GameId.Value);
            if (game == null || game.Status == GameStatus.Over)
                return Response.JoiningStatus.GameNotFound;
            chatId = game.ChatId;
            return Response.JoiningStatus.PrivateConfirm;
        }

        public Response.JoiningStatus AcceptJoin(long userId, string username, out long chatId)
        {
            chatId = 0;
            var player = Repository.GetPlayer(userId);
            if (player == null)
                return Response.JoiningStatus.Error;
            if (player.GameId == null)
                return Response.JoiningStatus.NoAnyJoins;
            if (player.HasAccepted)
                return Response.JoiningStatus.PlayerAlreadyJoined;

            var game = Repository.GetGame(player.GameId.Value);
            if (game == null)
                return Response.JoiningStatus.GameNotFound;
            if (game.Status == GameStatus.Over)
                return Response.JoiningStatus.ChatAlreadyPlaying;

            player.HasAccepted = true;
            chatId = game.ChatId;
            Repository.SavePlayer(player);
            return Response.JoiningStatus.ConfirmationSuccess;
        }

        public Response.StartStatus StartGame(long chatId, out List<PlayerRole> playerRoles)
        {
            playerRoles = new List<PlayerRole>();
            var game = Repository.GetChatGame(chatId);
            if (game == null || game.Status == GameStatus.Over)
                return Response.StartStatus.GameNotFound;

            var players = Repository.GetGamePlayers(game.Id);
            if (players.Count < Rules.MinPlayerCount)
                return Response.StartStatus.NotEnoughPlayers;

            game.Status = GameStatus.MissionSelection;
            Repository.SaveGame(game);
            var roles = RandomHelper.GetPlayerRoles(players.Count);
            for (var i = 0; i < players.Count; i++)
                players[i].Role = roles[i];

            return Response.StartStatus.Success;
        }

        //todo start mission
        //todo vote
        //todo add member

    }
}
