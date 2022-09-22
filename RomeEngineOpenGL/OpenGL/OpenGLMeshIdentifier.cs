using RomeEngine;

namespace RomeEngineOpenGL
{
    class OpenGLMeshIdentifier : IMeshIdentifier
    {
        public OpenGLMeshIdentifier(int verticesNumber, int vertexArrrayObjectIndex)
        {
            VertexArrrayObjectIndex = vertexArrrayObjectIndex;
            VerticesNumber = verticesNumber;
        }

        public int VertexArrrayObjectIndex { get; }
        public int VerticesNumber { get; }
    }
}
