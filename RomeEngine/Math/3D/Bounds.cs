using System;
using System.Collections.Generic;

namespace RomeEngine
{
    public struct Bounds
	{
		Vector3 center;
		Vector3 size;

        public Bounds(Vector3 center, Vector3 size)
        {
            this.center = center;
            this.size = ProcessSize(size);
        }

		static Vector3 ProcessSize(Vector3 size)
		{
			return size.Max(new Vector3(0.01f, 0.01f, 0.01f));
		}

        public Vector3 Center
        {
			get => center;
			set => center = value;
        }
		public Vector3 Size
		{
			get => size;
			set => size = value;
		}
		public Vector3 Max => center + size * 0.5f;
		public Vector3 Min => center - size * 0.5f;

		public static Bounds FromMinMax(Vector3 min, Vector3 max)
		{
			return new Bounds((min + max) * 0.5f, max - min);
		}
		public static Bounds FromMinSize(Vector3 min, Vector3 size)
		{
			return new Bounds(min + size * 0.5f, size);
		}
		public static Bounds FromPoints(IEnumerable<Vector3> points)
		{
			Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);
			bool initialized = false;
			foreach (var point in points)
			{
				if (!initialized)
				{
					bounds = new Bounds(point, Vector3.zero);
					initialized = true;
				}
				else
				{
					bounds = bounds.Incapsulate(point);
				}
			}
			return bounds;
		}
		public static Bounds FromBoxes(IEnumerable<Bounds> boxes)
		{
			Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);
			bool initialized = false;
			foreach (var box in boxes)
			{
				if (!initialized)
				{
					bounds = box;
					initialized = true;
				}
				else
				{
					bounds = FromMinMax(bounds.Min.Min(box.Min), bounds.Max.Max(box.Max));
				}
			}
			return bounds;
		}
		public Bounds Incapsulate(Vector3 point)
		{
			Vector3 min = point.Min(Min);
			Vector3 max = point.Max(Max);
			return FromMinMax(min, max);
		}
		public bool IntersectsWith(Bounds bounds)
		{
			Vector3 clampMin = Min.Clamp(bounds.Min, bounds.Max);
			Vector3 clampMax = Max.Clamp(bounds.Min, bounds.Max);
			Vector3 size = clampMax - clampMin;
			return size.x * size.y * size.z > Mathf.Epsilon;
		}
		public bool ContainsPoint(Vector3 point)
		{
			Vector3 min = Min;
			Vector3 max = Max;
			return point.x < max.x && point.x >= min.x
				&& point.y < max.y && point.y >= min.y
				&& point.z < max.z && point.z >= min.z;
		}

		static Vector3[] rotationBuffer = new Vector3[3];
		public Bounds Rotate(Vector3 euler)
		{
			lock (rotationBuffer)
			{
				var matrix = Matrix4x4.CreateRotationMatrix(euler);
				for (int i = 0; i < 3; i++)
				{
					rotationBuffer[i] = center + ((Vector3)matrix.GetColumn(i)) * size[i] * 0.5f;
				}
				return FromPoints(rotationBuffer);
			}
		}

		void ClampRay(Ray ray, int axis, ref float rMin, ref float rMax)
		{
			float r0 = (Min[axis] - ray.origin[axis]) / ray.direction[axis];
			float r1 = (Max[axis] - ray.origin[axis]) / ray.direction[axis];
			rMin = Mathf.Max(rMin, Mathf.Min(r0, r1));
			rMax = Mathf.Min(rMax, Mathf.Max(r0, r1));
		}

        public bool IntersectsRay(Ray ray)
        {
			if (ContainsPoint(ray.origin)) return true;
			if (Vector3.Dot(ray.direction, (center - ray.origin).Normalized) < -0.75f) return false;

			float rMin = float.NegativeInfinity;
			float rMax = float.PositiveInfinity;
			Vector3 min = Min;
			Vector3 max = Max;

            for (int i = 0; i < 3; i++)
            {
				if (ray.direction[i] != 0f) ClampRay(ray, i, ref rMin, ref rMax);
				else if (!ray.origin[i].InRange(min[i], max[i])) return false;
            }

			return rMax >= rMin;
        }

		static Vector3[][] boxPoints = new Vector3[][]
		{
			new Vector3[] // left
			{
				new Vector3(0, 0, 0),
				new Vector3(0, 0, 1),
				new Vector3(0, 1, 1),
				new Vector3(0, 1, 0),
			},
			new Vector3[] // right
			{
				new Vector3(1, 0, 0),
				new Vector3(1, 0, 1),
				new Vector3(1, 1, 1),
				new Vector3(1, 1, 0),
			},
			new Vector3[] // down
			{
				new Vector3(0, 0, 0),
				new Vector3(1, 0, 0),
				new Vector3(1, 0, 1),
				new Vector3(0, 0, 1),
			},
			new Vector3[] // up
			{
				new Vector3(0, 1, 0),
				new Vector3(1, 1, 0),
				new Vector3(1, 1, 1),
				new Vector3(0, 1, 1),
			},
			new Vector3[] // back
			{
				new Vector3(0, 0, 0),
				new Vector3(1, 0, 0),
				new Vector3(1, 1, 0),
				new Vector3(0, 1, 0),
			},
			new Vector3[] // forward
			{
				new Vector3(0, 0, 1),
				new Vector3(1, 0, 1),
				new Vector3(1, 1, 1),
				new Vector3(0, 1, 1),
			},
		};

		bool CheckNormal(Ray ray, int index, Vector3 normal)
		{
			if (Vector3.Dot(ray.direction, normal) >= 0f) return false;

			Matrix3x3 matrixA = new Matrix3x3();
			Matrix3x3 matrixB = new Matrix3x3();
			Vector3 min = Min;
            for (int i = 0; i < 3; i++)
            {
				matrixA.SetColumn((boxPoints[index][i] * size + min - ray.origin).Normalized, i);
				matrixB.SetColumn((boxPoints[index][(i + 2) % 4] * size + min - ray.origin).Normalized, i);
			}
			Vector3 triangleDir = matrixA.GetInversed() * ray.direction;
			if (triangleDir.x >= 0f && triangleDir.y >= 0f && triangleDir.z >= 0f) return true;
			triangleDir = matrixB.GetInversed() * ray.direction;
			if (triangleDir.x >= 0f && triangleDir.y >= 0f && triangleDir.z >= 0f) return true;

			return false;
		}

		public bool GetNormalForRay(Ray ray, out Vector3 normal)
		{
			for (int i = 0; i < 3; i++)
			{
				normal = new Vector3() { [i] = -1f };
				if (CheckNormal(ray, i * 2, normal)) return true;
				normal = new Vector3() { [i] = 1f };
				if (CheckNormal(ray, i * 2 + 1, normal)) return true;
			}
			normal = new Vector3();
			return false;
		}
    }
}