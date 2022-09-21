namespace RomeEngine
{
	public sealed class RoomCollider2D : Collider2D
	{
		public override Rect GetBounds()
		{
			throw new System.NotImplementedException();
		}

		protected override bool RaycastCollider(Ray2D ray, out RaycastHit2D hit)
		{
			throw new System.NotImplementedException();
		}

		protected override bool AddContactsWith(Collider2D collider, Collision2D data)
		{
			throw new System.NotImplementedException();
		}
	}
}
