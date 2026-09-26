using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using Modules.GameBoardModule.Data.ValueObjects;
using Modules.GameBoardModule.Models;
using Modules.GameBoardModule.Shared.Data.ValueObjects;
using Modules.GameBoardModule.Shared.Enums;
using UnityEngine;

namespace Modules.GameBoardModule.Controllers
{
    /// <summary>
    /// Marks every cell of a placement's area as standing under a new building, so the free-area search
    /// passes it by from now on. The placement arrives from the step before it. The building gets an
    /// entity id of its own, which is what tells the cells of two neighbouring buildings apart.
    /// </summary>
    internal class OccupyBuildingAreaCommand : Command<BuildingPlacementVO>
    {
        [Inject] private IGameBoardModel _gameBoardModel { get; set; }

        public override void Execute(BuildingPlacementVO placement)
        {
            _gameBoardModel.LastEntityId++;
            var occupant = new CellOccupantVO(_gameBoardModel.LastEntityId, CellOccupantType.Building);

            CellVO[,] cells = _gameBoardModel.Cells;
            foreach (Vector2Int cell in placement.Area.allPositionsWithin)
                cells[cell.x, cell.y].Occupant = occupant;
        }
    }
}
