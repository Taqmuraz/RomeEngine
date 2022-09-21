namespace RomeEngine
{
	public sealed class CircleCollider2D : Collider2D
	{
		public Vector2 center { get; set; }
		public float radius { get; set; }

		public override Rect GetBounds()
		{
			return Rect.FromCenterAndSize(Transform.TransformPointLocal(center), new Vector2(radius * 2f, radius * 2f));
		}

		protected override bool AddContactsWith(Collider2D collider, Collision2D data)
		{
			if (collider is CircleCollider2D circle) return Intersection_Circle_Circle(this, circle, data);
			else if (collider is BoxCollider2D box) return InterSection_Circle_Box(this, box, data);
			else throw new System.NotImplementedException();
		}

		protected override bool RaycastCollider(Ray2D ray, out RaycastHit2D hit)
		{
			//Vector2 p = transform.TransformPoint(center);
			//Vector2 delta = (p - ray.origin);
			//Vector2 proj = Vector2.ProjectOnPoint(p, ray.origin, ray.direction);
			//bool c = Vector2.Dot(ray.direction, delta) > 0f && delta.length > radius && (proj - p).length < radius;
			bool c = GetBounds().IntersectsRay(ray, out float distance);

			hit = new RaycastHit2D();
			hit.collider = this;
			//hit.distance = proj.length - radius;
			hit.distance = distance;

			return c;
		}
	}
}
