namespace RomeEngine
{
    public class Component : SerializableEventsHandler, IInitializable<IGameObject>, IGameEntity
	{
		[SerializeField] bool destroyed;

		public string Name => GameObject.Name;
		public virtual bool IsUnary { get; }

		public virtual ITransform Transform => gameObject.Transform;
		[SerializeField]
		IGameObject gameObject;
		public IGameObject GameObject => gameObject;

		protected override void OnEventCall(string name)
		{
			base.OnEventCall(name);
			if (GameObject == null)
			{
				Debug.LogError("Can't call event on destroyed component");
			}
		}

		void IInitializable<IGameObject>.Initialize(IGameObject arg)
		{
			gameObject = arg;
			gameObject.Activate(this);
		}

		public void Destroy()
		{
			if (!destroyed)
			{
				GameObject.Deactivate(this);
				destroyed = true;
			}
		}

		public override string ToString()
		{
			return $"({GetType().Name}){GameObject.Name}";
		}

        void IGameEntity.Activate(IGameEntityActivityProvider activityProvider)
        {
			activityProvider.Activate(this);
        }

        void IGameEntity.Deactivate(IGameEntityActivityProvider activityProvider)
        {
			activityProvider.Deactivate(this);
        }
    }
}
