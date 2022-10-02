namespace RomeEngine
{
    public sealed class StaticBody : IPhysicalBody
    {
        public StaticBody(float restitutionCoefficient, float mass)
        {
            RestitutionCoefficient = restitutionCoefficient;
            Mass = mass;
        }

        public void ApplyForceAtPoint(Vector3 point, Vector3 force)
        {

        }

        public Vector3 GetVelocityAtPoint(Vector3 point)
        {
            return new Vector3();
        }

        public float RestitutionCoefficient { get; set; } = 0.5f;
        public float Mass { get; set; } = 1f;

        public void Update()
        {

        }
    }
}