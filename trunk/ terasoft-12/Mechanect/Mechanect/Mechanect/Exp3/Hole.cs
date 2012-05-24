using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using UI.Cameras;
using Mechanect.Common;
using UI.Components;

namespace Mechanect.Exp3
{
   public class Hole :CustomModel
    {
        private int radius;
        private int terrainWidth;
        private int terrainHeight;
        public int Radius
        {
            get
            {
                return radius;
            }
            set
            {
                radius = value;
            }
        }
        public Hole(int radius, Vector3 position,int terrainWidth,int terrainHeight):
            base(position, Vector3.Zero, new Vector3((float)Constants3.scaleRatio * 1f * radius))
        {
            this.radius = radius;
            Position = position;
            this.terrainWidth = terrainWidth;
            this.terrainHeight = terrainHeight;
        }
        public override void LoadContent(Model model)
        {
            base.LoadContent(model);
          //  Scale = (radius / intialBoundingSphere.Radius) * Vector3.One;
        }
    }
}
