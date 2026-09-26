using FlowIoC.BaseModule.Adapters;
using FlowIoC.BaseModule.Constructables;
using FlowIoC.BaseModule.Injectable.Attributes;
using Modules.GridModule.Data.UnityObjects;
using Modules.GridModule.RootsContexts;
using Modules.GridModule.Shared.Data.ValueObjects;
using UnityEngine;

namespace Modules.GridModule.Models
{
    /// <summary>
    /// Holds the grid's state. The cells live in RD_Grid, the runtime asset filed on the Root's adapter;
    /// the model keeps that asset rather than a copy of its cells, so the two can never drift apart.
    /// </summary>
    internal class GridModel : IGridModel, IConstructable
    {
        [Inject(nameof(GridServiceContext))]
        private GameObject _root { get; set; }

        public bool IsPostConstructed { get; set; }
        public bool IsDeconstructed { get; set; }

        public Vector2Int GridSize { get; set; }
        public float CellSize { get; set; }
        public Rect Bounds { get; set; }
        public int LastEntityId { get; set; }

        public CellVO[,] Cells
        {
            get => _runtimeData.Cells;
            set => _runtimeData.Cells = value;
        }

        private RD_Grid _runtimeData;

        public void PostConstruct() => _runtimeData = _root.GetComponent<RootAdapter>().GetScriptable<RD_Grid>();

        // The asset keeps its fields between play sessions in the Editor, so the cells are cleared here
        // rather than left behind for the next session to find.
        public void Deconstruct() => _runtimeData.Cells = null;
    }
}
