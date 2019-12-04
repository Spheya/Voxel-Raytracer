using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Engine.Rendering
{
    public struct DirectionalLight
    {
        public Vector3 direction;
        public float intensity;
        public Vector3 colour;
    }

    public struct PointLight
    {
        public Vector3 position;
        public float intensity;
        public Vector3 colour;
    }
}
