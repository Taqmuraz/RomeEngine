namespace RomeEngine
{
    public sealed class GameObject2D : GameObject
	{
        public GameObject2D() : base("NewGameObject2D")
        {
        }
        public GameObject2D(string name) : base(name)
        {
        }

        protected override Transform CreateTransform()
        {
			return AddComponent<Transform2D>();
        }
        public override string ToString()
        {
            return $"(GameObject2D){Name}";
        }
        public new Transform2D Transform => (Transform2D)base.Transform;
    }
}
