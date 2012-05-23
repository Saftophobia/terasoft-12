using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UI.Cameras
{
    /// <summary>
    /// Represents the target camera type.
    /// </summary>
    /// <remarks>
    /// AUTHOR : Bishoy Bassem
    /// </remarks>
    public class TargetCamera : Camera
    {

        private Vector3 target;

        /// <summary>
        /// Constructs a TargetCamera instance.
        /// </summary>
        /// <param name="graphicsDevice"></param>
        /// <param name="position">Position of the camera</param>
        /// <param name="target">The postion the camera is looking at</param>
        public TargetCamera(Vector3 position, Vector3 target, GraphicsDevice graphicsDevice)
            : base(graphicsDevice)
        {
            Position = position;
            this.target = target;
        }

        /// <summary>
        /// Causes the camera to update its view and projection matrices.
        /// </summary>
        public override void Update()
        {
            View = Matrix.CreateLookAt(Position, target, Vector3.Up);
        }

    }

}
