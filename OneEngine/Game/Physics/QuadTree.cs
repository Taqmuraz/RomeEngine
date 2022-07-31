using System.Collections.Generic;

namespace OneEngine
{
	public class QuadTree<T> where T : ILocatable
	{
		public int GetDepth() => depth;
		public Rect GetBounds() => bounds;
		readonly Rect bounds;
		readonly int capacity;
		QuadTree<T> upperRight;
		QuadTree<T> upperLeft;
		QuadTree<T> downRight;
		QuadTree<T> downLeft;
		bool divided;
		readonly List<T> locatables;
		readonly int depthLimit;
		readonly int depth;
		QuadTree<T>[] nodes;

		public QuadTree(Rect bounds, int capacity, int depthLimit, int depth)
		{
			this.capacity = capacity;
			locatables = new List<T>(capacity);
			this.bounds = bounds;
			this.depth = depth;
			this.depthLimit = depthLimit;
		}

		public ReadOnlyArrayList<T> GetLocatables()
		{
			return locatables;
		}
		public ReadOnlyArray<QuadTree<T>> GetNodes()
		{
			return nodes;
		}

		void Subdivide()
		{
			nodes = new QuadTree<T>[4];
			nodes[0] = downLeft = new QuadTree<T>(new Rect(bounds.min, bounds.Center), capacity, depthLimit, depth + 1);
			nodes[1] = downRight = new QuadTree<T>(new Rect(bounds.Bottom, bounds.Right), capacity, depthLimit, depth + 1);
			nodes[2] = upperLeft = new QuadTree<T>(new Rect(bounds.Left, bounds.Top), capacity, depthLimit, depth + 1);
			nodes[3] = upperRight = new QuadTree<T>(new Rect(bounds.Center, bounds.max), capacity, depthLimit, depth + 1);

			for (int i = 0; i < locatables.Count; i++)
			{
				upperRight.AddLocatable(locatables[i]);
				upperLeft.AddLocatable(locatables[i]);
				downRight.AddLocatable(locatables[i]);
				downLeft.AddLocatable(locatables[i]);
			}

			locatables.Clear();

			divided = true;
		}

		public void AddLocatable(T locatable)
		{
			if (!locatable.IntersectsRect(bounds)) return;

			if (divided)
			{
				goto RECURSIVE_CALL;
			}
			else
			{
				if (locatables.Count < capacity || depth == depthLimit)
				{
					locatables.Add(locatable);
				}
				else
				{
					Subdivide();
					goto RECURSIVE_CALL;
				}
			}
			return;

			RECURSIVE_CALL:
			upperRight.AddLocatable(locatable);
			upperLeft.AddLocatable(locatable);
			downRight.AddLocatable(locatable);
			downLeft.AddLocatable(locatable);
		}

		public delegate void TreeRecursiveCheckDelegate(QuadTree<T> node);
		public delegate bool TreeNodeFilter(QuadTree<T> node);

		public void TreeRecursiveCheck(TreeRecursiveCheckDelegate action, TreeNodeFilter filter = null)
		{
			var nodes = GetNodes();
			if (nodes.Length == 0 && (filter == null ? true : filter(this)))
			{
				action(this);
			}
			else
			{
				for (int i = 0; i < nodes.Length; i++) nodes[i].TreeRecursiveCheck(action, filter);
			}
		}
	}
}
