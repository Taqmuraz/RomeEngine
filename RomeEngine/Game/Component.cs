using RomeEngine.IO;
using System.Collections.Generic;
using System.Linq;

namespace RomeEngine
{
    public class Component : SerializableEventsHandler, IInitializable<GameObject>
	{
		[SerializeField(HideInInspector = true)] bool destroyed;

		public string Name => GameObject.Name;
		public virtual bool IsUnary { get; }

		public virtual Transform2D Transform => GetTransform();
		internal protected virtual Transform2D GetTransform()
		{
			return transform;
		}
		[SerializeField(HideInInspector = true)]
		Transform2D transform;
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
    }
}
