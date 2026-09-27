using Modules.BuildingsModule.BuildingInfoScreenModule.Entities;
using Modules.UnitsModule.Shared.Enums;
using UnityEngine;

namespace Modules.BuildingsModule.BuildingInfoScreenModule.Data.ValueObjects
{
    /// <summary>A pooled card and the unit it is to show.</summary>
    internal readonly struct UnitCardVO
    {
        public readonly UnitItem Card;
        public readonly UnitType Type;
        public readonly Sprite Sprite;

        public UnitCardVO(UnitItem card, UnitType type, Sprite sprite)
        {
            Card = card;
            Type = type;
            Sprite = sprite;
        }
    }
}
