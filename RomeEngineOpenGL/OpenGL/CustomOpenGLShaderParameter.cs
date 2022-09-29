using System;

namespace RomeEngineOpenGL
{
    sealed class CustomOpenGLShaderParameter : IOpenGLShaderParameter
    {
        Action<OpenGLShader> parameterSetter;

        public CustomOpenGLShaderParameter(Action<OpenGLShader> parameterSetter)
        {
            this.parameterSetter = parameterSetter;
        }

        public void Setup(OpenGLShader shader)
        {
            parameterSetter(shader);
        }
    }
}
