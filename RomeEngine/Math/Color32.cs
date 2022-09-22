using System;

namespace RomeEngine
{
	public struct Color32
	{
		public byte r, g, b, a;

		public Color32(byte r, byte g, byte b, byte a)
		{
			this.r = r;
			this.g = g;
			this.b = b;
			this.a = a;
		}
		public Color32(float r, float g, float b, float a)
		{
			this.r = (byte)(r * 255f);
			this.g = (byte)(g * 255f);
			this.b = (byte)(b * 255f);
			this.a = (byte)(a * 255f);
		}
		public Color32(int argb)
		{
			a = (byte)((argb >> 24) & 255);
			r = (byte)((argb >> 16) & 255);
			g = (byte)((argb >> 8) & 255);
			b = (byte)(argb & 255);
		}
		public Color32(int rgb, byte alpha)
		{
			a = alpha;
			r = (byte)((rgb >> 16) & 255);
			g = (byte)((rgb >> 8) & 255);
			b = (byte)(rgb & 255);
		}

		public static readonly Color32 red = new Color32(255, 0, 0, 255);
		public static readonly Color32 green = new Color32(0, 255, 0, 255);
		public static readonly Color32 blue = new Color32(0, 0, 255, 255);
		public static readonly Color32 white = new Color32(255, 255, 255, 255);
        public static readonly Color32 black = new Color32(0, 0, 0, 255);
        public static readonly Color32 gray = new Color32(127, 127, 127, 255);

		public int Argb => (a << 24) | (r << 16) | (g << 8) | b;

		public Color32 Negative => new Color32(~Argb, a);

        public static Color32 operator * (Color32 a, float b)
		{
			return new Color32((byte)(a.r * b), (byte)(a.g * b), (byte)(a.b * b), a.a);
		}
		public static Color32 operator *(Color32 a, Color32 b)
		{
			return new Color32((float)a.r * b.r, (float)a.g * b.g, (float)a.b * b.b, (float)a.a * b.a);
		}
		public static Color32 operator +(Color32 a, Color32 b)
		{
			return new Color32(a.r + b.r, a.g + b.g, a.b + b.b, a.a + b.a);
		}

		public static implicit operator System.Drawing.Color (Color32 color)
		{
			return System.Drawing.Color.FromArgb (color.a, color.r, color.g, color.b);
		}
		public static implicit operator Color32(System.Drawing.Color color)
		{
			return new Color32(color.R, color.G, color.B, color.A);
		}

        public Color32 WithAlpha(byte alpha)
        {
			return new Color32(r, g, b, alpha);
        }

        public Vector4 ToVector4()
        {
			float d = 1f / 255f;
			return new Vector4(r * d, g * d, b * d, a * d);
        }
    }
}