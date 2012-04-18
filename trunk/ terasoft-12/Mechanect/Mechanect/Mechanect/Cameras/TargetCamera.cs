using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mechanect.Cameras
{
    public class TargetCamera : Camera
    {

        private Vector3 position;
        private Vector3 target;

        public TargetCamera(Vector3 position, Vector3 target, GraphicsDevice graphicsDevice)
            : base(graphicsDevice)
        {
            this.position = position;
            this.target = target;
        }

        public override void Update()
        {
            this.view = Matrix.CreateLookAt(position, target, Vector3.Up);
        }

    }

}
