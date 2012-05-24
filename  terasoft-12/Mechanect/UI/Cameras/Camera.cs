using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UI.Cameras
{
    /// <summary>
    /// Represents a generic type for all camera types.
    /// </summary>
    /// <remarks>
    /// AUTHOR : Bishoy Bassem.
    /// </remarks>
    public abstract class Camera
    {

        public Matrix View { get; set; }
        public Matrix Projection { get; set; }
        public Vector3 Position { get; protected set; }

        protected GraphicsDevice GraphicsDevice { get; set; }

        /// <summary>
        /// Intializes the camera's attributes.
        /// </summary>
        /// <param name="graphicsDevice">Displays graphics on the screen.</param>
        /// <remarks>
        /// AUTHOR : Bishoy Bassem.
        /// </remarks>
        public Camera(GraphicsDevice graphicsDevice)
        {
            this.GraphicsDevice = graphicsDevice;
            this.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 
                graphicsDevice.Viewport.AspectRatio, 0.1f, 1000000.0f);
        }

        /// <summary>
        /// Updates the camera's view and projection matrices.
        /// </summary>
        /// <remarks>
        /// AUTHOR : Bishoy Bassem.
        /// </remarks>
        public abstract void Update();

    }
}
