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
            this.size = size;
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
			Bounds bounds = new Bounds();
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
			Bounds bounds = new Bounds();
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

		Vector3 PointOnAxis(Vector3 origin, Vector3 direction, float axisValue, int axis)
		{
			float length = Mathf.Abs(axisValue - origin[axis]);
			return origin + direction * length;
		}

        public bool IntersectsRay(Ray ray)
        {
			Vector3 max = Max;
			Vector3 min = Min;
			Vector3 minP = ray.origin;
			Vector3 maxP = ray.origin;

			for (int i = 0; i < 3; i++)
            {
				minP = PointOnAxis(minP, ray.direction, min[i], i);
				maxP = PointOnAxis(maxP, ray.direction, max[i], i);
			}

			return (minP - maxP).length > Mathf.Epsilon;
        }
    }
}