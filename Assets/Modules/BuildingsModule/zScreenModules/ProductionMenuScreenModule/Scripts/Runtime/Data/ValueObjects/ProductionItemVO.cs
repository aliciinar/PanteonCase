using Modules.BuildingsModule.Shared.Enums;
using UnityEngine;

namespace Modules.BuildingsModule.ProductionMenuScreenModule.Data.ValueObjects
{
    /// <summary>One entry of the production menu: the building and the sprite it is shown with.</summary>
    internal readonly struct ProductionItemVO
    {
        public readonly BuildType Type;
        public readonly Sprite Sprite;

        public ProductionItemVO(BuildType type, Sprite sprite)
        {
            Type = type;
            Sprite = sprite;
        }
    }
}
