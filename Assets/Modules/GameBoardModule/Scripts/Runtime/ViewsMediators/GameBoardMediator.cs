using FlowIoC.BaseModule.Injectable.Attributes;
using FlowIoC.BaseModule.ViewsMediators.Mediator;
using Modules.GameBoardModule.Data.ValueObjects;
using Modules.GameBoardModule.Signals;

namespace Modules.GameBoardModule.ViewsMediators
{
    public class GameBoardMediator : IMediator
    {
        [Inject]       private GameBoardView            _view            { get; set; }
        [InjectSignal] private GameBoardInternalSignals _internalSignals { get; set; }

        public void OnRegister() => _internalSignals.Draw.AddListener(OnDraw);

        public void OnRemove() => _internalSignals.Draw.RemoveListener(OnDraw);

        private void OnDraw(GameBoardLayoutVO layout) =>
            _view.Draw(layout.GridBounds, layout.FrameBounds, layout.CellSize, layout.Cells);
    }
}
