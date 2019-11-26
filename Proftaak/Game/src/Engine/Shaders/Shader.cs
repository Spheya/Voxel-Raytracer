using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace Game.Engine.Shaders
{
    class Shader
    {
        public int Id { get; }

        public Shader(ShaderType type, string source)
        {
            Id = GL.CreateShader(type);
            GL.ShaderSource(Id, source);
            GL.CompileShader(Id);
            var info = GL.GetShaderInfoLog(Id);
            if (!string.IsNullOrWhiteSpace(info))
                throw new ShaderCompileException(info);
        }

        ~Shader()
        {
            GL.DeleteShader(Id);
        }
    }
}
