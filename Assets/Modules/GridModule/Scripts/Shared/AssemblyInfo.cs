using System.Runtime.CompilerServices;

// What stands on a cell is read by every module and written only by the grid: the setters an occupant's data
// changes through are internal, and the grid's runtime assembly is the one that sees them.
[assembly: InternalsVisibleTo("Modules.Grid")]
