using System.Collections.Generic;
using System.Linq;

namespace OneEngine
{
	public sealed class GameObject : Game.GameThreadHandler
	{
		public Transform Transform { get; private set; }

		public string Name { get; set; }

		public GameObject(string name)
		{
			this.Name = name;
			componentSearch = components.Concat(inOrderToAdd).Except(inOrderToRemove);

			Transform = AddComponent<Transform>();
			GameScene.activeScene.AddGameObject(this);
		}

		List<Component> components = new List<Component>();
		List<Component> inOrderToAdd = new List<Component>();
		List<Component> inOrderToRemove = new List<Component>();

		IEnumerable<Component> componentSearch;

		public override string ToString()
		{
			return Name;
		}

		public TComponent AddComponent<TComponent>() where TComponent : Component, IInitializable<GameObject>, new()
		{
			var component = new TComponent();

			if (component.IsUnary && GetComponent<TComponent>() != null) throw new System.ArgumentException("GameObject cant have two unary Components of the same type");

			component.Initialize(this);
			inOrderToAdd.Add(component);
			component.CallEvent("Start");
			return component;
		}
		public void RemoveComponent(Component component)
		{
			try
			{
				component.CallEvent("OnDestroy");
			}
			catch (System.Exception ex)
			{
				Debug.Log(ex);
			}
			inOrderToRemove.Add(component);
		}

		[BehaviourEvent]
		void Update()
		{
			components.AddRange(inOrderToAdd);
			inOrderToAdd.Clear();
			for (int i = 0; i < inOrderToRemove.Count; i++) components.Remove(inOrderToRemove[i]);
			inOrderToRemove.Clear();
		}

		public TComponent GetComponent<TComponent> ()
		{
			foreach (var component in componentSearch)
			{
				if (component is TComponent t) return t;
			}
			return default;
		}

		protected sealed override void OnEventCall(string name)
		{
			foreach (var component in components)
			{
				try
				{
					component.CallEvent(name);
				}
				catch (System.Exception ex)
				{
					Debug.Log(ex.ToString());
				}
			}
		}
	}
}
