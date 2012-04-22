using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Mechanect.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Kinect;

namespace Mechanect.Screens
{
    class AdjustPosition : Mechanect.Common.GameScreen
    {

        Skeleton skeleton;
        float minDepth;
        float maxDepth;
        float minAngle;
        float maxAngle;

        float depth;
        float angle;

        Boolean accepted;

        public float getDepth()
        {
            return skeleton.Joints[JointType.HipCenter].Position.Z;
        }

        public float getAngle()
        {
            Vector2 rightHip = new Vector2(skeleton.Joints[JointType.HipRight].Position.X, skeleton.Joints[JointType.HipRight].Position.Z);
            Vector2 leftHip = new Vector2(skeleton.Joints[JointType.HipLeft].Position.X, skeleton.Joints[JointType.HipLeft].Position.Z);
            Vector2 point = new Vector2(rightHip.X - leftHip.X, rightHip.Y - leftHip.Y);
            double angle = Math.Atan(point.Y / point.X);
            angle *= (180 / Math.PI);
            return (float)angle;
        }

        public AdjustPosition(Skeleton skeleton, int minDepth, int maxDepth, float minAngle, float maxAngle)
        {
            this.skeleton = skeleton;
            this.minDepth = minDepth;
            this.maxDepth = maxDepth;
            this.minAngle = minAngle;
            this.maxAngle = maxAngle;
        }

        public void Update(GameTime gameTime, Boolean covered)
        {
            depth = getDepth();
            angle = getAngle();

            accepted = (depth <= maxDepth && depth >= minDepth && angle <= maxAngle && angle >= minAngle);
        }

        public override void Draw(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
