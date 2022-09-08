using System;
using System.Collections.Generic;

namespace RomeEngine
{

	public abstract class Collider : Component, ILocatable
	{
		static List<Collider> colliders_list = new List<Collider>();
		static Dictionary<Collider, Collision> collisions = new Dictionary<Collider, Collision>();

		static readonly Collision emptyCollision = new Collision();
		static QuadTree<Collider> lastUpdateTree;

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

		static void TreeCollisionCheck(QuadTree<Collider> node)
		{
			var colliders = node.GetLocatables();

			for (int i = 0; i < colliders.Count; i++)
			{
				Collision collision = new Collision();
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

			QuadTree<Collider> tree = new QuadTree<Collider>(treeArea, 2, 6, 0);
			lastUpdateTree = tree;

			for (int i = 0; i < collidersCount; i++)
			{
				tree.AddLocatable(colliders_list[i]);
			}
			tree.TreeRecursiveCheck(TreeCollisionCheck);
		}

		public abstract Rect GetBounds();

		public bool HasCollision(out Collision data)
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

		protected static bool Intersection_Circle_Circle(CircleCollider a, CircleCollider b, Collision data)
		{
			float minDist2 = a.radius + b.radius;
			minDist2 *= minDist2;
			Vector2 dist = a.Transform.TransformPointLocal(a.center) - b.Transform.TransformPointLocal(b.center);
			Vector2 dist2 = dist * dist;

			if (minDist2 >= (dist2.x + dist2.y))
			{
				var contact = new ContactData();
				contact.hitNormal = dist;
				contact.hitPoint = b.Transform.TransformPointLocal(contact.hitNormal * b.radius);
				data.AddContact(contact);
				return true;
			}
			else return false;
		}
		protected static bool Intersection_Box_Circle(BoxCollider box, CircleCollider circle, Collision data)
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

				var contact = new ContactData();
				contact.hitPoint = box.Transform.TransformPointLocal(diff);
				contact.hitNormal = box.Transform.TransformVectorLocal(-new Vector2(Mathf.Cos(diffAxis), Mathf.Sin(diffAxis)));
				data.AddContact(contact);
				return true;
			}
			else return false;
		}

		protected static bool InterSection_Circle_Box(CircleCollider circle, BoxCollider box, Collision data)
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

		protected static bool Intersection_Box_Box(BoxCollider a, BoxCollider b, Collision data)
		{
			return false;
			//throw new System.NotImplementedException();
		}

		public static bool Raycast(Ray ray, out RaycastHit hit)
		{
			bool check = false;
			RaycastHit outHit = new RaycastHit();
			outHit.distance = float.MaxValue;
			lastUpdateTree.TreeRecursiveCheck(node => TreeRaycastCheck(ray, node, ref check, ref outHit), tree =>
			{
				var bounds = tree.GetBounds();
				return bounds.Contains(ray.origin) || bounds.IntersectsRay(ray, out float dist);
			});
			hit = outHit;
			return check;
		}
		static void TreeRaycastCheck(Ray ray, QuadTree<Collider> node, ref bool check, ref RaycastHit hit)
		{
			float min = hit.distance;
			var colliders = node.GetLocatables();

			//Debug.DrawBox(node.GetBounds(), Color32.green);

			for (int i = 0; i < colliders.Count; i++)
			{
				var collider = colliders[i];
				RaycastHit cHit;
				if (collider.RaycastCollider(ray, out cHit) && cHit.distance < min)
				{
					hit = cHit;
					min = cHit.distance;
					check = true;
					//Debug.DrawBox(collider.GetBounds(), Color32.green);
				}// else Debug.DrawBox(collider.GetBounds(), Color32.red);
			}
		}

		protected abstract bool RaycastCollider(Ray ray, out RaycastHit hit);

		protected abstract bool AddContactsWith(Collider collider, Collision data);

		bool ILocatable.IntersectsRect(Rect rect)
		{
			return GetBounds().IntersectsWith(rect);
		}
	}
}
