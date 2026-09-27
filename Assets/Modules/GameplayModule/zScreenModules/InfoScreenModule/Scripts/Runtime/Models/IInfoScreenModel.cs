using System.Collections.Generic;
using Modules.BuildingsModule.Shared.Data.ValueObjects;
using Modules.BuildingsModule.Shared.Enums;
using Modules.GameplayModule.InfoScreenModule.Enums;
using Modules.UnitsModule.Shared.Data.ValueObjects;
using Modules.UnitsModule.Shared.Enums;

namespace Modules.GameplayModule.InfoScreenModule.Models
{
    /// <summary>
    /// Every building and every unit as their configs author them - what the screen shows of them - and which of the
    /// two the screen shows now.
    /// </summary>
    internal interface IInfoScreenModel
    {
        IReadOnlyDictionary<BuildType, BuildingCVO> Buildings { get; }

        IReadOnlyDictionary<UnitType, UnitCVO> Units { get; }

        /// <summary>
        /// What the screen shows now. A selection is cleared in one module while another is made in a second, in no
        /// order the screen can rely on, so clearing hides the screen only while it still shows what was cleared.
        /// </summary>
        InfoSubjectType Shown { get; }

        void SetShown(InfoSubjectType subject);
    }
}
