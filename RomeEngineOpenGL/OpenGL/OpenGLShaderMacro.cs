namespace RomeEngineOpenGL
{
    sealed class OpenGLShaderMacro : IOpenGLShaderPostProcessor
    {
        string macro;

        public OpenGLShaderMacro(string macro)
        {
            this.macro = macro;
        }

        public string Process(string program)
        {
            return $"{macro}\n{program}";
        }
    }
}
