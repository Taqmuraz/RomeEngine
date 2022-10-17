using System.Windows.Forms;

namespace AthensEnvironment
{
    abstract class ControlManager<TControl> where TControl : Control, new()
    {
        public ControlManager()
        {
            Control = new TControl();
        }

        public TControl Control { get; }
    }
}
