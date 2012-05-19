using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using UI.Components;

namespace Mechanect.Exp3
{
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

        public Ball(Vector3 intialPosition, float radius,  GraphicsDevice device, ContentManager content)
            : base(content.Load<Model>(@"Models/ball"), intialPosition, Vector3.Zero, Vector3.One, device)
        {
            Radius = radius;
            Mass = 0.001;
        }

        public void Rotate(Vector3 displacement)
        {
            float perimeter = (float)(2 * Math.PI * radius);
            Rotation = new Vector3(displacement.Length() / (perimeter / 6), (float)Math.Atan2(displacement.X, displacement.Z), 0);
        }

        public void SetHeight(float height)
        {
            Position = new Vector3(Position.X, height + radius, Position.Z);
        }

        /// <summary>
        /// Creates the animation of the ball falling into the hole.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Omar Abdulaal.</para>
        /// </remarks>
        /// <returns>Radius of the balls model</returns>
        private float GetRadius()
        {
            BoundingSphere sphere = new BoundingSphere();
            foreach (ModelMesh mesh in Model.Meshes)
            {
                sphere = BoundingSphere.CreateMerged(sphere, mesh.BoundingSphere);
            }
            return sphere.Radius;
        }
    }
}
