namespace RomeEngine
{
    public interface IInstantiatable<TInstance>
	{
		TInstance CreateInstance();
	}
}
