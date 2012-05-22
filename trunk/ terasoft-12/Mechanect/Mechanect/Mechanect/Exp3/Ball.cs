using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using UI.Components;

namespace Mechanect.Exp3
{
    /// <summary>
    /// represents the ball type
    /// </summary>
    /// <remarks>
    /// Author : Bishoy Bassem
    /// </remarks>
    public class Ball : CustomModel
    {
        private float radius;
        public float Radius
        {
            get
            {
                return radius;
            }
            set
            {
                radius = value;
                Scale = (radius / BoundingSphere.Radius) * Vector3.One;
            }
        }

        public double Mass { get; set; }

        /// <summary>
        /// constructs a new ball instance
        /// </summary>
        /// <param name="radius">ball radius</param>
        /// <param name="device">graphics device</param>
        /// <param name="content">content manager</param>
        public Ball(float radius, GraphicsDevice device, ContentManager content)
            : base(content.Load<Model>(@"Models/ball"), Vector3.Zero, Vector3.Zero, Vector3.One, device)
        {
            Radius = radius;
            Mass = 0.001;
        }

        /// <summary>
        /// sets the ball position to a random one
        /// </summary>
        /// <param name="terrainWidth">terrain width</param>
        /// <param name="terrainHeight">terrain height</param>
        public void GenerateIntialPosition(float terrainWidth, float terrainHeight)
        {
            float number = (float)new Random().NextDouble();
            Position = new Vector3(-terrainWidth / 2, 0, -(0.25f + number / 2) * terrainHeight / 2);
        }

        /// <summary>
        /// sets the ball orientation according to the moving direction
        /// </summary>
        /// <param name="displacement">displacement vector</param>
        public void Rotate(Vector3 displacement)
        {
            float perimeter = (float)(2 * Math.PI * radius);
            Rotation = new Vector3(displacement.Length() / (perimeter / 6), (float)Math.Atan2(displacement.X, displacement.Z), 0);
        }

        /// <summary>
        /// sets the ball height
        /// </summary>
        /// <param name="height">height</param>
        public void SetHeight(float height)
        {
            Position = new Vector3(Position.X, height + radius, Position.Z);
        }

    }
}
