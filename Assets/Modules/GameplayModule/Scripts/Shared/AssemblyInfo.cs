using System.Runtime.CompilerServices;

// The game status is read by every module and written only by the gameplay module: its setters are internal, and the
// gameplay module's runtime assembly is the one that sees them.
[assembly: InternalsVisibleTo("Modules.Gameplay")]
