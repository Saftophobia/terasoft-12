using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mechanect.Cameras
{
    /// <summary>
    /// represents the chase camera type
    /// </summary>
    /// <remarks>
    /// Auther : Bishoy Bassem
    /// </remarks>
    public class ChaseCamera : Camera
    {
        private Vector3 target;

        private Vector3 followTargetPosition;

        private Vector3 positionOffset;
        private Vector3 targetOffset;

        private Vector3 relativeCameraRotation;

        /// <summary>
        /// constructs a ChaseCamera instance
        /// </summary>
        /// <param name="graphicsDevice"></param>
        /// <param name="positionOffset">offset between the object and the camera positions</param>
        /// <param name="relativeCameraRotation">start orientation of the camera</param>
        /// <param name="targetOffset">offset between the object and the camera's target positions</param>
        public ChaseCamera(Vector3 positionOffset, Vector3 targetOffset, Vector3 relativeCameraRotation, GraphicsDevice graphicsDevice)
            : base(graphicsDevice)
        {
            this.positionOffset = positionOffset;
            this.targetOffset = targetOffset;
            this.relativeCameraRotation = relativeCameraRotation;
        }

        /// <summary>
        /// changes the objects postion
        /// </summary>
        /// <param name="newFollowTargetPosition">the new object's postion</param>
        public void Move(Vector3 newFollowTargetPosition)
        {
            this.followTargetPosition = newFollowTargetPosition;
        }

        /// <summary>
        /// changes the camera orientation
        /// </summary>
        /// <param name="rotationChange">the change value for the orientation</param>
        public void Rotate(Vector3 rotationChange)
        {
            this.relativeCameraRotation += rotationChange;
        }

        /// <summary>
        /// causes the camera to update its view and projection matrices according to the new object's position and camera orientation
        /// </summary>
        public override void Update()
        {
            Matrix rotation = Matrix.CreateFromYawPitchRoll(relativeCameraRotation.Y, relativeCameraRotation.X, relativeCameraRotation.Z);
            Vector3 desiredPosition = followTargetPosition + Vector3.Transform(positionOffset, rotation);
            position = Vector3.Lerp(position, desiredPosition, .15f);
            target = followTargetPosition + Vector3.Transform(targetOffset, rotation);
            view = Matrix.CreateLookAt(position, target, Vector3.Transform(Vector3.Up, rotation));
        }
    }
}
