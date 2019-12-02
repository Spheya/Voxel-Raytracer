using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Engine.Shaders;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace Game.Engine.Rendering
{
    class Material
    {
        public Vector3 BaseColour { get; set; }
        public float RefractiveIndex { get; set; }

        public Material(Vector3 baseColour, float refractiveIndex = 1.1f)
        {
            BaseColour = baseColour;
            RefractiveIndex = refractiveIndex;
        }
        public void Load(ShaderProgram shader, string name)
        {
            GL.Uniform3(shader.GetUniformLocation(name + ".baseColour"), BaseColour);
            GL.Uniform1(shader.GetUniformLocation(name + ".refractiveIndex"), RefractiveIndex);
        }
    }
}
