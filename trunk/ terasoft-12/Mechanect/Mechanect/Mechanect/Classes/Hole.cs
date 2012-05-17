using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Mechanect.Common;
using Microsoft.Xna.Framework.Graphics;
using Mechanect.Cameras;

namespace Mechanect.Classes
{
    public class Hole
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
            shootingPosition = shootingPos ;
            SetHoleValues();
            hole = new CustomModel(c.Load<Model>(@"Models/holemodel"), position, Vector3.Zero, new Vector3((float)Constants3.scaleRatio*radius), d);
        }
        /// <summary>
        /// Draws the 3d model of the hole given a camera.
        /// </summary>
        /// <remarks>
        ///<para>AUTHOR: Khaled Salah </para>
        ///</remarks>
        /// <param name="cam">
        /// The camera needed to draw the 3d model.
        /// </param>

        public void DrawHole(Camera cam)
        {
            hole.Draw(cam);
        }
        /// <summary>
        /// Sets the X,Y,Z values for the hole position which are related to the enviroment's terrain width and height.
        /// </summary>
        /// <remarks>
        ///<para>AUTHOR: Khaled Salah </para>
        ///</remarks>

        public void SetHoleValues()
        {
            position.X = Tools3.GenerateRandomValue(-terrainWidth / 4, terrainWidth / 4);
            position.Y = 3;
            position.Z = Tools3.GenerateRandomValue(-(terrainHeight- radius)/2, (shootingPosition.Z - radius));
        }
    }
}
