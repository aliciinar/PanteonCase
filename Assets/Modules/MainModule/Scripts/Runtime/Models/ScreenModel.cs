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
    /// Takes the ScreenResizeNotifier off MainRoot's adapter and, whenever it reports a new size,
    /// stores it and announces it on MainSignals.Outgoing.ScreenResized.
    /// </summary>
    public class ScreenModel : IScreenModel, IConstructable
    {
        [Inject(nameof(MainContext))]
        private GameObject _root { get; set; }

        [InjectSignal] private MainSignals _mainSignals { get; set; }

        public bool IsPostConstructed { get; set; }
        public bool IsDeconstructed { get; set; }

        public Vector2Int ScreenSize { get; private set; }

        private ScreenResizeNotifier _notifier;

        public void PostConstruct()
        {
            ScreenSize = new Vector2Int(Screen.width, Screen.height);

            _notifier = _root.GetComponent<RootAdapter>().GetMonoBehaviour<ScreenResizeNotifier>();
            _notifier.Resized += OnResized;
        }

        public void Deconstruct() => _notifier.Resized -= OnResized;

        private void OnResized(Vector2Int screenSize)
        {
            // A minimised window reports a zero-sized screen; there is nothing to lay out against.
            if (screenSize == ScreenSize || screenSize.x <= 0 || screenSize.y <= 0) return;

            ScreenSize = screenSize;
            _mainSignals.Outgoing.ScreenResized.Dispatch(ScreenSize);
        }
    }
}
