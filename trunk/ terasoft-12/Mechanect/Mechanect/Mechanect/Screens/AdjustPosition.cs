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
        int minDepth;
        int maxDepth;
        float minAngle;
        float maxAngle;

        int depth;
        float angle;

        public int getDepth()
        {
            return 0;
        }

        public float getAngle()
        {
            return 0;
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
        }

        public override void Draw(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
