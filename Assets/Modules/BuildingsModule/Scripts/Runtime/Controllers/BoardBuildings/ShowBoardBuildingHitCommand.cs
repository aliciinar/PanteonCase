using DG.Tweening;
using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using Modules.BuildingsModule.Entities;
using Modules.GridModule.Shared.Data.ValueObjects;
using UnityEngine;

namespace Modules.BuildingsModule.Controllers.BoardBuildings
{
    /// <summary>
    /// A building was struck and still stands: its object - reached through its data - flashes and back to its own
    /// colour, and its health bar shows the health left. Both are vertex colours and atlas sprites, so it still batches.
    /// </summary>
    internal class ShowBoardBuildingHitCommand : Command
    {
        [SignalParam] private BoardBuildingVO _building { get; set; }

        public override void Execute()
        {
            var view = (BoardBuilding)_building.View;
            SpriteRenderer renderer = view.Sprite.Renderer;

            view.Flash?.Complete();
            view.Flash = DOTween.To(() => renderer.color, color => renderer.color = color, view.HitFlash, view.HitFlashDuration * 0.5f)
                                .SetLoops(2, LoopType.Yoyo);

            float health = (float)_building.Hp / _building.MaxHp;
            view.HealthBar.gameObject.SetActive(health < 1f);
            view.HealthFill.localScale = new Vector3(health, 1f, 1f);
        }
    }
}
