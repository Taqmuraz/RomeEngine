namespace OneEngine
{
	public struct ContactData
	{
		public Vector2 hitPoint { get; set; }
		public Vector2 hitNormal { get; set; }
		public Collider otherCollider { get; set; }
	}
}
