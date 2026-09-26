using System.Runtime.CompilerServices;

// What stands on a cell (CellVO.Occupant) is readable by every module but written only by GameBoardModule's
// own commands, so its setter is internal and only the module's runtime assembly sees it.
[assembly: InternalsVisibleTo("Modules.GameBoard")]
