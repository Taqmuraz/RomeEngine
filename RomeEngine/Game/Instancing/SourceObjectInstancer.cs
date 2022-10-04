using RomeEngine.IO;
using System.Collections.Generic;

namespace RomeEngine
{
    public sealed class SourceObjectInstancer : IInstancer
	{
		public bool CanInstantiate(object source) => source is ISourceObject;
		public object Instantiate(object source, Dictionary<ISerializable, ISerializable> objectsMap)
		{
			var newReference = ((ISourceObject)source).CloneSourceReference();
			if (newReference != source) objectsMap.Add((ISerializable)source, newReference);
			return newReference;
		}
	}
}
