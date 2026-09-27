using System.Collections.Generic;

namespace Modules.BuildingsModule.ProductionMenuScreenModule.Data.ValueObjects
{
    /// <summary>
    /// The rows of the menu now on screen, and those of them that were not a moment ago. Handed from the step that
    /// works them out to the step that moves the cards.
    /// </summary>
    internal class VisibleRowsVO
    {
        public int First { get; }

        public int Last { get; }

        /// <summary>The rows that came into view - the ones whose cells need a card.</summary>
        public IReadOnlyList<int> Entered { get; }

        public VisibleRowsVO(int first, int last, IReadOnlyList<int> entered)
        {
            First = first;
            Last = last;
            Entered = entered;
        }
    }
}
