﻿using RomeEngine.IO;
using System.Collections.Generic;

namespace RomeEngine
{
    public class Line : ISerializable
    {
        public Line()
        {
        }
        public Line(Vector2 pointA, Vector2 pointB, Color32 color, float widthA = 0.1f, float widthB = 0.1f)
        {
            PointA = pointA;
            PointB = pointB;
            Color = color;
            WidthA = widthA;
            WidthB = widthB;
        }

        public Vector2 PointA { get; set; }
        public Vector2 PointB { get; set; } = Vector2.right;
        public float WidthA { get; set; } = 0.1f;
        public float WidthB { get; set; } = 0.1f;
        public Color32 Color { get; set; } = Color32.white;

        public IEnumerable<SerializableField> EnumerateFields()
        {
            yield return new SerializableField(nameof(PointA), PointA, obj => PointA = (Vector2)obj, typeof(Vector2));
            yield return new SerializableField(nameof(PointB), PointB, obj => PointB = (Vector2)obj, typeof(Vector2));
            yield return new SerializableField(nameof(WidthA), WidthA, obj => WidthA = (float)obj, typeof(float));
            yield return new SerializableField(nameof(WidthB), WidthB, obj => WidthB = (float)obj, typeof(float));
            yield return new SerializableField(nameof(Color), Color, obj => Color = (Color32)obj, typeof(Color32));
        }
    }
}