using System.Collections.Generic;
using System.Linq;
using Resistance.Entities;
using Resistance.Enums;
using Resistance.Helpers;

namespace Resistance.Database
{
    public class Repository
    {
        public Game GetGame(int id)
        {
            using (var db = new GameContext())
            {
                return db.Games.FirstOrDefault(x => x.Id == id);
            }
        }

        public Game GetChatGame(long chatId)
        {
            using (var db = new GameContext())
            {
                return db.Games.FirstOrDefault(x => x.ChatId == chatId);
            }
        }

        public void SaveGame(Game game)
        {
            using (var db = new GameContext())
            {
                db.InsertOrUpdate(game);
            }
        }

        public Player GetPlayer(long id)
        {
            using (var db = new GameContext())
            {
                return db.Players.FirstOrDefault(x => x.TelegramId == id);
            }
        }
        public Player GetGameCaptain(long gameId)
        {
            using (var db = new GameContext())
            {
                return db.Players.SingleOrDefault(x=>x.IsLeader && x.GameId == gameId);
            }
        }

        public List<Player> GetGamePlayers(long gameId)
        {
            using (var db = new GameContext())
            {
                return db.Players.Where(x => x.GameId == gameId && x.HasAccepted).ToList();
            }
        }

        public void SavePlayer(Player player)
        {
            using (var db = new GameContext())
            {
                db.InsertOrUpdate(player);
            }
        }

        public List<Mission> GetGameMissions(long gameId)
        {
            using (var db = new GameContext())
            {
                return db.Missions.Where(x => x.GameId == gameId).ToList();
            }
        }

        public void SaveMission(int num, int gameId, MissionStatus status)
        {
            using (var db = new GameContext())
            {
                var mission = db.Missions.SingleOrDefault(x => x.GameId == gameId && x.Num == num) 
                              ?? new Mission { GameId = gameId, Num = num };
                mission.Status = status;
                db.InsertOrUpdate(mission);
            }
        }
    }
}
