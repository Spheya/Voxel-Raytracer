using System;
using System.Collections.Generic;
using CsharpVoxReader;
using CsharpVoxReader.Chunks;
using VoxelData;

namespace VoxLoader
{
    public class MyVoxLoader : IVoxLoader
    {
        public VoxelGrid _data;

        public void LoadModel(int sizeX, int sizeY, int sizeZ, byte[,,] data)
        {
            //throw new NotImplementedException();
            Console.WriteLine($"poggers (model size: {sizeX}; {sizeY}; {sizeZ})");
            _data = new VoxelGrid(sizeX, sizeY, sizeZ, data);
        }

        public void LoadPalette(uint[] palette)
        {
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
