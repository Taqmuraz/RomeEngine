using RomeEngine;

namespace RomeEngineOpenGL
{
    class OpenGLMeshIdentifier : IMeshIdentifier
    {
        public OpenGLMeshIdentifier(int indicesNumber, int vertexArrrayObjectIndex)
        {
            VertexArrrayObjectIndex = vertexArrrayObjectIndex;
            IndicesNumber = indicesNumber;
        }

        public int VertexArrrayObjectIndex { get; }
        public int IndicesNumber { get; }
    }
}
