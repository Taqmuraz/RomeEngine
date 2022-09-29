using System.Collections.Generic;

namespace RomeEngineOpenGL
{
    abstract class OpenGLShaderModel : IOpenGLShaderModel
    {
        protected abstract IEnumerable<IOpenGLShaderParameter> CreateParameters();
        public void SetupShader(OpenGLShader shader)
        {
            foreach (var parameter in CreateParameters())
            {
                parameter.Setup(shader);
            }
        }
    }
}
