using Modules.GameplayModule.InfoScreenModule.Entities;
using Modules.UnitsModule.Shared.Data.ValueObjects;
using UnityEngine;

namespace Modules.GameplayModule.InfoScreenModule.Data.ValueObjects
{
    /// <summary>
    /// A pooled card, the unit it asks for - which unit, and the shown building's door and spawn point - and the
    /// unit's image and name.
    /// </summary>
    internal readonly struct UnitCardVO
    {
        public readonly UnitItem Card;
        public readonly UnitSpawnRequestVO Request;
        public readonly Sprite Sprite;
        public readonly string Name;

        public UnitCardVO(UnitItem card, UnitSpawnRequestVO request, Sprite sprite, string name)
        {
            Card = card;
            Request = request;
            Sprite = sprite;
            Name = name;
        }
    }
}
