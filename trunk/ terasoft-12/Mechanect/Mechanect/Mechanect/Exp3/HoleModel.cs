using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using UI.Components;
using Physics;

namespace Mechanect.Exp3
{
    public class HoleModel :CustomModel
    {
        int radius;

        public HoleModel(ContentManager content, GraphicsDevice device, Vector3 position, int radius)
            : base(content.Load<Model>(@"Models/holemodel"), position,
            Vector3.Zero, new Vector3((float)Constants3.scaleRatio*1.1f*radius))
        {
            this.radius = radius;
        }
    }
}