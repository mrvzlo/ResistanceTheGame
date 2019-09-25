using Resistance.Entities;
using Resistance.Enums;
using Resistance.Helpers;

namespace Resistance.Models
{
    public class MissionViewModel : Key
    {
        private int PlayerCount { get; }
        public int Num { get; }
        private MissionStatus Status { get; }
        private bool IsEasy { get; }

        public bool IsAvailable => Status == MissionStatus.NotStarted;

        public MissionViewModel(Mission mission, int totalPlayerCount)
        {
            Num = mission.Num;
            Status = mission.Status;
            PlayerCount = Rules.GetMissionPlayerCount(totalPlayerCount, mission.Num);
            IsEasy = Rules.MissionIsEasy(totalPlayerCount, mission.Num);
        }

        public override string ToString() => 
            string.Format(Replic.MissionDescription, Num + 1, PlayerCount, IsEasy ? Replic.Easy : "", Status.GetDescription());
    }
}
