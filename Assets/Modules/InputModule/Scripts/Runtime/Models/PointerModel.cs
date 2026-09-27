namespace Modules.InputModule.Models
{
    internal class PointerModel : IPointerModel
    {
        public bool IsPressOnWorld { get; private set; }

        public void BeginPress(bool onWorld) => IsPressOnWorld = onWorld;

        public void EndPress() => IsPressOnWorld = false;
    }
}
