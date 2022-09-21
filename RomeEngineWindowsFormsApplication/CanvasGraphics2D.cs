using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using RomeEngine;

namespace OneEngineWindowsFormsApplication
{
    class MeshIdentifier : IMeshIdentifier
    {
    }
    class BitmapTexture : Texture
    {
        public BitmapTexture(Bitmap image)
        {
            Image = image;
        }

        public Bitmap Image { get; }
    }
    class Graphics3D : IGraphics
    {
        Graphics graphics;
        GraphicsContext context;

        SafeDictionary<TextureType, Texture> textures = new SafeDictionary<TextureType, Texture>();

        public void SetGraphics(Graphics graphics, GraphicsContext context)
        {
            this.context = context;
            this.graphics = graphics;
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

        public void DrawDynamicMesh(IMesh mesh)
        {
            var vertices = mesh.EnumerateVertices().ToArray();
            var indices = mesh.EnumerateIndices().ToArray();

            Matrix4x4 mvp = projection * view.GetInversed() * model;

            for (int i = 2; i < indices.Length; i+=3)
            {
                Vector3 vertexA = mvp.MultiplyPoint_With_WDevision(vertices[indices[i - 2]].Position);
                Vector3 vertexB = mvp.MultiplyPoint_With_WDevision(vertices[indices[i - 1]].Position);
                Vector3 vertexC = mvp.MultiplyPoint_With_WDevision(vertices[indices[i]].Position);
                var pen = new Pen(Color.White);
                graphics.DrawLine(pen, vertexA, vertexB);
                graphics.DrawLine(pen, vertexA, vertexC);
                graphics.DrawLine(pen, vertexB, vertexC);
            }
        }
    }
    class GraphicsContext : IGraphicsContext
    {
        Dictionary<IMeshIdentifier, IMesh> meshes = new Dictionary<IMeshIdentifier, IMesh>();
        Dictionary<IMesh, IMeshIdentifier> identifiers = new Dictionary<IMesh, IMeshIdentifier>();

        public IMesh GetMesh(IMeshIdentifier identifier) => meshes[identifier];

        public Texture LoadTexture(string fileName)
        {
            return new BitmapTexture(new Bitmap(fileName));
        }

        public IMeshIdentifier LoadMesh(IMesh mesh)
        {
            if (identifiers.TryGetValue(mesh, out IMeshIdentifier result)) return result;
            else
            {
                var identifier = new MeshIdentifier();
                meshes.Add(identifier, mesh);
                identifiers.Add(mesh, identifier);
                return identifier;
            }
        }
    }
    class CanvasGraphics2D : IGraphics2D
    {
        public Matrix3x3 Transform
        {
            get => transform;
            set
            {
                transform = value;
                try
                {
                    Graphics.Transform = new System.Drawing.Drawing2D.Matrix(value.Column_0.x, value.Column_0.y, value.Column_1.x, value.Column_1.y, value.Column_2.x, value.Column_2.y);
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex);
                }
            }
        }
        Matrix3x3 transform = Matrix3x3.identity;

        public IGraphicsBrush Brush { get; set; } = CanvasBrush.Default;
        public IGraphicsStyle Style
        {
            get => style;
            set
            {
                if (value is ICanvasStyle canvasStyle) style = canvasStyle;
                else throw new ArgumentException("Style must implement the ICanvasStyle interface");
            }
        }
        public IGraphicsStyle OutlineStyle => drawStyle;
        public IGraphicsStyle FillStyle => fillStyle;
        ICanvasStyle drawStyle;
        ICanvasStyle fillStyle;
        ICanvasStyle style;

        public Graphics Graphics { get; set; }
        public Vector2 ScreenSize { get; set; }

        public CanvasGraphics2D()
        {
            drawStyle = new CanvasOutlineStyle();
            fillStyle = new CanvasFillStyle();
            style = drawStyle;
        }

        ICanvasStyle SetupStyle()
        {
            style.Brush = new SolidBrush(Brush.Color);
            style.Pen = new Pen(Brush.Color, Brush.Size);
            style.Graphics = Graphics;
            style.Transform = Transform;
            style.Setup();
            return style;
        }

        public void DrawPoint(Vector2 position, float radius)
        {
            SetupStyle().DrawPoint(position, radius);
        }

        public void DrawLine(Vector2 a, Vector2 b)
        {
            SetupStyle().DrawLine(a, b);
        }

        public void DrawLine(Vector2 a, Vector2 b, float widthA, float widthB, bool smoothEnding)
        {
            SetupStyle().DrawLine(a, b, widthA, widthB, smoothEnding);
        }

        public void DrawText(string text, Rect rect, TextOptions options)
        {
            SetupStyle().DrawText(text, rect, options);
        }

        public void DrawEllipse(Vector2 center, Vector2 size)
        {
            SetupStyle().DrawEllipse(center, size);
        }

        public void DrawRect(Rect rect)
        {
            SetupStyle().DrawRect(rect);
        }

        public void DrawPolygon(Vector2[] points)
        {
            SetupStyle().DrawPolygon(points);
        }

        public Vector2 MeasureText(string text, float fontSize)
        {
            return Graphics.MeasureString(text, CreateFont(fontSize));
        }

        public static Font CreateFont(float fontSize)
        {
            return new Font(SystemFonts.DefaultFont.FontFamily, fontSize, FontStyle.Regular, GraphicsUnit.Pixel);
        }

        public void Clear(Color32 color)
        {
        }
    }
}
