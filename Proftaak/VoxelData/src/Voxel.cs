using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxelData
{
    public struct Voxel
    {
        public static readonly Voxel EMPTY = new Voxel(0);

        public ushort materialId;

        public Voxel(ushort materialId)
        {
            this.materialId = materialId;
        }
    }
}
