using dmr.gl.Renderers;
using dmr.Loaders;
using dmr.Models.Maps;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
//using OpenTK.Graphics.ES20;
using QuickFont;
using QuickFont.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dmr.gl
{
    public sealed class MainWindow : GameWindow
    {
        private Matrix4 ProjectionMatrix;

        private QFontDrawing FontDrawing;
        private QFont MainFont;

        private Map Map;
        private TerrainRenderer TerrainRenderer;

        public MainWindow()
            : base(800, 600, GraphicsMode.Default, "dmr", GameWindowFlags.Default, DisplayDevice.Default, 4, 0,
                  GraphicsContextFlags.ForwardCompatible)
        {
            Title += $" (ogl {GL.GetString(StringName.Version)})";
            VSync = VSyncMode.Off;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            GL.ClearColor(1, 1, 1, 1);

            FontDrawing = new QFontDrawing();
            MainFont = new QFont("Tahoma", 16, new QFontBuilderConfiguration());

            // load the rooms
            var rooms = Directory.GetFiles(@"Content\Rooms", "*.txt", SearchOption.AllDirectories)
                .Select(path => RoomLoader.Load(path))
                .ToList();
            var items = Directory.GetFiles(@"Content\Items", "*.txt", SearchOption.AllDirectories)
                .SelectMany(path => ItemLoader.Load(path).Select(w => w.itemtemplate))
                .ToList();

            // build the map
            const int mapw = 75, maph = 25;
            Map = new Map(mapw, maph, rooms, new Random());

            // and the terrain renderer
            TerrainRenderer = new TerrainRenderer(Map)
            {
                ViewMatrix = Matrix4.CreateTranslation(2, 2, 0)* Matrix4.CreateScale(.1f) 
            };
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            FontDrawing.ProjectionMatrix =
                Matrix4.CreateOrthographicOffCenter(ClientRectangle.X, ClientRectangle.Width, ClientRectangle.Y, ClientRectangle.Height, -1.0f, 1.0f);

            // I want (0,1) ranges for width and height for a perfect square, deformed by the aspect ratio
            var ratio = (float)Width / Height;
            TerrainRenderer.ProjectionMatrix = ProjectionMatrix = ratio > 1
                ? Matrix4.CreateOrthographicOffCenter(0, ratio, 0, 1, -1, 1)
                : Matrix4.CreateOrthographicOffCenter(0, 1, 0, 1 / ratio, -1, 1);
        }

        int LastFrameCounter, CurrentFrameCounter;
        DateTime LastFrameCounterUpdate;
        readonly static TimeSpan FrameCounterPeriod = TimeSpan.FromSeconds(1);

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            // fps
            var now = DateTime.Now;
            if (now - FrameCounterPeriod >= LastFrameCounterUpdate)
            {
                LastFrameCounterUpdate = now;
                LastFrameCounter = CurrentFrameCounter;
                CurrentFrameCounter = 0;
            }
            ++CurrentFrameCounter;

            // clear the screen
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // render the terrain
            TerrainRenderer.Render();

            // render the fps
            FontDrawing.DrawingPrimitives.Clear();
            FontDrawing.Print(MainFont, $"FPS: {LastFrameCounter / FrameCounterPeriod.TotalSeconds:0}",
                new Vector3(0, Height, 0), QFontAlignment.Left, Color.Black);
            FontDrawing.RefreshBuffers();
            FontDrawing.Draw();

            SwapBuffers();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            TerrainRenderer.Dispose();
            FontDrawing.Dispose();
            MainFont.Dispose();
        }
    }
}
