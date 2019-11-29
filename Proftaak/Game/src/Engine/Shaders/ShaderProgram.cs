using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace Game.Engine.Shaders
{
    class ShaderProgram
    {
        private readonly int _program;
        private readonly Dictionary<string, int> _uniforms = new Dictionary<string, int>();

        public ShaderProgram(IEnumerable<Shader> shaders)
        {
            var enumerable = shaders as Shader[] ?? shaders.ToArray();
            
            // Create program and link shaders
            _program = GL.CreateProgram();
            foreach(var shader in enumerable)
                GL.AttachShader(_program, shader.Id);

            // Link the code
            GL.LinkProgram(_program);
            string info = GL.GetProgramInfoLog(_program);
            if (!string.IsNullOrWhiteSpace(info))
                throw new ShaderLinkException(info);

            // Detach the shaders, as we already compiled them
            foreach (var shader in enumerable)
                GL.DetachShader(_program, shader.Id);
        }

        public int GetUniformLocation(string uniformName)
        {
            if (_uniforms.ContainsKey(uniformName))
                return _uniforms[uniformName];

            int uniformLocation = GL.GetUniformLocation(_program, uniformName);
            _uniforms.Add(uniformName, uniformLocation);
            return uniformLocation;
        }

        public void Bind()
        {
            GL.UseProgram(_program);
        }

        public void Unbind()
        {
            GL.UseProgram(0);
        }

        ~ShaderProgram()
        {
            GLGarbageCollector.AddProgram(_program);
        }

    }
}
