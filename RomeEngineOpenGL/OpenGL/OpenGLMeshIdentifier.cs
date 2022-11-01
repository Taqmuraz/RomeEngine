using RomeEngine;

namespace RomeEngineOpenGL
{
    class OpenGLMeshIdentifier : IMeshIdentifier
    {
        public OpenGLMeshIdentifier(int indicesNumber, int vertexArrrayObjectIndex, IMesh sourceMesh)
        {
            VertexArrrayObjectIndex = vertexArrrayObjectIndex;
            IndicesNumber = indicesNumber;
            SourceMesh = sourceMesh;
        }

        public int VertexArrrayObjectIndex { get; }
        public int[] VertexBufferObjectIndices { get; }
        public int IndicesNumber { get; }
        public IMesh SourceMesh { get; }
    }
}
