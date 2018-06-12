using dmr.Models.Maps;
using System;
using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using OpenTK;

namespace dmr.gl.Renderers
{
    internal class TerrainRenderer : IDisposable
    {
        private readonly Map Map;
        private readonly TileFlagsType[,] TileFlags;

        private uint VAO, CoordsVBO, IndicesVBO, VBOTriangleCount;
        private int ShaderProgram;

        private Matrix4 projectionMatrix;
        public Matrix4 ProjectionMatrix
        {
            get => projectionMatrix;
            set
            {
                projectionMatrix = value;
                GL.UseProgram(ShaderProgram);
                GL.ProgramUniformMatrix4(ShaderProgram, GL.GetUniformLocation(ShaderProgram, "ProjectionMatrix"), false, ref value);
            }
        }

        private Matrix4 viewMatrix;
        public Matrix4 ViewMatrix
        {
            get => viewMatrix;
            set
            {
                viewMatrix = value;
                GL.UseProgram(ShaderProgram);
                GL.ProgramUniformMatrix4(ShaderProgram, GL.GetUniformLocation(ShaderProgram, "ViewMatrix"), false, ref value);
            }
        }

        [Flags]
        enum TileFlagsType { Void = 0, WallN = 1 << 0, WallE = 1 << 1, WallS = 1 << 2, WallW = 1 << 3, DoorH = 1 << 4, DoorV = 1 << 5, Empty = 1 << 6 };

        internal TerrainRenderer(Map map)
        {
            Map = map;

            var h = map.Tiles.GetLength(0);
            var w = map.Tiles.GetLength(1);

            // "texture" the tiles
            TileFlags = new TileFlagsType[h, w];
            for (int y = 0; y < h; ++y)
                for (int x = 0; x < w; ++x)
                {
                    var tile = map.Tiles[y, x];

                    if (tile == TileTemplate.WallTile)
                    {
                        void TextureWallTile(int dx, int dy, TileFlagsType side)
                        {
                            if (x + dx >= 0 && x + dx < w && y + dy >= 0 && y + dy < h && map.Tiles[y + dy, x + dx] == TileTemplate.WallTile)
                                TileFlags[y, x] |= side;
                        }

                        TextureWallTile(-1, 0, TileFlagsType.WallW);
                        TextureWallTile(+1, 0, TileFlagsType.WallE);
                        TextureWallTile(0, -1, TileFlagsType.WallN);
                        TextureWallTile(0, +1, TileFlagsType.WallS);
                    }
                    else if (tile == TileTemplate.DoorTile)
                    {
                        if (x > 1 && (map.Tiles[y, x - 1] == TileTemplate.DoorTile || map.Tiles[y, x - 1] == TileTemplate.WallTile))
                            TileFlags[y, x] = TileFlagsType.DoorH;
                        else if (x < w - 1 && (map.Tiles[y, x + 1] == TileTemplate.DoorTile || map.Tiles[y, x + 1] == TileTemplate.WallTile))
                            TileFlags[y, x] = TileFlagsType.DoorH;
                        else if (y > 1 && (map.Tiles[y - 1, x] == TileTemplate.DoorTile || map.Tiles[y - 1, x] == TileTemplate.WallTile))
                            TileFlags[y, x] = TileFlagsType.DoorV;
                        else if (y < h - 1 && (map.Tiles[y + 1, x] == TileTemplate.DoorTile || map.Tiles[y + 1, x] == TileTemplate.WallTile))
                            TileFlags[y, x] = TileFlagsType.DoorV;
                        else
                            throw new InvalidOperationException($"Door in the middle of nowhere at {x}, {y}");
                    }
                    else if (tile == TileTemplate.EmptyTile || tile == TileTemplate.StartTile)
                        TileFlags[y, x] = TileFlagsType.Empty;
                }

            VBOTriangleCount = (uint)(2 * h * w);

            // build the coords array
            var coords = new float[2 * (w + 1) * (h + 1)];
            int idx = 0;
            for (int y = 0; y <= h; ++y)
                for (int x = 0; x <= w; ++x)
                {
                    coords[idx++] = x;
                    coords[idx++] = y;
                }

            // build the index array
            var indices = new uint[VBOTriangleCount * 3];
            idx = 0;
            for (uint y = 0; y < h; ++y)
                for (uint x = 0; x < w; ++x)
                {
                    // tri 1
                    indices[idx++] = x * ((uint)w + 1) + y;
                    indices[idx++] = x * ((uint)w + 1) + y + 1;
                    indices[idx++] = (x + 1) * ((uint)w + 1) + y + 1;

                    // tri 2
                    indices[idx++] = x * ((uint)w + 1) + y;
                    indices[idx++] = (x + 1) * ((uint)w + 1) + y + 1;
                    indices[idx++] = (x + 1) * ((uint)w + 1) + y;
                }

            // VAO
            GL.GenVertexArrays(1, out VAO);
            GL.BindVertexArray(VAO);

            // VBO for coords
            GL.GenBuffers(1, out uint CoordsVBO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, CoordsVBO);
            GL.BufferData(BufferTarget.ArrayBuffer, coords.Length * sizeof(float), coords, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(0);

            // VBO for index buffer
            GL.GenBuffers(1, out IndicesVBO);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, IndicesVBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

            // vertex shader
            var vertexshader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexshader, File.ReadAllText(@"Content\Shaders\terrain.vert"));
            GL.CompileShader(vertexshader);
            GL.GetShader(vertexshader, ShaderParameter.CompileStatus, out int compiled);
            if (compiled == 0)
                throw new InvalidOperationException();

            // fragment shader
            var fragmentshader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentshader, File.ReadAllText(@"Content\Shaders\terrain.frag"));
            GL.CompileShader(fragmentshader);
            GL.GetShader(fragmentshader, ShaderParameter.CompileStatus, out compiled);
            if (compiled == 0)
                throw new InvalidOperationException();

            // program
            ShaderProgram = GL.CreateProgram();
            GL.AttachShader(ShaderProgram, vertexshader);
            GL.AttachShader(ShaderProgram, fragmentshader);

            GL.BindAttribLocation(ShaderProgram, 0, "in_Position");

            GL.LinkProgram(ShaderProgram);
            GL.GetProgram(ShaderProgram, GetProgramParameterName.LinkStatus, out compiled);
            if (compiled == 0)
                throw new InvalidOperationException();

            ViewMatrix = projectionMatrix = Matrix4.Identity;
        }

        public void Render()
        {
            GL.UseProgram(ShaderProgram);
            GL.BindVertexArray(VAO);
            GL.DrawElements(PrimitiveType.Triangles, (int)VBOTriangleCount, DrawElementsType.UnsignedInt, 0);
        }

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
        // ~TerrainRenderer() {
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
