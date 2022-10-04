using RomeEngine.IO;

namespace RomeEngine
{
    public interface ISourceObject : ISerializable
	{
		ISerializable CloneSourceReference();
	}
}
