using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Engine.Maths;
using Game.Engine.Shaders;
using Game.Engine.Rendering;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace Game.Engine.Rendering
{
    class VoxelRenderer
    {
        private readonly Model _canvas;

        private readonly List<VoxelModel> _models = new List<VoxelModel>();
        private readonly BufferTexture<byte> _voxelData = new BufferTexture<byte>(SizedInternalFormat.R8ui);
        private readonly BufferTexture<int> _modelData = new BufferTexture<int>(SizedInternalFormat.Rgba32i);
        private readonly BufferTexture<Matrix4> _modelTransformations = new BufferTexture<Matrix4>(SizedInternalFormat.Rgba32f);

        public MaterialPalette Materials { get; }

        public List<DirectionalLight> DirectionalLights { get; set; } = new List<DirectionalLight>();
        public List<PointLight> PointLights { get; set; } = new List<PointLight>();

        private int _framebuffer;
        private int _textureColorBuffer;

        /// <summary>
        /// The shader program used to render stuff
        /// </summary>
        public ShaderProgram Shader { get; set; }
        public ShaderProgram ScaleShader { get; set; }

        /// <param name="shader">The initial shader to render stuff</param>
        public VoxelRenderer(ShaderProgram shader, ShaderProgram scaleShader)
        {
            Materials = new MaterialPalette(shader);

            _canvas = new Model(new[]{
                -1.0f, -1.0f,
                1.0f, -1.0f,
                1.0f,  1.0f,
                -1.0f,  1.0f
            }, 2, PrimitiveType.TriangleFan);

            _modelData.AddRange(new int[] { 0,0,0,0 });

            Shader = shader;
            ScaleShader = scaleShader;
        }

        /// <summary>
        /// Create a VoxelModel and add it to the renderer
        /// </summary>
        /// <param name="width">The width of the model in voxels</param>
        /// <param name="height">The height of the model in voxels</param>
        /// <param name="depth">The depth of the model in voxels</param>
        /// <param name="transform">The transform of the model</param>
        /// <returns>The created VoxelModel</returns>
        public VoxelModel CreateModel(int width, int height, int depth, Transform transform)
        {
            VoxelModel model = new VoxelModel(_voxelData, _voxelData.Count, width, height, depth, transform);
            _models.Add(model);
            _modelData[0] = (ushort) _models.Count;
            _modelData.AddRange(new [] { width, height, depth, model.Offset });
            _voxelData.AddRange(new byte[model.Footprint]);
            _modelTransformations.AddRange(
                new [] {
                    model.Transform.CalculateInverseMatrix(),
                    model.Transform.CalculateMatrix(),
                    model.Transform.CalculateNormalMatrix()
            });

            return model;
        }

        /// <summary>
        /// Create a VoxelModel and add it to the renderer
        /// </summary>
        /// <param name="width">The width of the model in voxels</param>
        /// <param name="height">The height of the model in voxels</param>
        /// <param name="depth">The depth of the model in voxels</param>
        /// <returns>The created VoxelModel</returns>
        public VoxelModel CreateModel(int width, int height, int depth)
        {
            VoxelModel model = new VoxelModel(_voxelData, _voxelData.Count, width, height, depth);
            _models.Add(model);
            _modelData[0] = (ushort)_models.Count;
            _modelData.AddRange(new [] { width, height, depth, model.Offset });
            _voxelData.AddRange(new byte[model.Footprint]);
            _modelTransformations.AddRange(
                    new[] {
                        model.Transform.CalculateInverseMatrix(),
                        model.Transform.CalculateMatrix(),
                        model.Transform.CalculateNormalMatrix()
            });

            return model;
        }

        public void GenerateFramebuffer(GameWindow window)
        {
            //Generate framebuffer
            _framebuffer = GL.GenFramebuffer();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, _framebuffer);

            // create a RGBA color texture
            GL.GenTextures(1, out _textureColorBuffer);
            GL.BindTexture(TextureTarget.Texture2D, _textureColorBuffer);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
                                window.Width / 2, window.Height / 2,
                                0, (PixelFormat)PixelInternalFormat.Rgba, PixelType.UnsignedByte,
                                IntPtr.Zero);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Linear);
            GL.BindTexture(TextureTarget.Texture2D, 0);


            ////Create color attachment texture
            GL.FramebufferTexture(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, _textureColorBuffer, 0);

            DrawBuffersEnum[] bufs = new DrawBuffersEnum[1] { (DrawBuffersEnum)FramebufferAttachment.ColorAttachment0};
            GL.DrawBuffers(bufs.Length, bufs);

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        /// <summary>
        /// Remove a model from the renderer
        /// If this function returns true, using the model will result in undefined behaviour
        /// </summary>
        /// <param name="model">The model that needs to be removed</param>
        /// <returns>If the removal was successful</returns>
        public bool Remove(VoxelModel model)
        {
            int index = _models.FindIndex(0, m => m == model);

            if (!_models.Remove(model))
                return false;

            _voxelData.Erase(model.Offset, model.Footprint);
            _modelData.Erase((index + 1) * 4, 4);
            _modelTransformations.Erase(index * 3, 3);

            for (int i = index; i < _models.Count; i++)
                _models[i].Offset -= model.Footprint;

            return true;
        }

        /// <summary>
        /// Draw the scene
        /// </summary>
        /// <param name="camera">The camera where you want to draw from</param>
        /// <param name="window">The window you want to draw to</param>
        public void Draw(Camera camera, GameWindow window)
        {
            Matrix4 mat = camera.CalculateMatrix();

            _voxelData.Update();
            _modelData.Update();

            for (int i = 0; i < _models.Count; i++)
            {
                Matrix4 inverseModelMatrix = _models[i].Transform.CalculateInverseMatrix();
                Matrix4 modelMatrix = _models[i].Transform.CalculateMatrix();
                Matrix4 normalMatrix = _models[i].Transform.CalculateNormalMatrix();

                // Check if the matrices are the same to avoid sending data the gpu already has
                if (_modelTransformations[i * 3 + 0] != inverseModelMatrix)
                    _modelTransformations[i * 3 + 0] = inverseModelMatrix;

                if (_modelTransformations[i * 3 + 1] != modelMatrix)
                    _modelTransformations[i * 3 + 1] = modelMatrix;

                if (_modelTransformations[i * 3 + 2] != normalMatrix)
                    _modelTransformations[i * 3 + 2] = normalMatrix;
            }
            _modelTransformations.Update();

            Shader.Bind();
            GL.BindVertexArray(_canvas.Vao);

            // Send the buffers containing the models
            _voxelData.Bind(TextureUnit.Texture0);
            GL.Uniform1(Shader.GetUniformLocation("u_voxelBuffer"), 1, new[] { 0 });
            _modelData.Bind(TextureUnit.Texture1);
            GL.Uniform1(Shader.GetUniformLocation("u_modelData"), 1, new[] { 1 });
            _modelTransformations.Bind(TextureUnit.Texture2);
            GL.Uniform1(Shader.GetUniformLocation("u_modelTransformations"), 1, new[] { 2 });

            // Send the materials
            Materials.Bind("u_materials");

            // Send the windowsize
            GL.Uniform2(Shader.GetUniformLocation("u_windowSize"), 1, new float[] { window.Width, window.Height });

            // Send the camera
            GL.Uniform1(Shader.GetUniformLocation("u_camera.zoom"), 1, new[] { (window.Height * 0.5f) / (float)Math.Tan(camera.Fov * (Math.PI / 360.0f)) });
            GL.UniformMatrix4(Shader.GetUniformLocation("u_camera.matrix"), false, ref mat);

            // Send the lights
            GL.Uniform1(Shader.GetUniformLocation("u_dirLightCount"), DirectionalLights.Count());
            for (int i = 0; i < DirectionalLights.Count(); i++)
            {
                GL.Uniform3(Shader.GetUniformLocation($"u_dirLights[{i}].direction"), DirectionalLights[i].direction);
                GL.Uniform1(Shader.GetUniformLocation($"u_dirLights[{i}].intensity"), DirectionalLights[i].intensity);
                GL.Uniform3(Shader.GetUniformLocation($"u_dirLights[{i}].colour"), DirectionalLights[i].colour);
            }
            GL.Uniform1(Shader.GetUniformLocation("u_pointLightCount"), PointLights.Count());
            for (int i = 0; i < PointLights.Count(); i++)
            {
                GL.Uniform3(Shader.GetUniformLocation($"u_pointLights[{i}].position"), PointLights[i].position); //_pointLights[i].position
                GL.Uniform1(Shader.GetUniformLocation($"u_pointLights[{i}].intensity"), PointLights[i].intensity);
                GL.Uniform3(Shader.GetUniformLocation($"u_pointLights[{i}].colour"), PointLights[i].colour);
            }

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, _framebuffer);
            GL.DrawArrays(PrimitiveType.TriangleFan, 0, 4);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

            Shader.Unbind();

            ScaleShader.Bind();
            //GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, _textureColorBuffer);
            GL.Uniform1(ScaleShader.GetUniformLocation("u_framebuffer"), 1, new[] { 0 });
            GL.Uniform2(ScaleShader.GetUniformLocation("u_resolution"), 1, new float[] { window.Width, window.Height });

            GL.DrawArrays(PrimitiveType.TriangleFan, 0, 4);

            ScaleShader.Unbind();
        }
    }
}
