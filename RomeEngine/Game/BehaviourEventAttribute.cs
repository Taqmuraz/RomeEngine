using System;

namespace RomeEngine
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
	public sealed class BehaviourEventAttribute : Attribute
	{
	}
}