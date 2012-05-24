using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UI.Cameras
{
    /// <summary>
    /// Represents the chase camera type.
    /// </summary>
    /// <remarks>
    /// AUTHOR : Bishoy Bassem.
    /// </remarks>
    public class ChaseCamera : Camera
    {
        private Vector3 target;
        private Vector3 followTargetPosition;

        private Vector3 positionOffset;
        private Vector3 targetOffset;

        private Vector3 relativeCameraRotation;

        /// <summary>
        /// Creates a ChaseCamera instance.
        /// </summary>
        /// <param name="graphicsDevice">Displays graphics on the screen.</param>
        /// <param name="positionOffset">Offset between the object and the camera positions.</param>
        /// <param name="relativeCameraRotation">Start orientation of the camera.</param>
        /// <param name="targetOffset">Offset between the object and the camera's target positions.</param>
        /// <remarks>
        /// AUTHOR : Bishoy Bassem.
        /// </remarks>
        public ChaseCamera(Vector3 positionOffset, Vector3 targetOffset, Vector3 relativeCameraRotation, 
            GraphicsDevice graphicsDevice) : base(graphicsDevice)
        {
            this.positionOffset = positionOffset;
            this.targetOffset = targetOffset;
            this.relativeCameraRotation = relativeCameraRotation;
        }

        /// <summary>
        /// Changes the object's postion.
        /// </summary>
        /// <param name="newFollowTargetPosition">New object's postion.</param>
        /// <remarks>
        /// AUTHOR : Bishoy Bassem.
        /// </remarks>
        public void Move(Vector3 newFollowTargetPosition)
        {
            this.followTargetPosition = newFollowTargetPosition;
        }

        /// <summary>
        /// Changes the camera orientation.
        /// </summary>
        /// <param name="rotationChange">Orientation change vector.</param>
        /// <remarks>
        /// AUTHOR : Bishoy Bassem.
        /// </remarks>
        public void Rotate(Vector3 rotationChange)
        {
            this.relativeCameraRotation += rotationChange;
        }

        /// <summary>
        /// Updates the camera's view and projection matrices according to the new object's position and camera orientation.
        /// </summary>
        /// <remarks>
        /// AUTHOR : Bishoy Bassem.
        /// </remarks>
        public override void Update()
        {
            Matrix rotation = Matrix.CreateFromYawPitchRoll(relativeCameraRotation.Y, 
                relativeCameraRotation.X, relativeCameraRotation.Z);
            Vector3 desiredPosition = followTargetPosition + Vector3.Transform(positionOffset, rotation);
            Position = Vector3.Lerp(Position, desiredPosition, .15f);
            target = followTargetPosition + Vector3.Transform(targetOffset, rotation);
            View = Matrix.CreateLookAt(Position, target, Vector3.Transform(Vector3.Up, rotation));
        }
    }
}
