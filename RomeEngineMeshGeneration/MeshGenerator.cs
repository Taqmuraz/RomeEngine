using System;
using RomeEngine;

namespace RomeEngineMeshGeneration
{
    public static class MeshGenerator
    {
        public static IMesh GenerateMesh(IMeshGenerationProvider provider)
        {
            var stream = new MeshStream();
            foreach (var element in provider.Elements)
            {
                element.WriteElement(stream);
            }
            return stream.BuildMesh(provider.Descriptor, provider.Builder);
        }
    }
}
