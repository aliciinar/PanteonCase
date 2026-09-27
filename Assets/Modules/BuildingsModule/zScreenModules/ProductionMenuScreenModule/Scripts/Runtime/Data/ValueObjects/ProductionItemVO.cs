using Modules.BuildingsModule.Shared.Enums;
using UnityEngine;

namespace Modules.BuildingsModule.ProductionMenuScreenModule.Data.ValueObjects
{
    /// <summary>One entry of the production menu: the building, the sprite it is shown with and its label.</summary>
    internal readonly struct ProductionItemVO
    {
        public readonly BuildType Type;
        public readonly Sprite Sprite;

        /// <summary>The building's name as its card prints it - made once, so a card that scrolls into another row allocates no string.</summary>
        public readonly string Label;

        public ProductionItemVO(BuildType type, Sprite sprite, string label)
        {
            Type = type;
            Sprite = sprite;
            Label = label;
        }
    }
}
