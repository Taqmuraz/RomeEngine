using RomeEngine.IO;
using System.Collections.Generic;
using System.Linq;

namespace RomeEngine
{
    public class Component : SerializableEventsHandler, IInitializable<GameObject>, IInstantiatable<Component>
	{
		[SerializeField(HideInInspector = true)] bool destroyed;

		public string Name => GameObject.Name;
		public virtual bool IsUnary { get; }

		public virtual Transform Transform => GetTransform();
		internal protected virtual Transform GetTransform()
		{
			return transform;
		}
		[SerializeField(HideInInspector = true)]
		Transform transform;
		[SerializeField(HideInInspector = true)]
		GameObject gameObject;
		public GameObject GameObject => gameObject;

		protected override void OnEventCall(string name)
		{
			base.OnEventCall(name);
			if (GameObject == null)
			{
				Debug.LogError("Can't call event on destroyed component");
			}
		}

		void IInitializable<GameObject>.Initialize(GameObject arg)
		{
			gameObject = arg;
			transform = GameObject.Transform;
		}

		[BehaviourEvent]
		void OnDestroy()
		{
			destroyed = true;
		}

		public void Destroy()
		{
			if (!destroyed)
			{
				GameObject.RemoveComponent(this);
			}
		}

		public override string ToString()
		{
			return $"({GetType().Name}){GameObject.Name}";
		}

        Component IInstantiatable<Component>.CreateInstance()
        {
			var newInstance = (Component)GetType().GetConstructors().First(c => c.GetParameters().Length != 0).Invoke(new object[0]);
			var fieldsMap = newInstance.EnumerateFields().ToDictionary(f => f.Name);
			foreach (var field in EnumerateFields())
			{
				fieldsMap[field.Name].Setter(field.Value);
			}
			return newInstance;
        }
    }
}
