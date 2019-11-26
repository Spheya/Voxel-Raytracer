using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Game.Engine.Shaders;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace Game.Engine.Rendering
{
    class Renderer
    {
        private readonly Model _cubeModel;

        private readonly List<VoxelModel> _models = new List<VoxelModel>();

        public Renderer()
        {
            _cubeModel = new Model(new[]
            {
                0.0f, 0.0f, 0.0f,
                0.0f, 0.0f, 1.0f,
                0.0f, 1.0f, 1.0f,
                1.0f, 1.0f, 0.0f,
                0.0f, 0.0f, 0.0f,
                0.0f, 1.0f, 0.0f,
                1.0f, 0.0f, 1.0f,
                0.0f, 0.0f, 0.0f,
                1.0f, 0.0f, 0.0f,
                1.0f, 1.0f, 0.0f,
                1.0f, 0.0f, 0.0f,
                0.0f, 0.0f, 0.0f,
                0.0f, 0.0f, 0.0f,
                0.0f, 1.0f, 1.0f,
                0.0f, 1.0f, 0.0f,
                1.0f, 0.0f, 1.0f,
                0.0f, 0.0f, 1.0f,
                0.0f, 0.0f, 0.0f,
                0.0f, 1.0f, 1.0f,
                0.0f, 0.0f, 1.0f,
                1.0f, 0.0f, 1.0f,
                1.0f, 1.0f, 1.0f,
                1.0f, 0.0f, 0.0f,
                1.0f, 1.0f, 0.0f,
                1.0f, 0.0f, 0.0f,
                1.0f, 1.0f, 1.0f,
                1.0f, 0.0f, 1.0f,
                1.0f, 1.0f, 1.0f,
                1.0f, 1.0f, 0.0f,
                0.0f, 1.0f, 0.0f,
                1.0f, 1.0f, 1.0f,
                0.0f, 1.0f, 0.0f,
                0.0f, 1.0f, 1.0f,
                1.0f, 1.0f, 1.0f,
                0.0f, 1.0f, 1.0f,
                1.0f, 0.0f, 1.0f
            }, 3, PrimitiveType.Triangles
        );
    }

        public void Add(VoxelModel model) => _models.Add(model);

        public bool Remove(VoxelModel model) => _models.Remove(model);

        public bool Contains(VoxelModel model) => _models.Contains(model);

        public void Draw(ShaderProgram shader, Camera camera, GameWindow window)
        {
            shader.Bind();
            GL.BindVertexArray(_cubeModel.Vao);

            Matrix4 projectionMatrix = camera.CalculateProjectionMatrix(window);
            Matrix4 viewMatrix = camera.CalculateViewMatrix();

            GL.UniformMatrix4(shader.GetUniformLocation("u_projectionMatrix"), false, ref projectionMatrix);
            GL.UniformMatrix4(shader.GetUniformLocation("u_viewMatrix"), false, ref viewMatrix);

            GL.Uniform2(shader.GetUniformLocation("u_windowSize"), 1, new float[] { window.Width, window.Height });
            GL.Uniform1(shader.GetUniformLocation("u_zoom"), 1, new[] { (window.Height * 0.5f) / (float)Math.Tan(camera.Fov * (Math.PI / 360.0f)) });

            foreach (var model in _models)
            {
                model.UpdateBufferTexture();
                
                Matrix4 modelMatrix = model.Transform.CalculateMatrix();

                GL.Uniform3(shader.GetUniformLocation("u_bufferDimensions"), 1, new[] { model.Width, model.Height, model.Depth });
                GL.UniformMatrix4(shader.GetUniformLocation("u_modelMatrix"), false, ref modelMatrix);

                GL.DrawArrays(_cubeModel.Type, 0, _cubeModel.Count);
            }

            GL.BindVertexArray(0);
            shader.Unbind();
        }
    }
}
