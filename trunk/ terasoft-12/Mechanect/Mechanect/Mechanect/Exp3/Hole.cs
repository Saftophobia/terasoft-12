using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using UI.Components;
using Physics;

namespace Mechanect.Exp3
{
    public class Hole :CustomModel
    {
        public int Radius { get; set; }
        private Vector3 shootingPos;
        int terrainWidth;
        int terrainHeight;

        public Hole(ContentManager content, GraphicsDevice device, int terrainWidth, int terrainHeight, int radius, Vector3 shootingPos)
            : base(content.Load<Model>(@"Models/holemodel"), Functions.GeneratePosition(radius,terrainWidth,terrainHeight),
            Vector3.Zero,  new Vector3((float)Constants3.scaleRatio*radius))
        {
            Radius = radius;
            this.terrainWidth = terrainWidth;
            this.terrainHeight = terrainHeight;
            this.shootingPos = shootingPos ;
        }
    }
}