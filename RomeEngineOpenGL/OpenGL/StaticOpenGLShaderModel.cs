using RomeEngine;
using System.Collections.Generic;

namespace RomeEngineOpenGL
{
    class StaticOpenGLShaderModel : OpenGLShaderModel
    {
        Matrix4x4 model, view, projection;

        public StaticOpenGLShaderModel(Matrix4x4 model, Matrix4x4 view, Matrix4x4 projection)
        {
            this.model = model;
            this.view = view;
            this.projection = projection;
        }

        protected override IEnumerable<IOpenGLShaderParameter> CreateParameters()
        {
            yield return new CustomOpenGLShaderParameter(shader => shader.SetMatrix("transformationMatrix", model));
            yield return new CustomOpenGLShaderParameter(shader => shader.SetMatrix("viewMatrix", view.GetInversed()));
            yield return new CustomOpenGLShaderParameter(shader => shader.SetMatrix("projectionMatrix", projection));
            yield return new CustomOpenGLShaderParameter(shader => shader.SetVector3("lightDirection", GlobalLight.LightDirection));
            yield return new CustomOpenGLShaderParameter(shader => shader.SetVector4("lightColor", GlobalLight.LightColor.ToVector4()));
            yield return new CustomOpenGLShaderParameter(shader => shader.SetFloat("ambienceIntencivity", GlobalLight.AmbienceIntencivity));
            yield return new CustomOpenGLShaderParameter(shader => shader.SetFloat("time", Time.CurrentTime));
        }
    }
}
