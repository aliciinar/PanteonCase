using Modules.GameBoardModule.Shared.Enums;

namespace Modules.GameBoardModule.Shared.Data.ValueObjects
{
    /// <summary>What stands on a cell: which entity, and what kind it is. A free cell holds none.</summary>
    public class CellOccupantVO
    {
        /// <summary>The entity standing here. Every cell a building covers holds the same id.</summary>
        public int EntityId { get; }

        public CellOccupantType Type { get; }

        public CellOccupantVO(int entityId, CellOccupantType type)
        {
            EntityId = entityId;
            Type = type;
        }
    }
}
