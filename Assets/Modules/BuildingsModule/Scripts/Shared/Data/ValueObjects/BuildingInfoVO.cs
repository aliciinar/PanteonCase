using Modules.BuildingsModule.Shared.Enums;

namespace Modules.BuildingsModule.Shared.Data.ValueObjects
{
    /// <summary>
    /// A building standing on the board, as a screen shows it: which building it is and the health it
    /// has left. Everything else a screen shows of it - its image, its full health, what it produces -
    /// is authored in CD_Buildings and read there.
    /// </summary>
    public class BuildingInfoVO
    {
        public BuildType Type { get; }

        public int Hp { get; }

        public BuildingInfoVO(BuildType type, int hp)
        {
            Type = type;
            Hp = hp;
        }
    }
}
