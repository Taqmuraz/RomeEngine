using System.Collections.Generic;
using System.Linq;

namespace OneEngine
{
	public sealed class GameObject : Game.GameThreadHandler
	{
		[SerializeField]
		public Transform Transform { get; private set; }
		[SerializeField]
		public string Name { get; set; }

		public GameObject()
		{
			componentSearch = components.Concat(inOrderToAdd).Except(inOrderToRemove);
			GameScene.activeScene.AddGameObject(this);
		}

		public GameObject(string name) : this()
		{
			Name = name;

			Transform = AddComponent<Transform>();
		}

		[SerializeField] List<Component> components = new List<Component>();
		[SerializeField] List<Component> inOrderToAdd = new List<Component>();
		[SerializeField] List<Component> inOrderToRemove = new List<Component>();

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
