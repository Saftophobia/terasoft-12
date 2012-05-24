using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using UI.Cameras;

namespace Mechanect.Exp3
{
   public class Hole
    {
        private int radius;
        public Vector3 Position { get; set; }
        private HoleModel holeModel;
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
        public Hole(int radius, Vector3 position,int terrainWidth,int terrainHeight)
        {
            this.radius = radius;
            Position = position;
            this.terrainWidth = terrainWidth;
            this.terrainHeight = terrainHeight;
        }
        public void InitialzeUI(ContentManager content, GraphicsDevice device)
        {
            holeModel = new HoleModel(content, device, Position, radius);
        }
        public void Draw(Camera c)
        {
            holeModel.Draw(c);
        }
    }
}
