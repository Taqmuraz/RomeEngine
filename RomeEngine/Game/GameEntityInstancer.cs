namespace RomeEngine
{
	public class GameEntityInstancer
	{
		public delegate IGameObject InstantiateDelegate ();

		InstantiateDelegate instantiateFunc;

		public GameEntityInstancer(InstantiateDelegate instantiateFunc)
		{
			if (instantiateFunc == null) throw new System.ArgumentNullException("instantiateFunc");
			this.instantiateFunc = instantiateFunc;
		}

		public IGameObject Instantiate()
		{
			return instantiateFunc();
		}
	}
}
