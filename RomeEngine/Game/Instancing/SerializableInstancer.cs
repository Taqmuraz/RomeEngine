using RomeEngine.IO;
using System.Collections.Generic;

namespace RomeEngine
{
    public sealed class SerializableInstancer : IInstancer
	{
		public bool CanInstantiate(object source) => source is ISerializable;
		public object Instantiate(object source, Dictionary<ISerializable, ISerializable> objectsMap)
		{
			var serializableSource = (ISerializable)source;
			if (objectsMap.TryGetValue(serializableSource, out var existValue))
			{
				return existValue;
			}
			else
			{
				return serializableSource.CreateSerializableInstance(objectsMap);
			}
		}
	}
}
