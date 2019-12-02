using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Engine.Shaders;

namespace Game.Engine.Rendering
{
    class MaterialPalette : IEnumerable<Material>
    {
        private readonly Material[] _materials = new Material[256];
        private bool _dirty = false;
        private ShaderProgram _shader;

        public ShaderProgram Shader
        {
            get => _shader;
            set
            {
                _shader = value;
                _dirty = true;
            }
        }

        public Material this[int index]
        {
            get => _materials[index];
            set
            {
                _materials[index] = value;
                _dirty = true;
            }
        }

        public int Count => _materials.Length;

        public MaterialPalette(ShaderProgram shader)
        {
            _shader = shader;
        }

        public MaterialPalette(ShaderProgram shader, IEnumerable<Material> materials) :
            this(shader)
        {
            Set(materials);
        }

        public void Set(IEnumerable<Material> materials)
        {
            int index = 0;
            foreach (var material in materials)
            {
                _materials[index++] = material;

                // Stop adding materials when the maximum is reached
                if (index == 255)
                    break;
            }

            _dirty = true;
        }

        public void Bind(string name)
        {
            if (_dirty)
            {
                for (int i = 0; i < _materials.Length; i++)
                {
                    if (_materials[i] == null)
                        break;

                    _materials[i].Load(_shader, name + '[' + i + ']');
                }

                _dirty = false;
            }
        }

        public IEnumerator<Material> GetEnumerator()
        {
            foreach (var material in _materials)
                yield return material;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _materials.GetEnumerator();
        }
    }
}
