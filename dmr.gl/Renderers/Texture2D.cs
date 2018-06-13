using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;

namespace dmr.gl.Renderers
{
    class Texture2D : IDisposable
    {
        private readonly int TextureID;

        public Texture2D(Stream stream, TextureMinFilter min, TextureMagFilter mag, TextureWrapMode wrap)
        {
            TextureID = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, TextureID);

            using (var bmp = new Bitmap(stream))
            {
                var bmpdata = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
                    System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmpdata.Width, bmpdata.Height, 0,
                    PixelFormat.Bgra, PixelType.UnsignedByte, bmpdata.Scan0);
                bmp.UnlockBits(bmpdata);
            }

            uint tmp = (uint)min;
            GL.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, ref tmp);
            tmp = (uint)mag;
            GL.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, ref tmp);
            tmp = (uint)wrap;
            GL.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, ref tmp);
            GL.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, ref tmp);
        }

        public void Bind() => GL.BindTexture(TextureTarget.Texture2D, TextureID);

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~Texture() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
