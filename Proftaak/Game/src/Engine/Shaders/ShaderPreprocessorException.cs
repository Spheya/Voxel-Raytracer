using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.src.Engine.Shaders
{
    class ShaderPreprocessorException : Exception
    {
        public ShaderPreprocessorException(string error) :
            base("Error when preprocessing the shader!\nError message:\n" + error)
        { }
    }
}
