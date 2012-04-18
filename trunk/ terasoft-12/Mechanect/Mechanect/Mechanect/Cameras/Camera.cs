using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mechanect.Cameras
{
    public abstract class Camera
    {

        public Matrix view { get; set; }
        public Matrix projection { get; set; }

        protected GraphicsDevice GraphicsDevice { get; set; }

        public Camera(GraphicsDevice graphicsDevice)
        {
            this.GraphicsDevice = graphicsDevice;
            this.projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), graphicsDevice.Viewport.AspectRatio, 0.1f, 1000000.0f);
        }

        public abstract void Update();

    }
}
