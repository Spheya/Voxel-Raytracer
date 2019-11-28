using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Engine.Maths;
using Game.Engine.Shaders;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace Game.Engine.Rendering
{
    class Renderer
    {
        private readonly Model _canvas;

        private readonly List<VoxelModel> _models = new List<VoxelModel>();
        private readonly BufferTexture<ushort> _voxelData = new BufferTexture<ushort>(SizedInternalFormat.R16ui);
        private readonly BufferTexture<int> _modelData = new BufferTexture<int>(SizedInternalFormat.Rgba32i);
        public ShaderProgram Shader { get; set; }

        public Renderer(ShaderProgram shader)
        {
            _canvas = new Model(new[]{
                -1.0f, -1.0f,
                1.0f, -1.0f,
                1.0f,  1.0f,
                -1.0f,  1.0f
            }, 2, PrimitiveType.TriangleFan);

            _modelData.AddRange(new int[] { 0,0,0,0 });

            Shader = shader;
        }

        public VoxelModel CreateModel(int width, int height, int depth, Transform transform)
        {
            VoxelModel model = new VoxelModel(_voxelData, _voxelData.Count, width, height, depth, transform);
            _models.Add(model);
            _modelData[0] = (ushort) _models.Count;
            _modelData.AddRange(new [] { width, height, depth, model.Offset });
            _voxelData.AddRange(new ushort[model.Footprint]);

            return model;
        }

        public VoxelModel CreateModel(int width, int height, int depth)
        {
            VoxelModel model = new VoxelModel(_voxelData, _voxelData.Count, width, height, depth);
            _models.Add(model);
            _modelData[0] = (ushort)_models.Count;
            _modelData.AddRange(new [] { 32, 32, 32, model.Offset });
            _voxelData.AddRange(new ushort[model.Footprint]);

            return model;
        }

        public bool Remove(VoxelModel model)
        {
            int index = _models.FindIndex(0, m => m == model);

            if (!_models.Remove(model))
                return false;

            _voxelData.Erase(model.Offset, model.Footprint);
            _modelData.Erase((index + 1) * 4, 4);

            for (int i = index; i < _models.Count; i++)
                _models[i].Offset -= model.Footprint;

            return true;
        }

        public void Draw(GameWindow window)
        {
            _voxelData.Update();
            _modelData.Update();

            Shader.Bind();

            GL.BindVertexArray(_canvas.Vao);

            _voxelData.Bind(TextureUnit.Texture0);
            GL.Uniform1(Shader.GetUniformLocation("u_voxelBuffer"), 1, new[] { 0 });
            _modelData.Bind(TextureUnit.Texture1);
            GL.Uniform1(Shader.GetUniformLocation("u_modelData"), 1, new[] { 1 });

            GL.Uniform3(Shader.GetUniformLocation("u_bufferDimensions"), 1, new[] { _models[0].Width, _models[0].Height, _models[0].Depth });
            GL.Uniform2(Shader.GetUniformLocation("u_windowSize"), 1, new float[] { window.Width, window.Height });
            GL.Uniform1(Shader.GetUniformLocation("u_zoom"), 1, new[] { (window.Height * 0.5f) / (float)Math.Tan(90.0f * (Math.PI / 360.0f)) });
            GL.Uniform1(Shader.GetUniformLocation("f"), 1, new[] { 0.0f });

            GL.DrawArrays(PrimitiveType.TriangleFan, 0, 4);

            Shader.Unbind();
        }
    }
}
