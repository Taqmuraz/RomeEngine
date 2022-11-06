namespace RomeEngineOpenGL
{
    sealed class OpenGLShaderReplaceMacro : IOpenGLShaderPostProcessor
    {
        string value;
        string replaceTo;

        public OpenGLShaderReplaceMacro(string value, string replaceTo)
        {
            this.value = value;
            this.replaceTo = replaceTo;
        }

        public string Process(string program)
        {
            return program.Replace(value, replaceTo);
        }
    }
}
