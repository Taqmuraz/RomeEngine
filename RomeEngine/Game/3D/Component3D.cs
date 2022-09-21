using System;

namespace RomeEngine
{
    public class Component3D : Component
	{
		[BehaviourEvent]
		void Start()
		{
			if (!(base.GameObject is GameObject3D)) throw new Exception("Conponent3D can be initialized only on GameObject3D");
		}
		public new GameObject3D GameObject => (GameObject3D)base.GameObject;
	}
}
