using RomeEngine.IO;
using System.Collections.Generic;

namespace RomeEngine
{
    public interface IInstancer
	{
		bool CanInstantiate(object source);
		object Instantiate(object source, Dictionary<ISerializable, ISerializable> objectsMap);
	}
}
