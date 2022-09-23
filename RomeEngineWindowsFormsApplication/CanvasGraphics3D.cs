using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using RomeEngine;

namespace OneEngineWindowsFormsApplication
{
    class CanvasGraphics3D : IGraphics
    {
        Graphics graphics;
        GraphicsContext context;
        PointF[] triangle = new PointF[3];
        CullingMode cullingMode;

        List<(Vector3 t0, Vector3 t1, Vector3 t2, Vector3 center, Vector3 worldNormal, Vector3 worldCenter)> triangles = new List<(Vector3, Vector3, Vector3, Vector3, Vector3, Vector3)>();

        SafeDictionary<TextureType, Texture> textures = new SafeDictionary<TextureType, Texture>();

        public void SetGraphics(Graphics graphics, GraphicsContext context)
        {
            this.context = context;
            this.graphics = graphics;
        }

        Matrix4x4 Viewport => Matrix4x4.CreateViewport(Screen.Size.x, Screen.Size.y);

        Matrix4x4 MVP => Viewport * projection * view.GetInversed() * model;
        Matrix4x4 VP => Viewport * projection * view.GetInversed();

        public void End()
        {
            var vp = VP;
            foreach (var t in triangles.OrderBy(t => -t.center.z))
            {
                triangle[0] = t.t0;
                triangle[1] = t.t1;
                triangle[2] = t.t2;
                graphics.FillPolygon(new SolidBrush(Color32.white * ((Vector3.Dot(t.worldNormal, -GlobalLight.LightDirection) + 1f) * 0.5f)), triangle);
            }
            triangles.Clear();
        }

        Matrix4x4 projection;
        Matrix4x4 view;
        Matrix4x4 model;

        public void Clear(Color32 color)
        {
            graphics.Clear(color);
        }

        public void SetProjectionMatrix(Matrix4x4 projection)
        {
            this.projection = projection;
        }

        public void SetViewMatrix(Matrix4x4 view)
        {
            this.view = view;
        }

        public void SetModelMatrix(Matrix4x4 model)
        {
            this.model = model;
        }

        public void SetTexture(Texture texture, TextureType type)
        {
            textures[type] = texture;
        }

        public void DrawMesh(IMeshIdentifier meshIdentifier)
        {
            var mesh = context.GetMesh(meshIdentifier);
            DrawDynamicMesh(mesh);
        }

        bool Cull(float dot)
        {
            switch (cullingMode)
            {
                default:
                case CullingMode.None:return false;
                case CullingMode.Back:return dot < 0f;
                case CullingMode.Front:return dot > 0f;
            }
        }

        public void DrawDynamicMesh(IMesh mesh)
        {
            var vertices = mesh.EnumerateVertices().ToArray();
            var indices = mesh.EnumerateIndices().ToArray();

            Matrix4x4 mvp = MVP;

            float[] positionsArray = vertices.SelectMany(v => v.Attributes.First().ToFloatsArray()).ToArray();

            Vector3 ReadVertex(int index)
            {
                index *= 3;
                return new Vector3(positionsArray[index], positionsArray[index + 1], positionsArray[index + 2]);
            }

            for (int i = 2; i < indices.Length; i+=3)
            {
                Vector3 t0 = ReadVertex(indices[i - 2]);
                Vector3 t1 = ReadVertex(indices[i - 1]);
                Vector3 t2 = ReadVertex(indices[i]);
                Vector3 worldNormal = model.MultiplyDirection(Vector3.Cross(t2 - t1, t0 - t1)).normalized;
                Vector3 worldCenter = model.MultiplyPoint((t0 + t1 + t2) * 0.333f);

                Vector3 vertexA = mvp.MultiplyPoint_With_WDivision(t0);
                Vector3 vertexB = mvp.MultiplyPoint_With_WDivision(t1);
                Vector3 vertexC = mvp.MultiplyPoint_With_WDivision(t2);

                Vector3 screenNormal = Vector3.Cross(vertexC - vertexB, vertexA - vertexB).normalized;
                float cameraDot = Vector3.Dot(Vector3.back, screenNormal);

                if (Cull(cameraDot)) triangles.Add((vertexA, vertexB, vertexC, (vertexA + vertexB + vertexC) * 0.333f, worldNormal, worldCenter));

                var pen = new Pen(Color.Black);
                graphics.DrawLine(pen, vertexA, vertexB);
                graphics.DrawLine(pen, vertexA, vertexC);
                graphics.DrawLine(pen, vertexB, vertexC);
            }
        }

        public void SetCulling(CullingMode cullingMode)
        {
            this.cullingMode = cullingMode;
        }
    }
}
