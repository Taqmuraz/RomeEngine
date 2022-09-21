﻿using System.Collections.Generic;

namespace RomeEngine
{
    public abstract class Renderer : Component
    {
        static List<Renderer> renderers = new List<Renderer>();

        [BehaviourEvent]
        void Start()
        {
            renderers.Add(this);
        }
        protected abstract void VisitContext(IGraphicsContext context);
        protected abstract void Draw(IGraphics graphics);

        public static void UpdateGraphics(IGraphics graphics, IGraphicsContext context)
        {
            var camera = Camera.ActiveCamera;
            graphics.Clear(camera.ClearColor);
            graphics.SetProjectionMatrix(camera.Projection);
            graphics.SetViewMatrix(camera.View);

            foreach (var renderer in renderers)
            {
                renderer.VisitContext(context);
                graphics.SetModelMatrix(renderer.Transform.LocalToWorld);
                renderer.Draw(graphics);
            }
        }
    }
}
