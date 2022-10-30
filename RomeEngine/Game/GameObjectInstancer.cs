namespace RomeEngine
{
	public class GameObjectInstancer
	{
		public delegate IGameObject InstantiateDelegate ();

		InstantiateDelegate instantiateFunc;

		public GameObjectInstancer(InstantiateDelegate instantiateFunc)
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
