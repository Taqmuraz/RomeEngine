using RomeEngine.IO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RomeEngine
{

    public sealed class GameObject : Game.GameThreadHandler, IInstantiatable<GameObject>
	{
		[SerializeField(HideInInspector = true)]
		public Transform Transform { get; private set; }
		[SerializeField]
		public string Name { get; set; }
		[SerializeField]
		public int Layer { get; set; }

		[Obsolete("Zero argument GameObject constructor only might be used in serialization", true)]
		public GameObject()
		{

		}

		public GameObject(string name)
		{
			Name = name;
			Transform = AddComponent<Transform>();
			GameScene.ActiveScene.AddGameObject(this);
		}

		public static GameObject CreateInactive(string name)
		{
			var result = (GameObject)Activator.CreateInstance(typeof(GameObject));
			result.Transform = result.AddComponent<Transform>();
			result.Name = name;
			return result;
		}

		[BehaviourEvent]
		void OnDestroy()
		{
			GameScene.ActiveScene.RemoveGameObject(this);
		}

		[SerializeField(HideInInspector = true)] DynamicLinkedList<Component> components = new DynamicLinkedList<Component>();

		public override string ToString()
		{
			return $"(GameObject){Name}";
		}

        public Component AddComponent(Type type)
        {
            var constructor = type.GetConstructors().FirstOrDefault(c => c.GetParameters().Length == 0);
            if (constructor == null) throw new System.InvalidOperationException($"Component of type {type.FullName} does not have zero-argument constructor");
            var component = (Component)constructor.Invoke(new object[0]);

            if (component.IsUnary && GetComponent(type) != null) throw new System.ArgumentException("GameObject cant have two unary Components of the same type");

            ((IInitializable<GameObject>)component).Initialize(this);
            components.Add(component);
			if (IsActive)
			{
				component.CallEvent("Start");
			}
            return component;
        }

		public TComponent AddComponent<TComponent>() where TComponent : Component, IInitializable<GameObject>, new()
		{
            return (TComponent)AddComponent(typeof(TComponent));
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
			components.Remove(component);
		}

		public TComponent GetComponent<TComponent> ()
		{
			foreach (var component in components)
			{
				if (component is TComponent t) return t;
			}
			return default;
		}
        public Component GetComponent(Type type)
        {
            foreach (var component in components)
            {
                if (component.GetType() == type) return component;
            }
            return default;
        }

		public Component[] GetComponents()
		{
			return components.ToArray();
		}

		public TComponent[] GetComponentsOfType<TComponent>()
		{
			return components.Where(c => c is TComponent).Select(c => (TComponent)(object)c).ToArray();
		}

        protected sealed override void OnEventCall(string name)
		{
			foreach (var component in GetComponents())
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
			foreach (var child in Transform.Children)
			{
				child.GameObject.CallEvent(name);
			}
		}

        GameObject IInstantiatable<GameObject>.CreateInstance()
        {
			Dictionary<ISerializable, ISerializable> objectsMap = new Dictionary<ISerializable, ISerializable>();
			var instance = (GameObject)Activator.CreateInstance(GetType());
			objectsMap.Add(this, instance);
			var components = GetComponents().Select(c => (Component)c.CreateSerializableInstance(objectsMap)).ToList();
			instance.components.AddRange(components);
			instance.Transform = (Transform)components.First(c => c is Transform);
			instance.Name = $"{Name}_Copy";
			instance.CallEvent("Start");
			GameScene.ActiveScene.AddGameObject(instance);
			return instance;
        }

		public static GameObject Instantiate(GameObject source)
		{
			return ((IInstantiatable<GameObject>)source).CreateInstance();
		}
    }
}
