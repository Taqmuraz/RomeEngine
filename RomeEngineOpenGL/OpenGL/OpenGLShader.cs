using OpenTK.Graphics.OpenGL;
using RomeEngine;
using System.Linq;

namespace RomeEngineOpenGL
{
    class OpenGLShader
    {
        private int programID;
        private int vertexShaderID;
        private int fragmentShaderID;

        static IOpenGLShaderPostProcessor[] ShaderPostProcessors { get; }

        static OpenGLShader()
        {
            ShaderPostProcessors = new IOpenGLShaderPostProcessor[]
            {
                new OpenGLShaderMacro($"#define JOINT_VEC vec{SkinnedMesh.MaxJointsSupported}"),
                new OpenGLShaderMacro("#version 140"),
            };
        }

        public int GetProgramID()
        {
            return programID;
        }

        public void Start()
        {
            GL.UseProgram(programID);
        }
        public void Stop()
        {
            GL.UseProgram(0);
        }

        public const int MATRIX_SIZE = 16;

        private static float[] matrixBuffer = new float[MATRIX_SIZE];

        public OpenGLShader(string directoryName) : this
            (
            $"./Resources/Shaders/{directoryName}/vertexShader.glsl",
            $"./Resources/Shaders/{directoryName}/fragmentShader.glsl"
            )
        {
        }

        public OpenGLShader(string vertexFile, string fragmentFile)
        {
            vertexShaderID = LoadShader(vertexFile, ShaderType.VertexShader);
            fragmentShaderID = LoadShader(fragmentFile, ShaderType.FragmentShader);
            programID = GL.CreateProgram();
            GL.AttachShader(programID, vertexShaderID);
            GL.AttachShader(programID, fragmentShaderID);
            BindAttributes();
            GL.LinkProgram(programID);
            GL.ValidateProgram(programID);
        }
        SafeDictionary<string, int> uniforms = new SafeDictionary<string, int>();

        public int GetUniformLocation(string uniformName)
        {
            if (string.IsNullOrEmpty(uniformName)) throw new System.ArgumentException("uniformName is null or empty");
            else if (uniforms.ContainsKey(uniformName)) return uniforms[uniformName];
            else
            {
                int location = GL.GetUniformLocation(programID, uniformName);
                uniforms.Add(uniformName, location);
                return location;
            }
        }

        protected void LoadFloat(int location, float value)
        {
            GL.Uniform1(location, value);
        }

        protected void LoadVector3(int location, Vector3 vector)
        {
            GL.Uniform3(location, vector.x, vector.y, vector.z);
        }
        protected void LoadVector4(int location, Vector4 vector)
        {
            GL.Uniform4(location, vector.x, vector.y, vector.z, vector.w);
        }

        protected void LoadVector2(int location, Vector2 vector)
        {
            GL.Uniform2(location, vector.x, vector.y);
        }

        protected void LoadBoolean(int location, bool value)
        {
            float toLoad = value ? 1f : 0f;
            GL.Uniform1(location, toLoad);
        }

        protected void LoadMatrix(int location, Matrix4x4 matrix)
        {
            for (int i = 0; i < 16; i++) matrixBuffer[i] = matrix[i % 4, i / 4];
            GL.UniformMatrix4(location, 1, false, matrixBuffer);
        }
        protected void LoadMatrixArray(int location, Matrix4x4[] matrices)
        {
            float[] array = new float[matrices.Length * MATRIX_SIZE];

            for (int i = 0; i < matrices.Length; i++) matrices[i].ToFloatArray(array, i * MATRIX_SIZE);

            GL.UniformMatrix4(location, matrices.Length, false, array);
        }

        public void SetFloat(string name, float value)
        {
            LoadFloat(GetUniformLocation(name), value);
        }
        public void SetVector2(string name, Vector2 vector)
        {
            LoadVector2(GetUniformLocation(name), vector);
        }
        public void SetVector3(string name, Vector3 vector)
        {
            LoadVector3(GetUniformLocation(name), vector);
        }
        public void SetVector4(string name, Vector4 vector)
        {
            LoadVector4(GetUniformLocation(name), vector);
        }
        public void SetBool(string name, bool value)
        {
            LoadBoolean(GetUniformLocation(name), value);
        }
        public void SetMatrix(string name, Matrix4x4 matrix)
        {
            LoadMatrix(GetUniformLocation(name), matrix);
        }
        public void SetMatrixArray(string name, Matrix4x4[] array)
        {
            LoadMatrixArray(GetUniformLocation(name), array);
        }

        protected virtual void BindAttributes()
        {
            BindAttribute(0, "position");
            BindAttribute(1, "textureCoords");
            BindAttribute(2, "normal");
        }

        protected void BindAttribute(int attribute, string variableName)
        {
            GL.BindAttribLocation(programID, attribute, variableName);
        }
        private static int LoadShader(string file, ShaderType type)
        {
            System.Text.StringBuilder shaderSource = new System.Text.StringBuilder();

            using (System.IO.StreamReader reader = new System.IO.StreamReader(file, true))
            {
                string line;
                while (!reader.EndOfStream)
                {
                    line = reader.ReadLine();
                    shaderSource.Append(line).Append("\n");
                }
            }

            var shaderCode = shaderSource.ToString();

            foreach (var postprocessor in ShaderPostProcessors)
            {
                shaderCode = postprocessor.Process(shaderCode);
            }

            int shaderID = GL.CreateShader(type);
            GL.ShaderSource(shaderID, shaderCode);
            GL.CompileShader(shaderID);
            GL.GetShader(shaderID, ShaderParameter.CompileStatus, out int p);
            if (p == 0)
            {
                GL.GetShaderInfoLog(shaderID, 500, out int l, out string info);
                throw new System.Exception($"Shader compile error : {info}, message length = {l}, file = {file}");
            }

            return shaderID;
        }
    }
}
