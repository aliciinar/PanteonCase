using DG.Tweening;
using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using FlowIoC.PoolModule.Services;
using Modules.BuildingsModule.Data.ValueObjects;
using Modules.BuildingsModule.Entities;
using Modules.BuildingsModule.Models;
using Modules.GridModule.Services;
using Modules.GridModule.Shared.Data.ValueObjects;
using UnityEngine;

namespace Modules.BuildingsModule.Controllers.BoardBuildings
{
    /// <summary>
    /// A building was destroyed - the grid has already taken it off the board: it explodes in CD_Buildings' number of
    /// puffs (pooled, key "building_explosion") - the first at its centre, the rest at random points over its footprint,
    /// each a little after the one before - each growing and fading, then going back to the pool; the building's
    /// object - reached through its data - goes back to the pool at once (group "board_buildings").
    /// </summary>
    internal class ReturnBoardBuildingCommand : Command
    {
        /// <summary>The key of a puff in CD_PoolGroup_BoardBuildings.</summary>
        private const string ExplosionPoolKey = "building_explosion";

        /// <summary>The share of its full width a puff starts at.</summary>
        private const float PuffStartScale = 0.4f;

        /// <summary>How far from the footprint's centre the later puffs may land, as a share of its half-size.</summary>
        private const float PuffSpread = 0.6f;

        [Inject]      private IBuildingsModel _buildingsModel { get; set; }
        [Inject]      private IGridService    _gridService    { get; set; }
        [Inject]      private IPoolService    _poolService    { get; set; }
        [SignalParam] private BoardBuildingVO _building       { get; set; }

        public override void Execute()
        {
            var view = (BoardBuilding)_building.View;
            Rect area = _gridService.AreaToWorldRect(_building.Area);
            float z = view.transform.position.z;

            BuildingExplosionVO explosion = _buildingsModel.Explosion;
            for (int i = 0; i < explosion.Count; i++)
            {
                Vector2 offset = i == 0
                    ? Vector2.zero
                    : new Vector2(Random.Range(-1f, 1f) * area.width, Random.Range(-1f, 1f) * area.height) * (0.5f * PuffSpread);
                PlayPuff(explosion, new Vector3(area.center.x + offset.x, area.center.y + offset.y, z),
                         explosion.Size * Mathf.Max(area.width, area.height), i * explosion.Stagger);
            }

            _poolService.Return.Item(view);
        }

        private void PlayPuff(BuildingExplosionVO explosion, Vector3 at, float width, float delay)
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
                               .AppendCallback(() => puff.transform.localScale = Vector3.one * (scale * PuffStartScale))
                               .Append(puff.transform.DOScale(scale, explosion.Duration).SetEase(Ease.OutQuad))
                               .Join(DOTween.ToAlpha(() => renderer.color, color => renderer.color = color, 0f, explosion.Duration)
                                            .SetEase(Ease.InQuad))
                               .OnComplete(() => pool.Return.Item(puff));
        }
    }
}
