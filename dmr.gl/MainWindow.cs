﻿using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
//using OpenTK.Graphics.ES20;
using QuickFont;
using QuickFont.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dmr.gl
{
    public sealed class MainWindow : GameWindow
    {
        private readonly string OriginalTitle;
        private Matrix4 ProjectionMatrix;

        private QFontDrawing FontDrawing;
        private QFont MainFont;

        public MainWindow()
            : base(800, 600, GraphicsMode.Default, "dmr", GameWindowFlags.FixedWindow, DisplayDevice.Default, 4, 0, GraphicsContextFlags.ForwardCompatible)
        {
            Title += OriginalTitle = $" (ogl {GL.GetString(StringName.Version)})";
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            GL.ClearColor(1, 1, 1, 1);

            FontDrawing = new QFontDrawing();
            MainFont = new QFont("Tahoma", 16, new QFontBuilderConfiguration());
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            FontDrawing.ProjectionMatrix = ProjectionMatrix =
                Matrix4.CreateOrthographicOffCenter(ClientRectangle.X, ClientRectangle.Width, ClientRectangle.Y, ClientRectangle.Height, -1.0f, 1.0f);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            FontDrawing.Print(MainFont, "meep", Vector3.Zero, QFontAlignment.Justify);
            FontDrawing.RefreshBuffers();
            FontDrawing.Draw();

            SwapBuffers();
        }
    }
}