namespace RomeEngine
{
    public interface IPhysicalBody
    {
        void ApplyForceAtPoint(Vector3 point, Vector3 force);
        void ApplyForce(Vector3 force);
        Vector3 GetVelocityAtPoint(Vector3 point);
        Vector3 Velocity { get; }
        float RestitutionCoefficient { get; }
        float Mass { get; }
        void Update();
    }
}