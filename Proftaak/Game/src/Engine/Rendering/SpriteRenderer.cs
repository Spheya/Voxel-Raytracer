using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Engine.Shaders;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace Game.Engine.Rendering
{
    class SpriteRenderer
    {
        private readonly List<Sprite> _sprites = new List<Sprite>();

        private readonly Model _spriteModel = new Model(new float[]
        {
            0.5f, -0.5f,
            0.5f,  0.5f,
            -0.5f, -0.5f,
            -0.5f,  0.5f
        }, 2, PrimitiveType.TriangleStrip);


        public ShaderProgram Shader { get; set; }

        public SpriteRenderer(ShaderProgram shader)
        {
            Shader = shader;
        }
        
        public void Add(Sprite sprite) => _sprites.Add(sprite);
        public void Remove(Sprite sprite) => _sprites.Remove(sprite);

        public void Draw(GameWindow window)
        {
            Shader.Bind();
            GL.BindVertexArray(_spriteModel.Vao);

            // Bind the projection uniform
            Matrix4 projection = new Matrix4(
                2.0f / window.Width, 0.0f, 0.0f, 0.0f,
                0.0f, 2.0f / window.Height, 0.0f, 0.0f,
                0.0f, 0.0f, 0.0f, 0.0f,
                0.0f, 0.0f, 0.0f, 1.0f
            );
            GL.UniformMatrix4(Shader.GetUniformLocation("u_projectionMatrix"), false, ref projection);

            foreach(Sprite sprite in _sprites)
            {
                // Bind sprite specific uniforms
                Matrix4 model = sprite.Transform.CalculateMatrix();
                sprite.Texture.Bind(TextureUnit.Texture0);

                GL.Uniform1(Shader.GetUniformLocation("u_texture"), 1, new [] {0});
                GL.UniformMatrix4(Shader.GetUniformLocation("u_modelMatrix"), false, ref model);
                GL.Uniform4(Shader.GetUniformLocation("u_colour"), 1, new [] {sprite.Colour.R, sprite.Colour.G, sprite.Colour.B, sprite.Colour.A});

                // Draw the sprite
                GL.DrawArrays(_spriteModel.Type, 0, _spriteModel.Count);
            }

            Shader.Unbind();
        }
    }
}
