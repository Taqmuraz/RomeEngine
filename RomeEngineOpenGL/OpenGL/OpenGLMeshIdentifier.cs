using RomeEngine;

namespace RomeEngineOpenGL
{
    class OpenGLMeshIdentifier : IMeshIdentifier
    {
        public OpenGLMeshIdentifier(int indicesNumber, int vertexArrrayObjectIndex, int[] vertexBufferObjectIndices, IMesh sourceMesh)
        {
            VertexArrrayObjectIndex = vertexArrrayObjectIndex;
            this.VertexBufferObjectIndices = vertexBufferObjectIndices;
            IndicesNumber = indicesNumber;
            SourceMesh = sourceMesh;
        }

        public int VertexArrrayObjectIndex { get; }
        public int[] VertexBufferObjectIndices { get; }
        public int IndicesNumber { get; }
        public IMesh SourceMesh { get; }

        public bool Released { get; set; }
    }
}
