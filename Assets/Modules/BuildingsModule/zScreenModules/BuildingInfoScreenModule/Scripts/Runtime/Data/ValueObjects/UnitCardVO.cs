using Modules.BuildingsModule.BuildingInfoScreenModule.Entities;
using Modules.UnitsModule.Shared.Data.ValueObjects;
using UnityEngine;

namespace Modules.BuildingsModule.BuildingInfoScreenModule.Data.ValueObjects
{
    /// <summary>A pooled card, the unit it asks for - which unit, and the shown building's door and spawn point - and its image.</summary>
    internal readonly struct UnitCardVO
    {
        public readonly UnitItem Card;
        public readonly UnitSpawnRequestVO Request;
        public readonly Sprite Sprite;

        public UnitCardVO(UnitItem card, UnitSpawnRequestVO request, Sprite sprite)
        {
            Card = card;
            Request = request;
            Sprite = sprite;
        }
    }
}
