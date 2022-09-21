using System;

namespace RomeEngine
{
    public class Component2D : Component
	{
		[BehaviourEvent]
		void Start()
		{
			if (!(base.GameObject is GameObject2D)) throw new Exception("Conponent2D can be initialized only on GameObject2D");
		}
		public new GameObject2D GameObject => (GameObject2D)base.GameObject;
	}
}
