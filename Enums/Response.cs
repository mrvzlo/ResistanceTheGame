using System.ComponentModel;

namespace Resistance.Enums
{
    public class Response
    {
        public enum OpeningStatus
        {
            [Description(Replic.ServerError)] Error,
            [Description(Replic.GameOpened)] Success,
            [Description(Replic.ChatIsPlaying)] AlreadyPlaying,
            [Description(Replic.ChatIsWaiting)] AlreadyOpen
        }

        public enum JoiningStatus
        {
            [Description(Replic.ServerError)] Error,
            [Description(Replic.PublicJoinSuccess)] JoinSuccess,
            [Description(Replic.PublicConfirm)] PublicConfirm,
            [Description(Replic.NoOpenGame)] GameNotFound,
            [Description(Replic.PublicIsPlaying)] PlayerAlreadyPlaying,
            [Description(Replic.ChatIsPlaying)] ChatAlreadyPlaying,
            [Description(Replic.PublicHasJoined)] PlayerAlreadyJoined,
            //Private only
            [Description(Replic.PrivateConfirm)] PrivateConfirm,
            [Description(Replic.PrivateJoinSuccess)] ConfirmationSuccess,
            [Description(Replic.PrivateIsPlaying)] YouAlreadyJoined,
            [Description(Replic.NoAnyJoins)] NoAnyJoins,
        }

        public enum StartStatus
        {
            [Description(Replic.ServerError)] Error,
            [Description(Replic.GameHasBegun)] Success,
            [Description(Replic.NoOpenGame)] GameNotFound,
            [Description(Replic.NotEnoughPlayers)] NotEnoughPlayers
        }
    }
}
