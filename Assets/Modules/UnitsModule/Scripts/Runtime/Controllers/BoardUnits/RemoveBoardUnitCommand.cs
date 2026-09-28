using DG.Tweening;
using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using FlowIoC.PoolModule.Services;
using Modules.GridModule.Services;
using Modules.GridModule.Shared.Data.ValueObjects;
using Modules.UnitsModule.Data.ValueObjects;
using Modules.UnitsModule.Entities;
using Modules.UnitsModule.Models;
using Modules.UnitsModule.Signals;
using UnityEngine;

namespace Modules.UnitsModule.Controllers.BoardUnits
{
    /// <summary>
    /// A unit was destroyed - the grid has already taken it off the board: it is no longer selected, if it was, which
    /// is announced; a puff (pooled, key "unit_explosion") grows and fades where it stood - one of CD_Units' explosion
    /// sprites, at a random turn - and goes back to the pool when it is over; the unit's object - reached through its
    /// data - goes back to the pool at once (group "units").
    /// </summary>
    internal class RemoveBoardUnitCommand : Command
    {
        /// <summary>The key of the puff in CD_PoolGroup_Units.</summary>
        private const string ExplosionPoolKey = "unit_explosion";

        /// <summary>The share of its full width a puff starts at.</summary>
        private const float PuffStartScale = 0.4f;

        [Inject]       private IUnitSelectionModel _selectionModel { get; set; }
        [Inject]       private IUnitsModel         _unitsModel     { get; set; }
        [Inject]       private IGridService        _gridService    { get; set; }
        [Inject]       private IPoolService        _poolService    { get; set; }
        [InjectSignal] private UnitsSignals        _signals        { get; set; }
        [SignalParam]  private BoardUnitVO         _unit           { get; set; }

        public override void Execute()
        {
            if (_selectionModel.Selected == _unit)
            {
                _selectionModel.ClearSelection();
                _signals.Outgoing.SelectionCleared.Dispatch();
            }

            var view = (BoardUnit)_unit.View;
            PlayExplosion(view.transform.position);
            _poolService.Return.Item(view);
        }

        private void PlayExplosion(Vector3 at)
        {
            UnitExplosionVO explosion = _unitsModel.Explosion;
            Sprite sprite = explosion.Sprites[Random.Range(0, explosion.Sprites.Length)];

            var puff = _poolService.Get<BoardExplosion>(ExplosionPoolKey, _unitsModel.BoardParent);
            puff.Renderer.sprite = sprite;
            puff.transform.position = at;
            puff.transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));

            // The sprite is scaled to the puff's width in cells, whatever its own pixel size.
            float scale = explosion.Size * _gridService.CellSize / Mathf.Max(sprite.bounds.size.x, sprite.bounds.size.y);
            puff.transform.localScale = Vector3.one * (scale * PuffStartScale);

            IPoolService pool = _poolService;
            SpriteRenderer renderer = puff.Renderer;
            puff.Puff = DOTween.Sequence()
                               .Join(puff.transform.DOScale(scale, explosion.Duration).SetEase(Ease.OutQuad))
                               .Join(DOTween.ToAlpha(() => renderer.color, color => renderer.color = color, 0f, explosion.Duration)
                                            .SetEase(Ease.InQuad))
                               .OnComplete(() => pool.Return.Item(puff));
        }
    }
}
