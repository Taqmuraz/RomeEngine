namespace OneEngine
{
	public sealed class CircleCollider : Collider
	{
		public Vector2 center { get; set; }
		public float radius { get; set; }

		public override Rect GetBounds()
		{
			return Rect.FromCenterAndSize(transform.TransformPoint(center), new Vector2(radius * 2f, radius * 2f));
		}

		protected override bool AddContactsWith(Collider collider, Collision data)
		{
			if (collider is CircleCollider circle) return Intersection_Circle_Circle(this, circle, data);
			else if (collider is BoxCollider box) return InterSection_Circle_Box(this, box, data);
			else throw new System.NotImplementedException();
		}

		protected override bool RaycastCollider(Ray ray, out RaycastHit hit)
		{
			//Vector2 p = transform.TransformPoint(center);
			//Vector2 delta = (p - ray.origin);
			//Vector2 proj = Vector2.ProjectOnPoint(p, ray.origin, ray.direction);
			//bool c = Vector2.Dot(ray.direction, delta) > 0f && delta.length > radius && (proj - p).length < radius;
			bool c = GetBounds().IntersectsRay(ray, out float distance);

			hit = new RaycastHit();
			hit.collider = this;
			//hit.distance = proj.length - radius;
			hit.distance = distance;

			return c;
		}
	}
}
