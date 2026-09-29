using FlowIoC.BaseModule.Adapters;
using FlowIoC.BaseModule.Constructables;
using FlowIoC.BaseModule.Injectable.Attributes;
using Modules.MainModule.Entities;
using Modules.MainModule.RootsContexts;
using Modules.MainModule.Signals;
using UnityEngine;

namespace Modules.MainModule.Models
{
    /// <summary>
    /// The window size as of the last change. Takes the ScreenResizeView off MainRoot's adapter and
    /// passes each size it reports on as MainInternalSignals.ScreenSizeChanged; ApplyScreenSizeCommand
    /// decides whether it is new and sets it here.
    /// </summary>
    internal class ScreenResizeModel : IScreenResizeModel, IConstructable
    {
        [Inject(nameof(MainContext))]
        private GameObject _root { get; set; }

        [InjectSignal] private MainInternalSignals _internalSignals { get; set; }

        public bool IsPostConstructed { get; set; }
        public bool IsDeconstructed { get; set; }

        public Vector2Int ScreenSize { get; private set; }

        private ScreenResizeView _view;

        public void PostConstruct()
        {
            ScreenSize = new Vector2Int(Screen.width, Screen.height);

            _view = _root.GetComponent<RootAdapter>().GetMonoBehaviour<ScreenResizeView>();
            _view.Resized += OnResized;
        }

        public void Deconstruct() => _view.Resized -= OnResized;

        public void SetScreenSize(Vector2Int screenSize) => ScreenSize = screenSize;

        private void OnResized(Vector2Int screenSize) => _internalSignals.ScreenSizeChanged.Dispatch(screenSize);
    }
}
