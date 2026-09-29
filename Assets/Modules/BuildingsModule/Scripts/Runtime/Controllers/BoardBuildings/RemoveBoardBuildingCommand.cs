using DG.Tweening;
using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using FlowIoC.PoolModule.Services;
using Modules.BuildingsModule.Entities;
using Modules.BuildingsModule.Models;
using Modules.BuildingsModule.Shared.Data.ValueObjects;
using Modules.BuildingsModule.Signals;
using Modules.GridModule.Services;
using Modules.GridModule.Shared.Data.ValueObjects;
using UnityEngine;

namespace Modules.BuildingsModule.Controllers.BoardBuildings
{
    /// <summary>
    /// A building was destroyed - the grid has already taken it off the board: it is no longer selected, if it was, which
    /// is announced - the info screen showing it hides; it explodes as CD_Buildings' explosion says, in puffs (pooled,
    /// key "building_explosion") - the first at its centre, the rest at random points over its footprint, each a little
    /// after the one before - each growing and fading, then going back to the pool; the building's object - reached
    /// through its data - goes back to the pool at once (group "board_buildings").
    /// </summary>
    internal class RemoveBoardBuildingCommand : Command
    {
        /// <summary>The key of a puff in CD_PoolGroup_BoardBuildings.</summary>
        private const string ExplosionPoolKey = "building_explosion";

        [Inject]       private IBuildingSelectionModel _selectionModel { get; set; }
        [Inject]       private IBuildingsModel         _buildingsModel { get; set; }
        [Inject]       private IGridService            _gridService    { get; set; }
        [Inject]       private IPoolService            _poolService    { get; set; }
        [InjectSignal] private BuildingsSignals        _signals        { get; set; }
        [SignalParam]  private BoardBuildingVO         _building       { get; set; }

        public override void Execute()
        {
            if (_selectionModel.Selected == _building)
            {
                _selectionModel.ClearSelection();
                _signals.Outgoing.SelectionCleared.Dispatch();
            }

            var view = (BoardBuilding)_building.View;
            Rect area = _gridService.AreaToWorldRect(_building.Area);
            float z = view.transform.position.z;

            BuildingExplosionCVO explosion = _buildingsModel.Explosion;
            for (int i = 0; i < explosion.Count; i++)
            {
                Vector2 offset = i == 0
                    ? Vector2.zero
                    : new Vector2(Random.Range(-1f, 1f) * area.width, Random.Range(-1f, 1f) * area.height) * (0.5f * explosion.Spread);
                PlayPuff(explosion, new Vector3(area.center.x + offset.x, area.center.y + offset.y, z),
                         explosion.Size * Mathf.Max(area.width, area.height), i * explosion.Stagger);
            }

            _poolService.Return.Item(view);
        }

        private void PlayPuff(BuildingExplosionCVO explosion, Vector3 at, float width, float delay)
        {
            Sprite sprite = explosion.Sprites[Random.Range(0, explosion.Sprites.Length)];

            var puff = _poolService.Get<BoardExplosion>(ExplosionPoolKey, _buildingsModel.BoardParent);
            puff.Renderer.sprite = sprite;
            puff.transform.position = at;
            puff.transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
            puff.transform.localScale = Vector3.zero; // unseen until its turn

            // The sprite is scaled to the puff's width, whatever its own pixel size.
            float scale = width / Mathf.Max(sprite.bounds.size.x, sprite.bounds.size.y);

            IPoolService pool = _poolService;
            SpriteRenderer renderer = puff.Renderer;
            puff.Puff = DOTween.Sequence()
                               .AppendInterval(delay)
                               .AppendCallback(() => puff.transform.localScale = Vector3.one * (scale * explosion.StartScale))
                               .Append(puff.transform.DOScale(scale, explosion.Duration).SetEase(Ease.OutQuad))
                               .Join(DOTween.ToAlpha(() => renderer.color, color => renderer.color = color, 0f, explosion.Duration)
                                            .SetEase(Ease.InQuad))
                               .OnComplete(() => pool.Return.Item(puff));
        }
    }
}
