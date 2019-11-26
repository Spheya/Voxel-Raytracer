using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Engine.Rendering
{
    class Renderer
    {
        private List<VoxelModel> _models = new List<VoxelModel>();

        public void Add(VoxelModel model) => _models.Add(model);

        public bool Remove(VoxelModel model) => _models.Remove(model);

        public bool Contains(VoxelModel model) => _models.Contains(model);

        public void Draw()
        {

        }

    }
}
