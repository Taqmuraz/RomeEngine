using System;

namespace OneEngine
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
	public sealed class BehaviourEventAttribute : Attribute
	{
	}
}