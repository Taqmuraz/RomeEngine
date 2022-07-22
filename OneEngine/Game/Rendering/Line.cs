namespace OneEngine
{
    public class Line
    {
        public Line(Vector2 pointA, Vector2 pointB, Color32 color, float widthA = 0.1f, float widthB = 0.1f)
        {
            PointA = pointA;
            PointB = pointB;
            Color = color;
            WidthA = widthA;
            WidthB = widthB;
        }

        public Vector2 PointA { get; set; }
        public Vector2 PointB { get; set; }
        public float WidthA { get; set; }
        public float WidthB { get; set; }
        public Color32 Color { get; set; }
    }
}