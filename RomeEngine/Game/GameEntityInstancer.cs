namespace RomeEngine
{
	public class GameEntityInstancer
	{
		public delegate IGameEntity InstantiateDelegate ();

		InstantiateDelegate instantiateFunc;

		public GameEntityInstancer(InstantiateDelegate instantiateFunc)
		{
			if (instantiateFunc == null) throw new System.ArgumentNullException("instantiateFunc");
			this.instantiateFunc = instantiateFunc;
		}

		public IGameEntity Instantiate()
		{
			return instantiateFunc();
		}
	}
}
