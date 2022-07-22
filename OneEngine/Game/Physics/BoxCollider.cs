namespace OneEngine
{
    public sealed class BoxCollider : Collider
	{
		public Vector2 size { get; set; } = Vector2.one;
		public Vector2 center { get; set; }

		public override Rect GetBounds()
		{
			Vector2 upperRight = Transform.TransformPoint(center + new Vector2(size.x * 0.5f, size.y * 0.5f));
			Vector2 upperLeft = Transform.TransformPoint(center + new Vector2(-size.x * 0.5f, size.y * 0.5f));
			Vector2 downRight = Transform.TransformPoint(center + new Vector2(size.x * 0.5f, -size.y * 0.5f));
			Vector2 downLeft = Transform.TransformPoint(center + new Vector2(-size.x * 0.5f, -size.y * 0.5f));

			Vector2 uMin = Vector2.Min(upperLeft, upperRight);
			Vector2 dMin = Vector2.Min(downLeft, downRight);
			Vector2 uMax = Vector2.Max(upperLeft, upperRight);
			Vector2 dMax = Vector2.Max(downLeft, downRight);

			return new Rect(Vector2.Min(uMin, dMin), Vector2.Max(uMax, dMax));
		}

		protected override bool AddContactsWith(Collider collider, Collision data)
		{
			if (collider is CircleCollider circle) return Intersection_Box_Circle(this, circle, data);
			else if (collider is BoxCollider box) return Intersection_Box_Box(this, box, data);
			else throw new System.NotImplementedException();
		}

		protected override bool RaycastCollider(Ray ray, out RaycastHit hit)
		{
			Matrix3x3 boxMatrix = Matrix3x3.WorldTransform(new Vector2(size.x, 0f), new Vector2(0f, size.y), center) * Transform.LocalToWorld;
			boxMatrix = boxMatrix.GetInversed();
			bool c = new Rect(0f, 0f, 1f, 1f).IntersectsRay(new Ray(boxMatrix.MultiplyPoint(ray.origin), boxMatrix.MultiplyVector(ray.direction)), out float dist);

			Debug.DrawRay(ray.origin, ray.direction * 100f, Color32.green);
			
			hit = new RaycastHit();
			hit.collider = this;
			hit.distance = dist;
			return c;
		}
	}
}
