using System;
using DG.Tweening;
using FlowIoC.BaseModule.Injectable.Components;
using FlowIoC.ScreenModule.ViewsMediators.Screen;
using Modules.ScreenModule.PopupScreenModule.Data.ValueObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.ScreenModule.PopupScreenModule.ViewsMediators
{
    /// <summary>
    /// A message on a panel in the middle of the screen, a warning sign above it, over a translucent backdrop that
    /// covers the whole screen: nothing under the popup can be clicked while it is up, and a click on the backdrop
    /// dismisses it - the message panel takes no clicks, so a click on it reaches the backdrop too. What it says is
    /// whatever it is handed.
    /// </summary>
    [RequireComponent(typeof(ViewInjector))]
    public class PopupScreenView : ScreenView
    {
        [SerializeField] private Button _backdrop;
        [SerializeField] private RectTransform _panel;
        [SerializeField] private TMP_Text _message;

        public Action Dismissed;

        private PopupAnimationVO _animation;
        private Tween _pop;

        private void OnEnable() => _backdrop.onClick.AddListener(DismissClicked);

        private void OnDisable()
        {
            _backdrop.onClick.RemoveListener(DismissClicked);
            _pop?.Kill();
        }

        public void DismissClicked() => Dismissed?.Invoke();

        internal void ShowMessage(string message) => _message.text = message;

        /// <summary>The opening command hands over how the panel pops in (CD_PopupScreen).</summary>
        public override void BeforeScreenActivation()
        {
            base.BeforeScreenActivation();
            _animation = (PopupAnimationVO)Data.Parameters[0];
        }

        /// <summary>The panel pops in; the screen takes clicks only once it has.</summary>
        protected override void PlayShowAnimation()
        {
            _pop?.Kill();
            _panel.localScale = Vector3.one * _animation.FromScale;
            _pop = _panel.DOScale(1f, _animation.Duration)
                         .SetEase(Ease.OutBack)
                         .SetUpdate(true)
                         .OnComplete(() => ShowCompleted?.Invoke(this));
        }

        protected override void PlayHideAnimation() => HideCompleted?.Invoke(this);
    }
}
