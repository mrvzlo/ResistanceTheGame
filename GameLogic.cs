using System.Collections.Generic;
using System.Linq;
using Resistance.Entities;
using Resistance.Enums;
using Resistance.Helpers;
using Resistance.Models;
using Telegram.Bot.Types;
using Game = Resistance.Entities.Game;

namespace Resistance.Database
{
    public class GameLogic
    {
        private readonly Repository Repository;
        public GameLogic() => Repository = new Repository();

        #region Before game start

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
            var captainId = RandomHelper.RandNum(players.Count);
            for (var i = 0; i < players.Count; i++)
            {
                players[i].Role = roles[i];
                players[i].IsLeader = i == captainId;
                Repository.SavePlayer(players[i]);
                playerRoles.Add(new PlayerRole(players[i]));
            }

            for (var i = 0; i < 5; i++)
                Repository.SaveMission(i, game.Id, MissionStatus.NotStarted);

            return Response.StartStatus.Success;
        }

        #endregion

        #region Missions

        public bool GetChatMissions(long chatId, bool capOnly, out List<MissionViewModel> missions, long? capTgId = null)
        {
            missions = null;
            var game = Repository.GetChatGame(chatId);
            if (game == null || game.Status == GameStatus.Over || game.Status == GameStatus.Over)
                return false;

            if (capOnly)
            {
                var cap = Repository.GetGameCaptain(game.Id);
                if (cap.TelegramId != capTgId) return false;
            }

            var playerCount = Repository.GetGamePlayers(game.Id).Count;
            missions = Repository.GetGameMissions(game.Id).Select(x => new MissionViewModel(x, playerCount)).ToList();
            return true;
        }

        public Response.SelectionStatus CanSelectMission(long capId)
        {
            var player = Repository.GetPlayer(capId);
            if (player.GameId == null)
                return Response.SelectionStatus.YouAreNotPlaying;
            if (!player.IsLeader)
                return Response.SelectionStatus.YouAreNotCaptain;
            var game = Repository.GetGame(player.GameId.Value);
            if (game == null)
                return Response.SelectionStatus.Error;
            if (game.Status != GameStatus.MissionSelection)
                return Response.SelectionStatus.CantSelectNow;
            return Response.SelectionStatus.Success;
        }

        #endregion
        //todo start mission
        //todo vote
        //todo add member

    }
}
