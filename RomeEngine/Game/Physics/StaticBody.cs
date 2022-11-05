namespace RomeEngine
{
    public sealed class StaticBody : IPhysicalBody
    {
        public StaticBody(float mass)
        {
            Mass = mass;
        }

        public void ApplyForceAtPoint(Vector3 point, Vector3 force)
        {

        }
        public void ApplyForce(Vector3 force)
        {
        }

        public Vector3 Velocity => new Vector3();

        public Vector3 GetVelocityAtPoint(Vector3 point)
        {
            return new Vector3();
        }

        public float Mass { get; set; } = 1f;

        public void Update()
        {

        }
    }
}