using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Engine.Shaders
{
    class ShaderCompileException : Exception
    {
        public ShaderCompileException(string error) : 
            base("Error when compiling the shader!\nError message:\n" + error)
        {}
    }
}
