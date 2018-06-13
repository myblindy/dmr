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

        private readonly uint VAO, CoordsVBO, IndicesVBO, VBOTriangleCount;
        private readonly ShaderProgram ShaderProgram;
        private readonly Texture2D WallsTexture;

        private Matrix4 projectionMatrix;
        public Matrix4 ProjectionMatrix
        {
            get => projectionMatrix;
            set
            {
                projectionMatrix = value;
                ShaderProgram.Use();
                ShaderProgram.ProgramUniform("ProjectionMatrix", ref value, false);
            }
        }

        private Matrix4 viewMatrix;
        public Matrix4 ViewMatrix
        {
            get => viewMatrix;
            set
            {
                viewMatrix = value;
                ShaderProgram.Use();
                ShaderProgram.ProgramUniform("ViewMatrix", ref value, false);
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

            ShaderProgram = new ShaderProgram(
                File.ReadAllText(@"Content\Shaders\terrain.vert"),
                File.ReadAllText(@"Content\Shaders\terrain.frag"),
                (0, "vert"), (1, "uv"));

            // trigger the matrix update
            ViewMatrix = projectionMatrix = Matrix4.Identity;

            // 2 triangles per cell, w * h cells
            VBOTriangleCount = (uint)(2 * w * h);

            // build the vertex attributes array: 2 vertex coords + 2 uv coords, 4 vertices per cell
            var vertexdata = new float[4 * 4 * w * h];
            int idx = 0;
            for (int y = 0; y < h; ++y)
                for (int x = 0; x < w; ++x)
                {
                    // vertex 1
                    vertexdata[idx++] = x;
                    vertexdata[idx++] = y;

                    // uv 1
                    vertexdata[idx++] = 0;
                    vertexdata[idx++] = 0;

                    // vertex 2
                    vertexdata[idx++] = x;
                    vertexdata[idx++] = y + 1;

                    // uv 2
                    vertexdata[idx++] = 0;
                    vertexdata[idx++] = .25f;

                    // vertex 3
                    vertexdata[idx++] = x + 1;
                    vertexdata[idx++] = y + 1;

                    // uv 3
                    vertexdata[idx++] = .25f;
                    vertexdata[idx++] = .25f;

                    // vertex 4
                    vertexdata[idx++] = x + 1;
                    vertexdata[idx++] = y;

                    // uv 4
                    vertexdata[idx++] = .25f;
                    vertexdata[idx++] = 0;

                }

            // build the index array
            var indexdata = new ushort[VBOTriangleCount * 3];
            idx = 0;
            ushort srcidx = 0;
            for (uint y = 0; y < h; ++y)
                for (uint x = 0; x < w; ++x)
                {
                    // tri 1
                    indexdata[idx++] = srcidx;
                    indexdata[idx++] = (ushort)(srcidx + 1);
                    indexdata[idx++] = (ushort)(srcidx + 2);

                    // tri 2
                    indexdata[idx++] = srcidx;
                    indexdata[idx++] = (ushort)(srcidx + 2);
                    indexdata[idx++] = (ushort)(srcidx + 3);

                    // advance to the next set of 4 vertices
                    srcidx += 4;
                }

            // VAO
            GL.GenVertexArrays(1, out VAO);
            GL.BindVertexArray(VAO);

            // VBO for vertex + uv data
            GL.GenBuffers(1, out CoordsVBO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, CoordsVBO);
            GL.BufferData(BufferTarget.ArrayBuffer, vertexdata.Length * sizeof(float), vertexdata, BufferUsageHint.StaticDraw);

            var varidx = 0;
            GL.EnableVertexAttribArray(varidx);
            GL.VertexAttribPointer(varidx, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);

            varidx = 1;
            GL.EnableVertexAttribArray(varidx);
            GL.VertexAttribPointer(varidx, 2, VertexAttribPointerType.Float, true, 4 * sizeof(float), 2 * sizeof(float));

            // VBO for index buffer
            GL.GenBuffers(1, out IndicesVBO);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, IndicesVBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indexdata.Length * sizeof(ushort), indexdata, BufferUsageHint.StaticDraw);

            // load the walls texture
            using (var texstream = File.OpenRead(@"Content\Graphics\walls.png"))
                WallsTexture = new Texture2D(texstream, TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.ClampToEdge);
        }

        public void Render()
        {
            ShaderProgram.Use();

            GL.ActiveTexture(TextureUnit.Texture0);
            WallsTexture.Bind();
            ShaderProgram.ProgramUniform("tex", 0);

            GL.BindVertexArray(VAO);

            GL.DrawElements(PrimitiveType.Triangles, (int)VBOTriangleCount, DrawElementsType.UnsignedShort, 0);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // managed state
                    ShaderProgram?.Dispose();
                    WallsTexture?.Dispose();
                }

                // free unmanaged resources (unmanaged objects) and override a finalizer below.
                // set large fields to null.

                disposedValue = true;
            }
        }

        // override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~TerrainRenderer() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
            // uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
