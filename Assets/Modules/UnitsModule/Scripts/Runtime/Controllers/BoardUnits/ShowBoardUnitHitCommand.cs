using DG.Tweening;
using FlowIoC.BaseModule.Controller;
using FlowIoC.BaseModule.Injectable.Attributes;
using Modules.GridModule.Shared.Data.ValueObjects;
using Modules.UnitsModule.Entities;
using Modules.UnitsModule.Models;
using UnityEngine;

namespace Modules.UnitsModule.Controllers.BoardUnits
{
    /// <summary>
    /// A unit was struck and still stands: its object - reached through its data - flashes CD_Units' hit colour and
    /// back to the colour it wears, and its health bar shows the health left. Both are vertex colours and atlas
    /// sprites, so it still batches.
    /// </summary>
    internal class ShowBoardUnitHitCommand : Command
    {
        [Inject]      private IUnitsModel _unitsModel { get; set; }
        [SignalParam] private BoardUnitVO _unit       { get; set; }

        public override void Execute()
        {
            var view = (BoardUnit)_unit.View;
            SpriteRenderer renderer = view.Renderer;

            view.Flash?.Kill();
            renderer.color = view.Tint;
            view.Flash = DOTween.To(() => renderer.color, color => renderer.color = color, _unitsModel.HitFlash,
                                    _unitsModel.HitFlashDuration * 0.5f)
                                .SetLoops(2, LoopType.Yoyo);

            float health = (float)_unit.Hp / _unit.MaxHp;
            view.HealthBar.gameObject.SetActive(health < 1f);
            view.HealthFill.localScale = new Vector3(health, 1f, 1f);
        }
    }
}
