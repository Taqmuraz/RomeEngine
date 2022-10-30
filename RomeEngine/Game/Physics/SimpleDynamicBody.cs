namespace RomeEngine
{
    public sealed class SimpleDynamicBody : IPhysicalBody
    {
        Vector3 totalForce;
        ITransform transform;

        public SimpleDynamicBody(ITransform transform)
        {
            this.transform = transform;
        }

        public void ApplyForceAtPoint(Vector3 point, Vector3 force)
        {
            totalForce += force;
        }

        public Vector3 GetVelocityAtPoint(Vector3 point)
        {
            return totalForce;
        }

        public float RestitutionCoefficient { get; set; } = 1f;
        public float Mass { get; set; } = 1f;

        public void Update()
        {
            transform.Position += totalForce * Time.DeltaTime;
            totalForce += Physics.Gravity * Time.DeltaTime;
        }
    }
}