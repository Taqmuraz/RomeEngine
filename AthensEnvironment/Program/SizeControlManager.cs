using System.Drawing;
using System.Windows.Forms;

namespace AthensEnvironment
{
    sealed class SizeControlManager<TControl> : ControlManager<TControl> where TControl : Control, new()
    {
        public SizeControlManager(Form root, RectangleF normalizedRect)
        {
            Control.Parent = root;

            void Setup()
            {
                Control.Size = new Size((int)(root.Width * normalizedRect.Width), (int)(root.Height * normalizedRect.Height));
                Control.Location = new Point((int)(root.Width * normalizedRect.X), (int)(root.Height * normalizedRect.Y));
            }
            root.SizeChanged += (s, e) => Setup();
            Setup();
        }
    }
}
