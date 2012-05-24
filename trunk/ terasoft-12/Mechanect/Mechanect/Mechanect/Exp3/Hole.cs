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
        int terrainWidth;
        int terrainHeight;

        public Hole(ContentManager content, GraphicsDevice device, int terrainWidth, int terrainHeight, int radius)
            : base(content.Load<Model>(@"Models/holemodel"), Functions.GeneratePosition(radius,terrainWidth,terrainHeight),
            Vector3.Zero, new Vector3((float)Constants3.scaleRatio*1.1f*radius))
        {
            Radius = radius;
            this.terrainWidth = terrainWidth;
            this.terrainHeight = terrainHeight;
        }
    }
}