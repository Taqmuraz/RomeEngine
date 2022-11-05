namespace RomeEngine
{
    public sealed class SimpleDynamicBody : IPhysicalBody
    {
        Vector3 velocity;
        ITransform transform;

        public SimpleDynamicBody(ITransform transform)
        {
            this.transform = transform;
        }

        public void ApplyForceAtPoint(Vector3 point, Vector3 force)
        {
            velocity += force / Mass;
        }

        public Vector3 GetVelocityAtPoint(Vector3 point)
        {
            return velocity;
        }

        public float Mass { get; set; } = 1f;

        public void Update()
        {
            transform.Position += velocity * Time.DeltaTime;
            velocity += Physics.Gravity * Time.DeltaTime;
        }

        public void ApplyForce(Vector3 force)
        {
            velocity += force / Mass;
        }

        public Vector3 Velocity => velocity;
    }
}