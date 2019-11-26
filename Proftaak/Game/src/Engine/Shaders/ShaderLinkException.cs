using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Engine.Shaders
{
    class ShaderLinkException : Exception
    {
        public ShaderLinkException(string error) :
            base("Error when linking the shader!\nError log:\n" + error)
        { }
    }
}
