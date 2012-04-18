using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mechanect.Cameras
{
    public class ChaseCamera : Camera
    {
        private Vector3 position;
        private Vector3 target;

        private Vector3 followTargetPosition;

        private Vector3 positionOffset;
        private Vector3 targetOffset;

        private Vector3 relativeCameraRotation { get; set; }

        public ChaseCamera(Vector3 positionOffset, Vector3 targetOffset, Vector3 relativeCameraRotation, GraphicsDevice graphicsDevice)
            : base(graphicsDevice)
        {
            this.positionOffset = positionOffset;
            this.targetOffset = targetOffset;
            this.relativeCameraRotation = relativeCameraRotation;
        }

        public void Move(Vector3 newFollowTargetPosition)
        {
            this.followTargetPosition = newFollowTargetPosition;
        }

        public void Rotate(Vector3 rotationChange)
        {
            this.relativeCameraRotation += rotationChange;
        }

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
