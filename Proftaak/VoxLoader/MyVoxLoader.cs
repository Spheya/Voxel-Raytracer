using System;
using System.Collections.Generic;
using CsharpVoxReader;
using CsharpVoxReader.Chunks;
using VoxelData;

namespace VoxLoader
{
    public struct PaletteMaterial
    {
        public byte r;
        public byte g;
        public byte b;
        public byte a;

        public float roughness;
        public float metallic;
        public float specular;
    };

    public class MyVoxLoader : IVoxLoader
    {
        public VoxelGrid _data;

        public int Width;
        public int Height;
        public int Depth;

        public PaletteMaterial[] _materials = new PaletteMaterial[255];

        public void LoadModel(int sizeX, int sizeY, int sizeZ, byte[,,] data)
        {
            //throw new NotImplementedException();
            Console.WriteLine($"poggers (model size: {sizeX}; {sizeY}; {sizeZ})");
            _data = new VoxelGrid(sizeX, sizeY, sizeZ, data);
            Width = sizeX; Height = sizeY; Depth = sizeZ;
        }

        public void LoadPalette(UInt32[] palette)
        {
            for (int i = 1; i < 255; i++)
            {
                _materials[i] = new PaletteMaterial();
                palette[i].ToARGB(out _materials[i].a, out _materials[i].r, out _materials[i].g, out _materials[i].b);
            }
            //throw new NotImplementedException();
        }

        public void NewGroupNode(int id, Dictionary<string, byte[]> attributes, int[] childrenIds)
        {
            //throw new NotImplementedException();
        }

        public void NewLayer(int id, Dictionary<string, byte[]> attributes)
        {
            //throw new NotImplementedException();
        }

        public void NewMaterial(int id, Dictionary<string, byte[]> attributes)
        {
            //throw new NotImplementedException();
        }

        public void NewShapeNode(int id, Dictionary<string, byte[]> attributes, int[] modelIds, Dictionary<string, byte[]>[] modelsAttributes)
        {
            //throw new NotImplementedException();
        }

        public void NewTransformNode(int id, int childNodeId, int layerId, Dictionary<string, byte[]>[] framesAttributes)
        {
            //throw new NotImplementedException();
        }

        public void SetMaterialOld(int paletteId, MaterialOld.MaterialTypes type, float weight, MaterialOld.PropertyBits property, float normalized)
        {
            //throw new NotImplementedException();
        }

        public void SetModelCount(int count)
        {
            //throw new NotImplementedException();
        }
    }
}
