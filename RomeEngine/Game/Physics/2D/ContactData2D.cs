namespace RomeEngine
{
	public struct ContactData2D
	{
		public Vector2 hitPoint { get; set; }
		public Vector2 hitNormal { get; set; }
		public Collider2D otherCollider { get; set; }
	}
}
