using Modules.GridModule.Shared.Entities;
using Modules.GridModule.Shared.Enums;
using UnityEngine;

namespace Modules.GridModule.Shared.Data.ValueObjects
{
    /// <summary>
    /// What stands on a cell. The grid is the one place a thing on the board keeps its data: every cell an
    /// occupant covers holds the same instance, and whoever needs the thing reads it off the cell. Kind is
    /// what a search or a press asks first; the concrete type carries the rest. Its health changes only
    /// through the grid (Grid's DamageOccupant); at 0 the grid takes it off the board. It also keeps the object that
    /// shows it on the board, so a cell leads to that object as it leads to the data.
    /// </summary>
    public abstract class CellOccupantVO
    {
        public abstract CellOccupantType Kind { get; }

        /// <summary>The cells it covers: bottom-left cell and size.</summary>
        public abstract RectInt Area { get; }

        public int MaxHp { get; }

        public int Hp { get; internal set; }

        /// <summary>The object that shows it on the board.</summary>
        public IOccupantView View { get; }

        protected CellOccupantVO(int maxHp, IOccupantView view)
        {
            MaxHp = maxHp;
            Hp = maxHp;
            View = view;
        }
    }
}
