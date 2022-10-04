using RomeEngine.IO;
using System;
using System.Collections;
using System.Collections.Generic;

namespace RomeEngine
{
    public sealed class ArrayListInstancer : IInstancer
	{
		public bool CanInstantiate(object source)
		{
			if (source == null) return false;

			var type = source.GetType();
			return typeof(IList).IsAssignableFrom(type) && type.IsGenericType && typeof(ISerializable).IsAssignableFrom(type.GetGenericArguments()[0]);
		}

        public object Instantiate(object source, Dictionary<ISerializable, ISerializable> objectsMap)
        {
			var sourceType = source.GetType();
			var sourceList = (IList)source;
			var instance = (IList)Activator.CreateInstance(sourceType);
			foreach (var sourceElement in sourceList)
			{
				instance.Add(InstantiatableExtensions.GetInstancerForSource(sourceElement).Instantiate(sourceElement, objectsMap));
			}
			return instance;
        }
    }
}
