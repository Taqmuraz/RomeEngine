using RomeEngine.IO;
using System.Collections.Generic;

namespace RomeEngine
{
    public sealed class EmptyInstancer : IInstancer
	{
		public bool CanInstantiate(object source) => true;
		public object Instantiate(object source, Dictionary<ISerializable, ISerializable> objectsMap) => source;
    }
}
