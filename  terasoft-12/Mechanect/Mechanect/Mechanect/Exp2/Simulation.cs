using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Mechanect.Exp2
{
    class Simulation
    {
        private Environment2 environment;
        private float initialVelocity;
        private float initialAngle;

        private Vector2 InitialVelocity
        {
            get
            {
                return initialVelocity * new Vector2((float)Math.Cos(MathHelper.ToRadians(initialAngle)),
                    (float)Math.Sin(MathHelper.ToRadians(initialAngle)));
            }
            set
            {
                initialVelocity = value.Length();
                initialAngle = MathHelper.ToDegrees((float)Math.Tan(value.Y / value.X));
            }
        }

        public Simulation(Vector2 predatorPosition, Rectangle preyPosition, Rectangle aquariumPosition, float velocity, float angle)
        {
            environment = new Environment2(predatorPosition, preyPosition, aquariumPosition);
            initialVelocity = velocity;
            initialAngle = angle;
        }

        void Update(GameTime gameTime)
        {
        }

        void Draw(Rectangle position)
        {
        }
    }
}
