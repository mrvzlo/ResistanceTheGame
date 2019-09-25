using System.ComponentModel;

namespace Resistance.Enums
{
    public enum MissionStatus
    {
        [Description(Replic.MissionIsAvailable)]
        NotStarted,
        [Description(Replic.MissionStarted)]
        Started,
        [Description(Replic.MissionFailed)]
        RedWon,
        [Description(Replic.MissionSucceeded)]
        BlueWon
    }
}
