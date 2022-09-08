using System;
using System.Drawing;

using RomeEngine;

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
