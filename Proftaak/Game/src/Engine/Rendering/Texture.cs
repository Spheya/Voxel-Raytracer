using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;

namespace Game.Engine.Rendering
{
    class Texture
    {
        private readonly int _id;

        public int Width { get; }
        public int Height { get; }

        public Texture(Bitmap bitmap, TextureMinFilter minFilter = TextureMinFilter.Nearest, TextureMagFilter magFilter = TextureMagFilter.Nearest)
        {
            Width = bitmap.Width;
            Height = bitmap.Height;

            _id = GL.GenTexture();

            GL.BindTexture(TextureTarget.Texture2D, _id);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)minFilter);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)magFilter);

            BitmapData data = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                OpenTK.Graphics.OpenGL4.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

            bitmap.UnlockBits(data);
        }

        public Texture(string file, TextureMinFilter minFilter = TextureMinFilter.Nearest, TextureMagFilter magFilter = TextureMagFilter.Nearest) :
            this(new Bitmap(file), minFilter, magFilter)
        { }

        public void Bind(TextureUnit unit)
        {
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.TextureBuffer, _id);
        }

        ~Texture()
        {
            GLGarbageCollector.AddTexture(_id);
        }
    }
}
