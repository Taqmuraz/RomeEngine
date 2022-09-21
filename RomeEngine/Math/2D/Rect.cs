using System;

namespace RomeEngine
{
	public struct Rect
	{
		public Vector2 min;
		public Vector2 max;

		public Rect(float x, float y, float width, float height)
		{
			min = new Vector2(x, y);
			max = new Vector2(x + width, y + height);
		}

		public Rect(Vector2 min, Vector2 max)
		{
			this.min = Vector2.Min(min, max);
			this.max = Vector2.Max(min, max);
		}

		public float Volume => Mathf.Abs(Size.x * Size.y);

		public static Rect FromCenterAndSize(Vector2 center, Vector2 size)
		{
			return new Rect(center - size * 0.5f, center + size * 0.5f);
		}
		public static Rect FromLocationAndSize(Vector2 location, Vector2 size)
		{
			return new Rect(location, location + size);
		}
		public static Rect FromLocationAndSize(float x, float y, float width, float height)
		{
			return new Rect(new Vector2(x, y), new Vector2(x + width, y + height));
		}

		public void SplitHorizontal(out Rect left, out Rect right)
		{
			left = FromLocationAndSize(min, new Vector2(Width * 0.5f, Height));
			right = FromLocationAndSize(min + new Vector2(Width * 0.5f, 0f), new Vector2(Width * 0.5f, Height));
		}
		public void SplitVertical(out Rect up, out Rect down)
		{
			up = FromLocationAndSize(min, new Vector2(Width, Height * 0.5f));
			down = FromLocationAndSize(min + new Vector2(0f, Height * 0.5f), new Vector2(Width, Height * 0.5f));
		}

		public override string ToString()
		{
			return $"({min.x}, {min.y}, {max.x - min.x}, {max.y - min.y})";
		}

		public Vector2 Size => max - min;
		public Vector2 Center => (min + max) * 0.5f;
		public Vector2 UpperLeft => new Vector2(min.x, max.y);
		public Vector2 DownRight => new Vector2(max.x, min.y);
		public Vector2 Bottom => new Vector2((min.x + max.x) * 0.5f, min.y);
		public Vector2 Top => new Vector2((min.x + max.x) * 0.5f, max.y);
		public Vector2 Right => new Vector2(max.x, (min.y + max.y) * 0.5f);
		public Vector2 Left => new Vector2(min.x, (min.y + max.y) * 0.5f);

        public float Width => max.x - min.x;
        public float Height => max.y - min.y;

        public float X => min.x;
        public float Y => min.y;

        public bool IntersectsWith(Rect rect)
		{
			Rect r = this;
			Vector2.Clamp(ref r.min, rect.min, rect.max);
			Vector2.Clamp(ref r.max, rect.min, rect.max);
			Vector2 area = r.Size;
			return area.x * area.y != 0f;
		}

		public Rect Spread(Rect rect)
		{
			Vector2 nMin = new Vector2();
			Vector2 nMax = new Vector2();
			nMin.x = Mathf.Min(min.x, rect.min.x);
			nMin.y = Mathf.Min(min.y, rect.min.y);
			nMax.x = Mathf.Max(max.x, rect.max.x);
			nMax.y = Mathf.Max(max.y, rect.max.y);
			return new Rect(nMin, nMax);
		}
		public Rect Spread(Vector2 point)
		{
			return new Rect(Vector2.Min(min, point), Vector2.Max(max, point));
		}

		public static implicit operator System.Drawing.RectangleF(Rect rect)
		{
			return new System.Drawing.RectangleF(rect.min.x, rect.min.y, rect.Size.x, rect.Size.y);
		}
		public static explicit operator System.Drawing.Rectangle(Rect rect)
		{
			return new System.Drawing.Rectangle((int)rect.min.x, (int)rect.min.y, (int)rect.Size.x, (int)rect.Size.y);
		}
		public static implicit operator Rect(System.Drawing.RectangleF rectangle)
		{
			return new Rect(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
		}

		static Vector2[] nonAllocVertices = new Vector2[4];
		public bool IntersectsRay(Ray2D ray, out float distance)
		{
			distance = (Center - ray.origin).length;

			Matrix3x3 rayMatrix = Matrix3x3.New(ray.direction, new Vector2(-ray.direction.y, ray.direction.x), ray.origin).GetInversed();
			bool negative = false, positive = false;
			lock (nonAllocVertices)
			{
				nonAllocVertices[0] = min;
				nonAllocVertices[1] = max;
				nonAllocVertices[2] = UpperLeft;
				nonAllocVertices[3] = DownRight;

				for (int i = 0; i < 4; i++)
				{
					Vector2 v = rayMatrix.MultiplyPoint(nonAllocVertices[i]);
					if (v.x < 0) return false;
					if (v.y < 0) negative = true;
					else positive = true;
				}
			}
			return negative && positive;
		}

		public bool Contains(Vector2 point)
		{
			return point.x <= max.x && point.x >= min.x && point.y <= max.y && point.y >= min.y;
		}
	}
}