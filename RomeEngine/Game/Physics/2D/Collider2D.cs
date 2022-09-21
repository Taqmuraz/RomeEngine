using System;
using System.Collections.Generic;

namespace RomeEngine
{

	public abstract class Collider2D : Component, ILocatable2D
	{
		static List<Collider2D> colliders_list = new List<Collider2D>();
		static Dictionary<Collider2D, Collision2D> collisions = new Dictionary<Collider2D, Collision2D>();

		static readonly Collision2D emptyCollision = new Collision2D();
		static QuadTree<Collider2D> lastUpdateTree;

		[BehaviourEvent]
		void Start()
		{
			colliders_list.Add(this);
		}
		[BehaviourEvent]
		void OnDestroy()
		{
			colliders_list.Remove(this);
		}

		static void TreeCollisionCheck(QuadTree<Collider2D> node)
		{
			var colliders = node.GetLocatables();

			for (int i = 0; i < colliders.Count; i++)
			{
				Collision2D collision = new Collision2D();
				var collider = colliders[i];

				for (int j = i + 1; j < colliders.Count; j++)
				{
					var other = colliders[j];

					if (collider.GameObject != other.GameObject && collider.GetBounds().IntersectsWith(other.GetBounds()))
					{
						collider.AddContactsWith(other, collision);
					}
				}

				if (collision.contancsCount > 0)
				{
					if (collisions.ContainsKey(collider))
					{
						var cl = collisions[collider];
						for (int c = 0; c < collision.contancsCount; c++) cl.AddContact(collision[c]);
					}
					else collisions.Add(collider, collision);
				}
			}
		}

		public static void UpdatePhysics()
		{
			int collidersCount = colliders_list.Count;
			collisions.Clear();

			Rect treeArea = new Rect();

			for (int i = 0; i < collidersCount; i++)
			{
				treeArea = i == 0 ? colliders_list[i].GetBounds() : treeArea.Spread(colliders_list[i].GetBounds());
			}

			QuadTree<Collider2D> tree = new QuadTree<Collider2D>(treeArea, 2, 6, 0);
			lastUpdateTree = tree;

			for (int i = 0; i < collidersCount; i++)
			{
				tree.AddLocatable(colliders_list[i]);
			}
			tree.TreeRecursiveCheck(TreeCollisionCheck);
		}

		public abstract Rect GetBounds();

		public bool HasCollision(out Collision2D data)
		{
			if (collisions.ContainsKey(this))
			{
				data = collisions[this];
				return true;
			}
			else
			{
				data = emptyCollision;
				return false;
			}
		}

		protected static bool Intersection_Circle_Circle(CircleCollider2D a, CircleCollider2D b, Collision2D data)
		{
			float minDist2 = a.radius + b.radius;
			minDist2 *= minDist2;
			Vector2 dist = a.Transform.TransformPointLocal(a.center) - b.Transform.TransformPointLocal(b.center);
			Vector2 dist2 = dist * dist;

			if (minDist2 >= (dist2.x + dist2.y))
			{
				var contact = new ContactData2D();
				contact.hitNormal = dist;
				contact.hitPoint = b.Transform.TransformPointLocal(contact.hitNormal * b.radius);
				data.AddContact(contact);
				return true;
			}
			else return false;
		}
		protected static bool Intersection_Box_Circle(BoxCollider2D box, CircleCollider2D circle, Collision2D data)
		{
			var boxCenter = box.Transform.TransformPointLocal(box.center);
			var circleCenter = circle.Transform.TransformPointLocal(circle.center);

			Vector2 local_circle_center = circleCenter - boxCenter;
			local_circle_center = new Vector2(Vector2.Dot(box.Transform.LocalRight, local_circle_center), Vector2.Dot(box.Transform.LocalUp, local_circle_center));

			Vector2 diff = local_circle_center;
			Vector2 hSize = box.size * 0.5f;
			Vector2.Clamp(ref diff, -hSize, hSize);

			if ((local_circle_center - diff).length < circle.radius)
			{
				float diffAxis = Mathf.Round(diff.ToAngle() / 90f) * 90f;

				var contact = new ContactData2D();
				contact.hitPoint = box.Transform.TransformPointLocal(diff);
				contact.hitNormal = box.Transform.TransformVectorLocal(-new Vector2(Mathf.Cos(diffAxis), Mathf.Sin(diffAxis)));
				data.AddContact(contact);
				return true;
			}
			else return false;
		}

		protected static bool InterSection_Circle_Box(CircleCollider2D circle, BoxCollider2D box, Collision2D data)
		{
			int contacts_index = data.contancsCount;
			bool collision = Intersection_Box_Circle(box, circle, data);
			if (collision)
			{
				for (int i = contacts_index; i < data.contancsCount; i++)
				{
					var contact = data[i];
					contact.hitNormal = -contact.hitNormal;
					data[i] = contact;
				}
			}
			return collision;
		}

		protected static bool Intersection_Box_Box(BoxCollider2D a, BoxCollider2D b, Collision2D data)
		{
			return false;
			//throw new System.NotImplementedException();
		}

		public static bool Raycast(Ray2D ray, out RaycastHit2D hit)
		{
			bool check = false;
			RaycastHit2D outHit = new RaycastHit2D();
			outHit.distance = float.MaxValue;
			lastUpdateTree.TreeRecursiveCheck(node => TreeRaycastCheck(ray, node, ref check, ref outHit), tree =>
			{
				var bounds = tree.GetBounds();
				return bounds.Contains(ray.origin) || bounds.IntersectsRay(ray, out float dist);
			});
			hit = outHit;
			return check;
		}
		static void TreeRaycastCheck(Ray2D ray, QuadTree<Collider2D> node, ref bool check, ref RaycastHit2D hit)
		{
			float min = hit.distance;
			var colliders = node.GetLocatables();

			//Debug.DrawBox(node.GetBounds(), Color32.green);

			for (int i = 0; i < colliders.Count; i++)
			{
				var collider = colliders[i];
				RaycastHit2D cHit;
				if (collider.RaycastCollider(ray, out cHit) && cHit.distance < min)
				{
					hit = cHit;
					min = cHit.distance;
					check = true;
					//Debug.DrawBox(collider.GetBounds(), Color32.green);
				}// else Debug.DrawBox(collider.GetBounds(), Color32.red);
			}
		}

		protected abstract bool RaycastCollider(Ray2D ray, out RaycastHit2D hit);

		protected abstract bool AddContactsWith(Collider2D collider, Collision2D data);

		bool ILocatable2D.IntersectsRect(Rect rect)
		{
			return GetBounds().IntersectsWith(rect);
		}
	}
}
