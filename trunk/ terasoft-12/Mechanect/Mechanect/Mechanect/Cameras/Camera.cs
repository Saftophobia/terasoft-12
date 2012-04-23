using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mechanect.Cameras
{
    /// <summary>
    /// represents a generic type for all camera types
    /// </summary>
    /// <remarks>
    /// Auther : Bishoy Bassem
    /// </remarks>
    public abstract class Camera
    {
        /// <summary>
        /// the view matrix
        /// </summary>
        public Matrix view { get; set; }
        
        /// <summary>
        /// the projection matrix
        /// </summary>
        public Matrix projection { get; set; }

        protected GraphicsDevice graphicsDevice { get; set; }

        /// <summary>
        /// takes a GraphicsDevice as a parameter
        /// </summary>
        /// <param name="graphicsDevice"></param>
        public Camera(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            this.projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), graphicsDevice.Viewport.AspectRatio, 0.1f, 1000000.0f);
        }

        /// <summary>
        /// causes the camera to update its view and projection matrices
        /// </summary>
        public abstract void Update();

    }
}
