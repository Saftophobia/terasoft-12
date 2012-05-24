using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UI.Cameras
{
    /// <summary>
    /// Represents the target camera type.
    /// </summary>
    /// <remarks>
    /// AUTHOR : Bishoy Bassem.
    /// </remarks>
    public class TargetCamera : Camera
    {

        private Vector3 target;

        /// <summary>
        /// Creates a new TargetCamera instance.
        /// </summary>
        /// <param name="graphicsDevice">Displays graphics on the screen.</param>
        /// <param name="position">Position of the camera.</param>
        /// <param name="target">The postion the camera is looking at.</param>
        /// <remarks>
        /// AUTHOR : Bishoy Bassem.
        /// </remarks>
        public TargetCamera(Vector3 position, Vector3 target, GraphicsDevice graphicsDevice)
            : base(graphicsDevice)
        {
            Position = position;
            this.target = target;
        }

        /// <summary>
        /// Updates the camera's view and projection matrices.
        /// </summary>
        /// <remarks>
        /// AUTHOR : Bishoy Bassem.
        /// </remarks>
        public override void Update()
        {
            View = Matrix.CreateLookAt(Position, target, Vector3.Up);
        }

    }

}
