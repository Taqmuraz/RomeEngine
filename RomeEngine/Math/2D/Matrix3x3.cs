﻿using System;
using System.Drawing.Drawing2D;

namespace RomeEngine
{
	public struct Matrix3x3
	{
		Vector3 row_0;
		Vector3 row_1;
		Vector3 row_2;

		public static readonly Matrix3x3 identity = new Matrix3x3(Vector3.right, Vector3.up, Vector3.forward);

		private Matrix3x3(Vector3 row_0, Vector3 row_1, Vector3 row_2)
		{
			this.row_0 = row_0;
			this.row_1 = row_1;
			this.row_2 = row_2;
		}

		public static Matrix3x3 Viewport(float width, float height)
		{
			width *= 0.5f;
			height *= 0.5f;
			return new Matrix3x3()
			{
				Column_0 = new Vector3(width, 0f, 0f),
				Column_1 = new Vector3(0f, -height, 0f),
				Column_2 = new Vector3(width, height, 1f)
			};
		}

		public static Matrix3x3 WorldTransform(Vector2 right, Vector2 up, Vector2 translate)
		{
			return new Matrix3x3()
			{
				Column_0 = right,
				Column_1 = up,
				Column_2 = new Vector3(translate.x, translate.y, 1f)
			};
		}

		public static Matrix3x3 New(Vector3 column_0, Vector3 column_1, Vector3 column_2)
		{
			return new Matrix3x3()
			{
				Column_0 = column_0,
				Column_1 = column_1,
				Column_2 = column_2
			};
		}

		public Vector3 Column_0
		{
			get => new Vector3(row_0.x, row_1.x, row_2.x);
			set => (row_0.x, row_1.x, row_2.x) = (value.x, value.y, value.z);
		}
		public Vector3 Column_1
		{
			get => new Vector3(row_0.y, row_1.y, row_2.y);
			set => (row_0.y, row_1.y, row_2.y) = (value.x, value.y, value.z);
		}
		public Vector3 Column_2
		{
			get => new Vector3(row_0.z, row_1.z, row_2.z);
			set => (row_0.z, row_1.z, row_2.z) = (value.x, value.y, value.z);
		}

		public Vector3 GetColumn(int index)
		{
			return new Vector3(row_0[index], row_1[index], row_2[index]);
		}
		public void SetColumn(Vector3 column, int index)
		{
            switch (index)
			{
				case 0: Column_0 = column; break;
				case 1: Column_1 = column; break;
				case 2: Column_2 = column; break;
			}
        }

		private Vector3 this[int index]
		{
			get
			{
				switch (index)
				{
					case 0: return row_0;
					case 1: return row_1;
					case 2: return row_2;
					default: throw new System.ArgumentException("Row index out of matrix range");
				}
			}
			set
			{
				switch (index)
				{
					case 0: row_0 = value; break;
					case 1: row_1 = value; break;
					case 2: row_2 = value; break;
					default: throw new System.ArgumentException("Row index out of matrix range");
				}
			}
		}
		public float this[int index, int element]
		{
			get
			{
				return this[index][element];
			}
			set
			{
				switch (index)
				{
					case 0: row_0[element] = value; break;
					case 1: row_1[element] = value; break;
					case 2: row_2[element] = value; break;
					default: throw new System.ArgumentException("Row index out of matrix range");
				}
			}
		}

		public static Matrix3x3 operator *(Matrix3x3 a, float b)
		{
			a.row_0 *= b;
			a.row_1 *= b;
			a.row_2 *= b;
			return a;
		}

		public static Vector3 operator *(Matrix3x3 lhs, Vector3 vector)
		{
			Vector3 res;
			res.x = lhs.row_0.x * vector.x + lhs.row_0.y * vector.y + lhs.row_0.z * vector.z;
			res.y = lhs.row_1.x * vector.x + lhs.row_1.y * vector.y + lhs.row_1.z * vector.z;
			res.z = lhs.row_2.x * vector.x + lhs.row_2.y * vector.y + lhs.row_2.z * vector.z;

			if (res.z != 0)
			{
				float d = 1f / res.z;
				res.x *= d;
				res.y *= d;
				res.z = 0f;
			}

			return res;
		}

		public static Matrix3x3 operator *(Matrix3x3 lhs, Matrix3x3 rhs)
		{
			Matrix3x3 res;
			res.row_0.x = lhs.row_0.x * rhs.row_0.x + lhs.row_0.y * rhs.row_1.x + lhs.row_0.z * rhs.row_2.x;
			res.row_0.y = lhs.row_0.x * rhs.row_0.y + lhs.row_0.y * rhs.row_1.y + lhs.row_0.z * rhs.row_2.y;
			res.row_0.z = lhs.row_0.x * rhs.row_0.z + lhs.row_0.y * rhs.row_1.z + lhs.row_0.z * rhs.row_2.z;

			res.row_1.x = lhs.row_1.x * rhs.row_0.x + lhs.row_1.y * rhs.row_1.x + lhs.row_1.z * rhs.row_2.x;
			res.row_1.y = lhs.row_1.x * rhs.row_0.y + lhs.row_1.y * rhs.row_1.y + lhs.row_1.z * rhs.row_2.y;
			res.row_1.z = lhs.row_1.x * rhs.row_0.z + lhs.row_1.y * rhs.row_1.z + lhs.row_1.z * rhs.row_2.z;

			res.row_2.x = lhs.row_2.x * rhs.row_0.x + lhs.row_2.y * rhs.row_1.x + lhs.row_2.z * rhs.row_2.x;
			res.row_2.y = lhs.row_2.x * rhs.row_0.y + lhs.row_2.y * rhs.row_1.y + lhs.row_2.z * rhs.row_2.y;
			res.row_2.z = lhs.row_2.x * rhs.row_0.z + lhs.row_2.y * rhs.row_1.z + lhs.row_2.z * rhs.row_2.z;

			return res;
		}

		public static bool operator ==(Matrix3x3 a, Matrix3x3 b)
		{
			return a.row_0 == b.row_0 && a.row_1 == b.row_1 && a.row_2 == b.row_2;
		}
		public static bool operator !=(Matrix3x3 a, Matrix3x3 b)
		{
			return a.row_0 != b.row_0 || a.row_1 != b.row_1 || a.row_2 != b.row_2;
		}

		public static Matrix3x3 TransformMatrix(Vector2 position, Vector2 scale, float rotation)
		{
			Matrix3x3 matrix = RotationMatrix(rotation);
			matrix.Column_2 = new Vector3(position.x, position.y, 1f);
			matrix.Column_0 *= scale.x;
			matrix.Column_1 *= scale.y;
			return matrix;
		}
		public static Matrix3x3 CreateScaleMatrix(Vector2 scale)
		{
			return new Matrix3x3
				(
					new Vector3(scale.x, 0f, 0f),
					new Vector3(0f, scale.y, 0f),
					new Vector3(0f, 0f, 1f)
				);
		}
		public static Matrix3x3 RotationMatrix(float rotation)
		{
			float sin = rotation.Sin();
			float cos = rotation.Cos();
			Vector3 right = new Vector3(cos, sin, 0f);
			Vector3 up = new Vector3(-sin, cos, 0f);
			return Matrix3x3.New(right, up, Vector3.forward);
		}
		public static Matrix3x3 TranslationMatrix(Vector2 vector)
		{
			var m = identity;
			m.Column_2 = new Vector3(vector.x, vector.y, 1f);
			return m;
		}

		public float GetDeterminant()
		{
			return row_0.x * row_1.y * row_2.z + row_0.z * row_1.x * row_2.y + row_0.y * row_1.z * row_2.x
				- row_0.z * row_1.y * row_2.x - row_0.x * row_1.z * row_2.y - row_0.y * row_1.x * row_2.z;
		}

		public Matrix3x3 GetTransponed()
		{
			return new Matrix3x3
				(
				new Vector3(row_0.x, row_1.x, row_2.x),
				new Vector3(row_0.y, row_1.y, row_2.y),
				new Vector3(row_0.z, row_1.z, row_2.z)
				);
		}

		public Matrix3x3 GetInversed()
		{
			return GetTransponed().GetAdj() * (1f / GetDeterminant());
		}

		public Matrix3x3 GetAdj()
		{
			Matrix3x3 resoult = new Matrix3x3();
			for (int i_x = 0; i_x < 3; i_x++)
			{
				for (int i_y = 0; i_y < 3; i_y++)
				{
					resoult[i_x, i_y] = (-1f).Pow(i_x + i_y) * MinorStage(i_x, i_y);
				}
			}
			return resoult;
		}
		public Matrix2x2 Minor(int x, int y)
		{
			var m = new Matrix2x2();
			int nx = 0;

			for (int i = 0; i < 3; i++)
			{
				int ny = 0;
				if (i == x) continue;
				for (int j = 0; j < 3; j++)
				{
					if (j == y) continue;
					m[nx, ny] = this[i, j];
					ny++;
				}
				nx++;
			}

			return m;
		}
		private float MinorStage(int x, int y)
		{
			return Minor(x, y).GetDeterminant();
		}

		public Vector2 MultiplyPoint(Vector2 point)
		{
			return (Vector2)(this * new Vector3(point.x, point.y, 1f));
		}
		public Vector2 MultiplyVector(Vector2 vector)
		{
			return (Vector2)(this * vector);
		}
		public Vector2 MultiplyScale(Vector2 scale)
		{
			return new Vector2(Column_0.length * scale.x, Column_1.length * scale.y);
		}
		public Vector2 MultiplyDirection(Vector2 direction)
		{
			return new Matrix3x3()
			{
				Column_0 = this.Column_0.normalized,
				Column_1 = this.Column_1.normalized,
				Column_2 = this.Column_2.normalized,
			}.MultiplyVector(direction);
		}

		public override string ToString()
		{
			return $"{row_0};\n{row_1};\n{row_2};\nDet = {GetDeterminant()}";
		}
	}
}