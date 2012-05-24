using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using UI.Components;

namespace Mechanect.Exp3
{
    /// <summary>
    /// Represents the ball type
    /// </summary>
    /// <remarks>
    /// AUTHOR : Bishoy Bassem
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
        /// Creates a new ball instance.
        /// </summary>
        /// <param name="radius">Ball radius</param>
        /// <param name="device">Graphics device</param>
        /// <param name="content">Content manager</param>
        public Ball(float radius, GraphicsDevice device, ContentManager content)
            : base(content.Load<Model>(@"Models/ball"), Vector3.Zero, Vector3.Zero, Vector3.One)
        {
            Radius = radius;
            Mass = 0.004f;
        }

        ///<remarks>
        ///<para>
        ///Author: Cena
        ///</para> 
        /// </remarks>
        /// <summary>
        /// generates a random mass for the ball within a certain given range
        /// </summary>
        /// <returns> returns the generated mass</returns>
        public float GenerateBallMass(float minMass, float maxMass)
        {
            Random random = new Random();
            float generatedMass = ((float)(random.NextDouble() * (maxMass - minMass))) + minMass;
            return generatedMass;
        }

        /// <summary>
        /// sets the ball position to a random one
        /// </summary>
        /// <param name="terrainWidth">terrain width</param>
        /// <param name="terrainHeight">terrain height</param>
        public void GenerateIntialPosition(float terrainWidth, float terrainHeight)
        {
            float number = (float)new Random().NextDouble();
            Position = new Vector3(-terrainWidth / 4, 0, (0.25f + number / 2) * -terrainHeight / 2);
        }

        /// <summary>
        /// Sets the ball orientation according to the moving direction.
        /// </summary>
        /// <param name="displacement">Displacement vector</param>
        public void Rotate(Vector3 displacement)
        {
            float perimeter = (float)(2 * Math.PI * radius);
            Rotation = new Vector3(displacement.Length() / (perimeter / 6), (float)Math.Atan2(displacement.X, displacement.Z), 0);
        }


        /// <remarks>
        ///<para>AUTHOR: Omar Abdulaal </para>
        ///</remarks>
        /// <summary>
        /// Checks whether the ball entered the shooting region.
        /// </summary>
        /// <returns>A boolean representing whether the ball entered the shooting region or not.</returns>
        public bool hasBallEnteredShootRegion()
        {
            float ballPosX = Position.X;
            float ballPosZ = Position.Z;
            return (ballPosX <= Constants3.maxShootingX) && (ballPosX >= Constants3.minShootingX) &&
                (ballPosZ <= Constants3.maxShootingZ) && (ballPosZ >= Constants3.minShootingZ);

        }
        
        /// <summary>
        /// Sets the ball height.
        /// </summary>
        /// <param name="height">Height magnitude</param>
        public void SetHeight(float height)
        {
            Position = new Vector3(Position.X, height + radius, Position.Z);
        }

        /// <summary>
        /// Checks whether the ball is inside the terrain or not.
        /// </summary>
        /// <param name="terrainWidth">Terrain width</param>
        /// <param name="terrainHeight">Terrain height</param>
        public bool InsideTerrain(float terrainWidth, float terrainHeight)
        {
            return Position.X > -terrainWidth / 3 && Position.X < terrainWidth / 3 && Position.Z > (-0.8 * terrainHeight / 2);
        }

    }
}
