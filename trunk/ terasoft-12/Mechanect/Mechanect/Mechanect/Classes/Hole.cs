using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Mechanect.Common;
using Microsoft.Xna.Framework.Graphics;
using Mechanect.Cameras;

namespace Mechanect.Classes
{
    class Hole
    {
        private int radius;
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
        private Vector3 position;
        public Vector3 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }
        private Vector3 shootingPosition;
        //Environment3 environment;

        CustomModel hole;
        int terrainWidth;
        int terrainHeight;

        public Hole(ContentManager c, GraphicsDevice d, int terrainWidth, int terrainHeight, int radius, Vector3 shootingPos)
        {
            this.radius = radius;
            this.terrainWidth = terrainWidth;
            this.terrainHeight = terrainHeight;
            this.shootingPosition = shootingPosition;
            SetHoleValues();
            hole = new CustomModel(c.Load<Model>(@"Models/holemodel"), position, Vector3.Zero, new Vector3(0.1f), d);
        }
        /// <remarks>
        ///<para>AUTHOR: Khaled Salah </para>
        ///</remarks>
        /// <summary>
        /// Draws the 3d model of the hole given a camera.
        /// </summary>
        /// <param name="cam">
        /// The camera needed to draw the 3d model
        /// </param>

        public void DrawHole(Camera cam)
        {
            hole.Draw(cam);
        }

        /// <remarks>
        ///<para>AUTHOR: Khaled Salah </para>
        ///</remarks>
        /// <summary>
        /// Generates a random float value between two float numbers.
        /// </summary>
        /// <param name="min">
        /// The minimum value. 
        /// </param>
        /// /// <param name="max">
        /// The maximum value.
        /// </param>
        /// <returns>
        /// Float number which is the generated random value.
        /// </returns>

        public float GenerateRandomValue(float min, float max)
        {
            if (max > min)
            {
                var random = new Random();
               var value = ((float)(random.NextDouble() * (max - min))) + min;
                return value;
             
            }
            else throw new ArgumentException("max value has to be greater than min value");
        }
        /// <remarks>
        ///<para>AUTHOR: Khaled Salah </para>
        ///</remarks>
        /// <summary>
        /// Sets the X,Y,Z values for the hole position which are related to the enviroment's terrain width and height.
        /// </summary>

        public void SetHoleValues()
        {


            position.X = GenerateRandomValue(-terrainWidth / 4, terrainWidth / 4);
            position.Y = 3;
            position.Z = GenerateRandomValue(-(terrainHeight- radius)/2, (shootingPosition.Z - radius));
        }
    }
}
