using RomeEngine.IO;
using System.Collections.Generic;

namespace RomeEngine
{
    public interface ITransform : IGameEntity
    {
        Matrix4x4 LocalToWorld { get; }

        Vector3 Right { get; }
        Vector3 Up { get; }
        Vector3 Forward { get; }

        Vector3 Position { get; set; }
        Vector3 Rotation { get; set; }

        Vector3 LocalPosition { get; set; }
        Vector3 LocalRotation { get; set; }
        Vector3 LocalScale { get; set; }

        ITransform Parent { get; set; }

        void ApplyMatrix(Matrix4x4 matrix);
    }
}