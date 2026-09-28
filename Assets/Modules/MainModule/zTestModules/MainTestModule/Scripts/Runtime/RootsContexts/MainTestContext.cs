#if UNITY_EDITOR
using FlowIoC.BaseModule.Connectors;
using FlowIoC.BaseModule.Contexts;
using FlowIoC.BaseModule.SharedData;
using FlowIoC.ConsoleModule;
using FlowIoC.PoolModule.Services;
using Modules.BuildingsModule.Entities;
using Modules.BuildingsModule.Shared.Data.UnityObjects;
using Modules.GameBoardModule.Signals;
using Modules.GridModule.Services;
using Modules.GridModule.Shared.Data.ValueObjects;
using Modules.MainModule.MainTestModule.Data.UnityObjects;
using Modules.UnitsModule.Entities;
using Modules.UnitsModule.Shared.Data.UnityObjects;
using UnityEngine;

namespace Modules.MainModule.MainTestModule.RootsContexts
{
    /// <summary>
    /// The design test scene: the whole game, booted by MainRoot as in MainScene, on the test copies of the design
    /// configs in MainTestModule/Scriptables - assigned in this scene to the RootAdapters of the Roots that read them.
    /// As soon as the board is built, the buildings and soldiers the designer put in RD_Grid_Test stand on their
    /// cells at full health; an entry off the grid or on a taken cell is skipped with a warning. The placing steps
    /// mirror PlaceBoardBuildingCommand / PlaceBoardUnitCommand, without the walk and the lock.
    /// </summary>
    public class MainTestContext : Context
    {
        private const string TestSuffix = "_Test";

        private IGridService _grid;
        private IPoolService _pool;
        private CD_Buildings _buildings;
        private CD_Units _units;
        private RD_GridTest _startBoard;
        private GameBoardSignals _gameBoardSignals;

        public override void Setup()
        {
            base.Setup();

            _grid = InjectionBinderCrossContext.GetInstance<IGridService>();
            _pool = InjectionBinderCrossContext.GetInstance<IPoolService>();

            var sharedData = InjectionBinderCrossContext.GetInstance<ISharedDataModel>();
            _buildings = sharedData.GetScriptable<CD_Buildings>();
            _units = sharedData.GetScriptable<CD_Units>();
            _startBoard = sharedData.GetScriptable<RD_GridTest>();
            Check(_buildings);
            Check(_units);

            _gameBoardSignals = InjectionBinderCrossContext.GetInstance<GameBoardSignals>();
            _gameBoardSignals.Outgoing.BoardBuilt.Connect(_ => PlaceStartBoard());

            FlowLogger.Log("Design test scene - the game runs on the *_Test configs and starts with RD_Grid_Test's board.");
        }

        public override void DestroyContext()
        {
            _gameBoardSignals?.Outgoing.BoardBuilt.Disconnect();
            base.DestroyContext();
        }

        private static void Check(ScriptableObject config)
        {
            if (!config.name.EndsWith(TestSuffix))
                FlowLogger.LogError($"Design test scene - {config.name} is the shipped config, not a test copy. " +
                                    "Assign its _Test copy on the RootAdapter of the Root that files it, in this scene.");
        }

        private void PlaceStartBoard()
        {
            Transform buildingsParent = GameObject.Find("BuildingsSystemRoot").transform;
            foreach (RD_GridTest.BuildingEntry entry in _startBoard.Buildings)
            {
                var config = _buildings.Buildings[entry.Type];
                var area = new RectInt(entry.Origin, config.Size);
                if (!_grid.IsAreaFree(area))
                {
                    Debug.LogWarning($"[Design test] {entry.Type} at {entry.Origin} does not fit; skipped.");
                    continue;
                }

                var view = _pool.Get<BoardBuilding>("board_building", buildingsParent);
                _grid.Occupy(area, new BoardBuildingVO(entry.Type, area, config.Hp, view));

                Rect rect = _grid.AreaToWorldRect(area);
                view.name = entry.Type.ToString();
                view.Sprite.Show(config.BoardSprite, rect);

                // The object is scaled to the footprint, so the bar undoes that scale to keep its own size.
                Vector3 scale = view.transform.lossyScale;
                float inset = _buildings.HealthBarInset * _grid.CellSize;
                view.HealthBar.position = new Vector3(rect.center.x, rect.yMax - inset, view.transform.position.z);
                view.HealthBar.localScale = new Vector3(rect.width * _buildings.HealthBarWidth / scale.x, 1f / scale.y, 1f);
            }

            Transform unitsParent = GameObject.Find("UnitsSystemRoot").transform;
            foreach (RD_GridTest.UnitEntry entry in _startBoard.Units)
            {
                var config = _units.Units[entry.Type];
                var area = new RectInt(entry.Cell, Vector2Int.one);
                if (!_grid.IsAreaFree(area))
                {
                    Debug.LogWarning($"[Design test] {entry.Type} at {entry.Cell} does not fit; skipped.");
                    continue;
                }

                var view = _pool.Get<BoardUnit>("board_unit", unitsParent);
                _grid.Occupy(area, new BoardUnitVO(entry.Type, config.Hp, entry.Cell, view));

                // The sprite is scaled to the cell, so a unit covers one cell whatever the sprite's own pixel size.
                Rect rect = _grid.AreaToWorldRect(area);
                Vector2 spriteSize = config.Sprite.bounds.size;
                view.name = entry.Type.ToString();
                view.Renderer.sprite = config.Sprite;
                view.transform.position = rect.center;
                view.transform.localScale = new Vector3(rect.width / spriteSize.x, rect.height / spriteSize.y, 1f);
            }
        }
    }
}
#endif
