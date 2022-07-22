﻿namespace OneEngine
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
		Transform transform;

		public GameObject GameObject { get; private set; }

		protected override void OnEventCall(string name)
		{
			base.OnEventCall(name);
			if (GameObject == null) throw new System.Exception("Can't call event on destroyed component");
		}

		void IInitializable<GameObject>.Initialize(GameObject arg)
		{
			GameObject = arg;
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
