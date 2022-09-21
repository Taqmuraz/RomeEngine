namespace RomeEngine
{
    public sealed class GameObject3D : GameObject
	{
        public GameObject3D() : base("NewGameObject2D")
        {
        }
        public GameObject3D(string name) : base(name)
        {
        }
        protected override Transform CreateTransform()
        {
			return AddComponent<Transform3D>();
        }
        public override string ToString()
        {
            return $"(GameObject3D){Name}";
        }
        public new Transform3D Transform => (Transform3D)base.Transform;
    }
}
