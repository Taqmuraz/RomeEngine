using System;
using System.Drawing;

using OneEngine;

namespace OneEngineWindowsFormsApplication
{
    class CanvasStyleBase
    {
        public Graphics Graphics { get; set; }
        public SolidBrush Brush { get; set; }
        public Pen Pen { get; set; }
        public Matrix3x3 Transform { get; set; }

        public virtual void Setup()
        {
        }

        static PointF[][] nonAllocPoints;

        protected PointF[] AllocPoints(int count)
        {
            return nonAllocPoints[count];
        }

        static CanvasStyleBase()
        {
            nonAllocPoints = new PointF[128][];
            for (int i = 0; i < nonAllocPoints.Length; i++) nonAllocPoints[i] = new PointF[i];
        }

        public void DrawText(Vector2 position, string text, int fontSize)
        {
            DrawInReversedScale(() => Graphics.DrawString(text, CanvasGraphics.CreateFont(fontSize), Brush, position));
        }
        protected void DrawInReversedScale(Action drawAction)
        {
            var transform = Graphics.Transform;
            var reversed = transform.Clone();
            reversed.Scale(1f, -1f);
            Graphics.Transform = reversed;
            drawAction();
            Graphics.Transform = transform;
        }
    }
}
