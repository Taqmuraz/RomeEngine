﻿namespace RomeEngine
{
	public struct Vector4
	{
		public float x, y, z, w;

		public Vector4(float x, float y, float z, float w)
		{
			this.x = x;
			this.y = y;
			this.z = z;
			this.w = w;
		}

		public Vector4(Vector3 vector, float w)
		{
			x = vector.x;
			y = vector.y;
			z = vector.z;
			this.w = w;
		}

		public Vector4 WithX(float x) => new Vector4(x, y, z, w);
		public Vector4 WithY(float y) => new Vector4(x, y, z, w);
		public Vector4 WithZ(float z) => new Vector4(x, y, z, w);
		public Vector4 WithW(float w) => new Vector4(x, y, z, w);

		public static Vector4 operator +(Vector4 a, Vector4 b)
		{
			return new Vector4(a.x + b.x, a.y + b.y, a.z + b.z, b.w + a.w);
		}
		public static Vector4 operator -(Vector4 a, Vector4 b)
		{
			return new Vector4(a.x - b.x, a.y - b.y, a.z - b.z, a.w - b.w);
		}
		public static Vector4 operator *(Vector4 a, float b)
		{
			return new Vector4(a.x * b, a.y * b, a.z * b, a.w * b);
		}
		public static Vector4 operator /(Vector4 a, float b)
		{
			return new Vector4(a.x / b, a.y / b, a.z / b, a.w / b);
		}

		public static Vector4 operator *(Vector4 a, Vector4 b)
		{
			return new Vector4(a.x * b.x, a.y * b.y, a.z * b.z, a.w * b.w);
		}
		public static Vector4 operator /(Vector4 a, Vector4 b)
		{
			return new Vector4(a.x / b.x, a.y / b.y, a.z / b.z, a.w / b.w);
		}
		public static float Dot(Vector4 a, Vector4 b)
		{
			return a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;
		}
		public override string ToString()
		{
			return string.Format("{0} | {1} | {2} | {3}", x.ToString("F3"), y.ToString("F3"), z.ToString("F3"), w.ToString("F3"));
		}
		public static bool operator == (Vector4 a, Vector4 b)
		{
			return (a - b).length == 0;
		}
		public static bool operator !=(Vector4 a, Vector4 b)
		{
			return (a - b).length != 0;
		}

		public static implicit operator System.Drawing.PointF (Vector4 v)
		{
			return new System.Drawing.PointF (v.x, v.y);
		}

		public static explicit operator Vector2(Vector4 v)
		{
			return new Vector2(v.x, v.y);
		}

		public Vector3 ToVector3WithWDevision()
		{
			if (w == 0) return new Vector3();
			float dev = 1f / w;
			return new Vector3(x * dev, y * dev, z * dev);
		}

		public static Vector4 Lerp(Vector4 a, Vector4 b, float t)
		{
			return a + (b - a) * t.Clamp(0f, 1f);
		}
		public float length
		{
			get
			{
				return Mathf.Sqrt(x * x + y * y + z * z + w * w);
			}
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
		public override bool Equals(object obj)
		{
			if (obj is Vector4 v)
			{
				return v == this;
			}
			return false;
		}

		public float this[int index]
		{
			get
			{
				switch (index)
				{
					case 0: return x;
					case 1: return y;
					case 2: return z;
					case 3: return w;
					default: return 0;
				}
			}
			set
			{
				switch (index)
				{
					case 0: x = value; break;
					case 1: y = value; break;
					case 2: z = value; break;
					case 3: w = value; break;
					default: break;
				}
			}
		}
	}
}