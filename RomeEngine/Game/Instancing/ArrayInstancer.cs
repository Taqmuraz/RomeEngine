using RomeEngine.IO;
using System;
using System.Collections.Generic;

namespace RomeEngine
{
    public sealed class ArrayInstancer : IInstancer
	{
        public bool CanInstantiate(object source)
        {
			if (source == null) return false;

			var type = source.GetType();
			return type.IsArray && typeof(ISerializable).IsAssignableFrom(type.GetElementType());
        }

        public object Instantiate(object source, Dictionary<ISerializable, ISerializable> objectsMap)
        {
			var sourceArray = (Array)source;
			var arrayInstance = Array.CreateInstance(source.GetType().GetElementType(), sourceArray.Length);
            for (int i = 0; i < sourceArray.Length; i++)
            {
				var element = sourceArray.GetValue(i);
				arrayInstance.SetValue(InstantiatableExtensions.GetInstancerForSource(element).Instantiate(element, objectsMap), i);
            }
			return arrayInstance;
        }
    }
}
