using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UI.Cameras
{
    /// <summary>
    /// represents a generic type for all camera types
    /// </summary>
    /// <remarks>
    /// Auther : Bishoy Bassem
    /// </remarks>
    public abstract class Camera
    {

        public Matrix View { get; set; }
        public Matrix Projection { get; set; }
        public Vector3 Position { get; protected set; }

        protected GraphicsDevice GraphicsDevice { get; set; }

        /// <summary>
        /// takes a GraphicsDevice as a parameter
        /// </summary>
        /// <param name="graphicsDevice"></param>
        public Camera(GraphicsDevice graphicsDevice)
        {
            this.GraphicsDevice = graphicsDevice;
            this.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 
                graphicsDevice.Viewport.AspectRatio, 0.1f, 1000000.0f);
        }

        /// <summary>
        /// causes the camera to update its view and projection matrices
        /// </summary>
        public abstract void Update();

    }
}
