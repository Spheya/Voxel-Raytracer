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
        private readonly BufferTexture<ushort> _bufferTexture = new BufferTexture<ushort>(SizedInternalFormat.R16ui);
        public ShaderProgram Shader { get; set; }

        public Renderer(ShaderProgram shader)
        {
            _canvas = new Model(new[]{
                -1.0f, -1.0f,
                1.0f, -1.0f,
                1.0f,  1.0f,
                -1.0f,  1.0f
            }, 2, PrimitiveType.TriangleFan);

            Shader = shader;
        }

        public VoxelModel CreateModel(int width, int height, int depth, Transform transform)
        {
            
            VoxelModel model = new VoxelModel(_bufferTexture, _bufferTexture.Count, width, height, depth, transform);
            _bufferTexture.AddRange(new ushort[model.Footprint]);
            _models.Add(model);

            return model;
        }

        public VoxelModel CreateModel(int width, int height, int depth)
        {
            VoxelModel model = new VoxelModel(_bufferTexture, _bufferTexture.Count, width, height, depth);
            _bufferTexture.AddRange(new ushort[model.Footprint]);
            _models.Add(model);

            return model;
        }

        public bool Remove(VoxelModel model)
        {
            if (!_models.Remove(model))
                return false;

            _bufferTexture.Erase(model.Offset, model.Footprint);

            foreach(var m in _models)
                if (m.Offset > model.Offset)
                    m.Offset -= model.Footprint;

            return true;
        }

        public void Draw(GameWindow window)
        {
            _bufferTexture.Update();

            Shader.Bind();

            GL.BindVertexArray(_canvas.Vao);

            _bufferTexture.Bind(TextureUnit.Texture0);
            GL.Uniform1(Shader.GetUniformLocation("u_voxelBuffer"), 1, new[] { 0 });
            GL.Uniform3(Shader.GetUniformLocation("u_bufferDimensions"), 1, new[] { _models[0].Width, _models[0].Height, _models[0].Depth });
            GL.Uniform2(Shader.GetUniformLocation("u_windowSize"), 1, new float[] { window.Width, window.Height });
            GL.Uniform1(Shader.GetUniformLocation("u_zoom"), 1, new[] { (window.Height * 0.5f) / (float)Math.Tan(90.0f * (Math.PI / 360.0f)) });
            GL.Uniform1(Shader.GetUniformLocation("f"), 1, new[] { 0.0f });

            GL.DrawArrays(PrimitiveType.TriangleFan, 0, 4);

            Shader.Unbind();
        }
    }
}
