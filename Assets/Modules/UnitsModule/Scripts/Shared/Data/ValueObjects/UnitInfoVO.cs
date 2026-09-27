using Modules.UnitsModule.Shared.Enums;

namespace Modules.UnitsModule.Shared.Data.ValueObjects
{
    /// <summary>
    /// The selected unit, as much of it as changes during play: which unit and its health now. Everything else a
    /// reader shows of it - its name, image, full health and damage - is authored in CD_Units and read there.
    /// </summary>
    public class UnitInfoVO
    {
        public UnitType Type { get; }
        public int Hp { get; }

        public UnitInfoVO(UnitType type, int hp)
        {
            Type = type;
            Hp = hp;
        }
    }
}
