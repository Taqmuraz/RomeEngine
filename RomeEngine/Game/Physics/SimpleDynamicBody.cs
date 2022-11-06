namespace RomeEngine
{
    public sealed class SimpleDynamicBody : IPhysicalBody
    {
        Vector3 forces;
        Vector3 Forces
        {
            get
            {
                lock (forceLock) return forces;
            }
            set
            {
                lock (forceLock) forces = value;
            }
        }
        object forceLock = new object();
        ITransform transform;

        public SimpleDynamicBody(ITransform transform)
        {
            this.transform = transform;
        }

        public void ApplyForceAtPoint(Vector3 point, Vector3 force)
        {
           Forces += force;
        }

        public Vector3 GetVelocityAtPoint(Vector3 point)
        {
            return Velocity;
        }

        public float Mass { get; set; } = 1f;

        public void FixedUpdate()
        {
            transform.Position += Velocity * PhysicalEntity.PhysicsDeltaTime;
            Forces += Physics.Gravity * PhysicalEntity.PhysicsDeltaTime;
        }

        public void ApplyForce(Vector3 force)
        {
            Forces += force;
        }

        public Vector3 Velocity => Forces / Mass;
    }
}