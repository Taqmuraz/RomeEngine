using OneEngine.IO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OneEngine
{
    public class Component : BehaviourEventsHandler, IInitializable<GameObject>
	{
		bool destroyed;
		static int id = 0;
		int localID;

		public string Name => GameObject.Name;
		public virtual bool IsUnary { get; }

		public virtual Transform Transform => GetTransform();
		internal protected virtual Transform GetTransform()
		{
			return transform;
		}
		[SerializeField]
		Transform transform;
		[SerializeField]
		GameObject gameObject;
		public GameObject GameObject => gameObject;

		protected override void OnEventCall(string name)
		{
			base.OnEventCall(name);
			if (GameObject == null) throw new Exception("Can't call event on destroyed component");
		}

		void IInitializable<GameObject>.Initialize(GameObject arg)
		{
			gameObject = arg;
			transform = GameObject.Transform;
			localID = id++;
		}

		public void Destroy()
		{
			if (!destroyed)
			{
				GameObject.RemoveComponent(this);
				destroyed = true;
			}
		}

		public override string ToString()
		{
			return GameObject.Name + "_" + localID;
		}
    }
}
