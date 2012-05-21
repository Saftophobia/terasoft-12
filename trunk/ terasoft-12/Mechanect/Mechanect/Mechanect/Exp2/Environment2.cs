using System;
using Mechanect.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Mechanect.Exp2
{
    public class Environment2
    {
        /// <summary>
        /// region for generating the basic environment elements
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Alzayat </para>   
        /// <para>DATE WRITTEN: May, 18 </para>
        /// <para>DATE MODIFIED: May, 18  </para>
        /// </remarks>
        #region
        private Texture2D xyAxisTexture;
        private Texture2D lineConnector;
        private Vector2 predatorScaling;
        private Vector2 preyScaling;
        private Vector2 aquariumScaling;
        private float windowWidth;
        private float windowHeight;
        private Vector2 pixelsPerMeter;
        private Vector2 windowStartPosition;
        private float axisesPercentage;
        private MySpriteBatch mySpriteBatch;
        private SpriteBatch spriteBatch;
        private SpriteFont spriteFont;
        private GraphicsDevice graphicsDevice;
        private Viewport viewPort;
        #endregion
        public Prey Prey { get; set; }
        public Predator Predator { get; set; }
        public Aquarium Aquarium { get; set; }
        public Aquarium StartAquarium { get; set; }
        private readonly Random _rand;
        private double _velocity;
        public double Velocity
        {
            get { return _velocity; }
            set { _velocity = value; }
        }
        private double _angle;
        private int _tolerance;
        /// <summary>
        /// getterMethod for tolerance
        /// setterMethod for tolerance
        /// </summary>
        public int Tolerance
        {
            get { return _tolerance; }
            set { _tolerance = value; }
        }
        /// <summary>
        /// getAngle,returns the angle in degree.
        /// </summary>
        public double Angle
        {
            get { return _angle * (180 / Math.PI); }
        }
        public Environment2(int tolerance)
        {
            _rand = new Random();
            _tolerance = tolerance;
            _velocity = 0;
            _angle = 0;
            GetSolvablePoints();
        }
        /// <summary>
        /// Constructor that assigns Predator,prey and aquarium positions,length and width
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Tamer Nabil </para>
        /// </remarks>
        /// <param name="position">represents position for predator</param>
        /// <param name="prey">represents x and y coordinates for prey + length + width</param>
        /// <param name="aquriaum">represents x and y coordinates for aquarium + length + width</param>

        public Environment2(Vector2 position, Rectangle prey, Rectangle aquriaum)
        {
            Predator = new Predator(position);
            Prey = new Prey(new Vector2(prey.X, prey.Y), prey.Width, prey.Height);
            Aquarium = new Aquarium(new Vector2(aquriaum.X, aquriaum.Y), aquriaum.Width, aquriaum.Height);
            StartAquarium = new Aquarium(new Vector2(position.X, position.Y), aquriaum.Width, aquriaum.Height);
        }
        /// <summary>
        /// generates random angle between 20 and 70
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Tamer Nabil </para>
        /// </remarks>
        /// <returns>return angle in form of int</returns>
        private int GetRandomAngle()
        {
            return (int)GetRandomNumber(20, 70);
        }
        /// <summary>
        /// generates random velocity between 5 and 25
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Tamer Nabil </para>
        /// </remarks>
        /// <returns>returns random velocity "int"</returns>
        private int GetRandomVelocity()
        {
            return (int)GetRandomNumber(5, 25);
        }
        /// <summary>
        /// getSolve : Generate solvable enviroment by setting solvable points for predator,Prey and Aquarium 
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Tamer Nabil </para>
        /// </remarks>

        private void GetSolvablePoints()
        {
            var preyLocation = new Vector2();
            var predatorLocation = new Vector2();
            var aquariumLocation = new Vector2();
            predatorLocation.X = 0;
            predatorLocation.Y = _rand.Next(0, 20);
            double angleInDegree = GetRandomAngle();
            _angle = angleInDegree * (Math.PI / 180);
            _velocity = GetRandomVelocity();
            double totalTime = GetTotalTime(predatorLocation.Y);

            double timeSlice = totalTime / 3;
            double timePrey = GetRandomNumber(timeSlice - 10 / 100, (timeSlice * 2) - (timeSlice + timeSlice * Tolerance * 2 / 100));
            double timeAquarium = GetRandomNumber(timePrey + (timePrey * Tolerance * 2 / 100), totalTime);

            preyLocation.X = GetX(timePrey);
            preyLocation.Y = (float)((_velocity * Math.Sin(_angle) * timePrey) - (0.5 * 9.8 * Math.Pow(timePrey, 2)) + predatorLocation.Y);

            aquariumLocation.X = GetX(timeAquarium);
            aquariumLocation.Y = (float)((_velocity * Math.Sin(_angle) * timeAquarium) - (0.5 * 9.8 * Math.Pow(timeAquarium, 2)) + predatorLocation.Y);

            float minimumHeight = Math.Min(predatorLocation.Y, aquariumLocation.Y - aquariumLocation.Y * ((float)_tolerance / 100));
            predatorLocation.Y = predatorLocation.Y - minimumHeight;
            preyLocation.Y = preyLocation.Y - minimumHeight;
            aquariumLocation.Y = aquariumLocation.Y - minimumHeight;


            Predator = new Predator(predatorLocation);
            Aquarium = new Aquarium(aquariumLocation, aquariumLocation.X * ((float)_tolerance / 100), aquariumLocation.Y * ((float)_tolerance / 100));
            Prey = new Prey(preyLocation, Aquarium.Length * 2 / 5, Aquarium.Width * 2 / 5);
        }

        /// <summary>
        /// GetTotalTime method calculates the total time needed for aprojectile to come to an end.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Tamer Nabil </para>
        /// </remarks>
        /// <param name="predatorLocationY">takes as input location y of the predator to be able to calculate time</param>
        /// <returns>returns total time needed for the projectile</returns>
        private double GetTotalTime(float predatorLocationY)
        {
            var secondqudrantInFormula = _velocity * Math.Sin(_angle);
            //solving quadratic formula for time
            double totalTime = (-secondqudrantInFormula + Math.Sqrt(Math.Pow(secondqudrantInFormula, 2) - (4 * 0.5 * -9.8 * predatorLocationY))) / (2 * 0.5 * -9.8);
            if (totalTime < 0)
                totalTime = (-secondqudrantInFormula - Math.Sqrt(Math.Pow(secondqudrantInFormula, 2) - (4 * 0.5 * -9.8 * predatorLocationY))) / (2 * 0.5 * -9.8);
            return totalTime;
        }
        /// <summary>
        /// GetX is method which return the horizontal displacment of a projectile at certain time. 
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Tamer Nabil </para>
        /// </remarks>
        /// <param name="time">time in which you want to know the value of x axis at</param>
        /// <returns>x position at certain time in float</returns>
        private float GetX(double time)
        {
            var xPosition = (float)(_velocity * (Math.Cos(_angle)) * time);
            if (xPosition >= 0)
                return xPosition;
            return xPosition * -1;
        }
        /// <summary>
        /// generate Random number between min and max
        /// </summary>
        ///   <remarks>
        /// <para>AUTHOR: Tamer Nabil </para>
        /// </remarks>
        /// <param name="min">min number to generate</param>
        /// <param name="max">max number to generate></param>
        /// <returns>returns random number in double </returns>
        private double GetRandomNumber(double min, double max)
        {
            return (max - min) * _rand.NextDouble() + min;
        }


        #region Update

        /// <summary>
        /// determines whether the predator eats the prey or not
        /// </summary>
        ///<remarks>
        ///<para>
        ///Author: Mohamed AbdelAzim
        ///</para>
        ///</remarks>
        ///<returns>a boolean flag which is true if the prey is eating and false otherwise</returns>
        private Boolean isPreyEaten()
        {
            if (Predator.Location.X >= Prey.Location.X - Prey.Width / 2
                && Predator.Location.X <= Prey.Location.X + Prey.Width / 2
                && Predator.Location.Y >= Prey.Location.Y - Prey.Length / 2
                && Predator.Location.Y <= Prey.Location.Y + Prey.Length / 2)
                return true;
            return false;
        }


        /// <summary>
        /// determines whether the predator reached the aquarium or not
        /// </summary>
        ///<remarks>
        ///<para>
        ///Author: Mohamed AbdelAzim
        ///</para>
        ///</remarks>
        ///<returns>returns true if the predator reached the aquarium</returns>
        private Boolean isAquariumReached()
        {
            if (Predator.Location.X >= Aquarium.Location.X - Aquarium.Width / 2
                && Predator.Location.X <= Aquarium.Location.X + Aquarium.Width / 2
                && Predator.Location.Y >= Aquarium.Location.Y - Aquarium.Length / 2
                && Predator.Location.Y <= Aquarium.Location.Y + Aquarium.Length / 2)
                return true;
            return false;
        }

        public bool Update(GameTime gameTime)
        {
            if (Predator.Movable)
            {
                Predator.UpdatePosition(gameTime);
                if (!Prey.Eaten) Prey.Eaten = isPreyEaten();
                Predator.Movable = isAquariumReached() || Predator.Location.Y < 0;
                return true;
            }
            return false;
        }

        #endregion


        // <summary>
        /// Sets the texture for the X, Y axises
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Alzayat </para>   
        /// <para>DATE WRITTEN: May, 18 </para>
        /// <para>DATE MODIFIED: May, 18  </para>
        /// </remarks>
        /// <param name="contentManager">A content Manager to get the texture from the directories</param>
        /// <param name="graphicsDevice">A graphics device to be able to create the line connector</param>
        public void LoadContent(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {

            this.graphicsDevice = graphicsDevice;


            xyAxisTexture = contentManager.Load<Texture2D>("Textures/Experiment2/ImageSet1/xyAxis");
            lineConnector = new Texture2D(graphicsDevice, 1, 1, false, SurfaceFormat.Color);
            lineConnector.SetData(new[] { Color.Gray });
            spriteFont = contentManager.Load<SpriteFont>("Ariel");
            Predator.setTexture(contentManager);
            Prey.setTexture(contentManager);
            Aquarium.setTexture(contentManager);
            StartAquarium.setTexture(contentManager);
            //TBC
            //aquariumScaling = Aquarium.Width / pixelsPerMeter.X;
            //predatorScaling = aquariumScaling;
            //preyScaling = Prey.Width / pixelsPerMeter.X;

            aquariumScaling = new Vector2(0.1f, 0.1f) ;
            predatorScaling = aquariumScaling;
            preyScaling = new Vector2(0.1f, 0.1f); ;
        }



        /// <summary>
        /// Draw the basic elements of the experiment (x and y axises, predator, prey, aquariums, connectors)
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Alzayat </para>   
        /// <para>DATE WRITTEN: May, 18 </para>
        /// <para>DATE MODIFIED: May, 18  </para>
        /// </remarks>
        /// <param name="contentManager">A content Manager to get the texture from the directories</param>
        ///<param name="mySpriteBatch">The MySpriteBatch that will be used in drawing</param>
        ///<param name="rectangle">A rectangle that represents the size of the window that will contain the basic experiment elements</param>
        /// <param name="viewPort"> A viewPort to coorectly calculate were should the objects appear</param>
        public void Draw(Rectangle rectangle, ContentManager contentManager, SpriteBatch spriteBatch, Viewport viewPort)
        {
            
            if (mySpriteBatch == null)
                mySpriteBatch = new MySpriteBatch(spriteBatch);
            if (this.spriteBatch == null)
                this.spriteBatch = spriteBatch;
            this.viewPort = viewPort;


            ConfigureWindowSize(rectangle, viewPort);
            DrawObjects(mySpriteBatch);

            


        }
        /// <summary>
        /// Draw the basic elements of the experiment (x and y axises, predator, prey, aquariums, connectors)
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Alzayat </para>   
        /// <para>DATE WRITTEN: May, 18 </para>
        /// <para>DATE MODIFIED: May, 19  </para>
        /// </remarks>
        /// <param name="contentManager">A content Manager to get the texture from the directories</param>
        ///<param name="mySpriteBatch">The MySpriteBatch that will be used in drawing</param>
        ///<param name="rectangle">A rectangle that represents the size of the window that will contain the basic experiment elements</param>
        private void DrawObjects(MySpriteBatch mySpriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(xyAxisTexture, windowStartPosition, null, Color.White, 0, windowStartPosition, new Vector2(windowWidth,windowHeight)*1.05f/new Vector2(viewPort.Width,viewPort.Height), SpriteEffects.None, 0);
            spriteBatch.End();
            Predator.Draw(mySpriteBatch, PositionMapper(Predator.Location), predatorScaling);
            Prey.Draw(mySpriteBatch, PositionMapper(Prey.Location), preyScaling);
            Aquarium.Draw(mySpriteBatch, PositionMapper(Aquarium.Location), aquariumScaling);
            spriteBatch.Begin();
            spriteBatch.DrawString(spriteFont, PositionMapper(Predator.Location)+"", Vector2.Zero, Color.Red);
            spriteBatch.End();
        }

        /// <summary>
        /// Configure the size of the window that the basic objects will be drawn in.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Alzayat </para>   
        /// <para>DATE WRITTEN: May, 18 </para>
        /// <para>DATE MODIFIED: May, 18  </para>
        /// </remarks>
        /// <param name="rectangle">A rectangle that represents the size of the window that will contain the basic experiment elements</param>
        /// <param name="viewPort"> A viewPort to coorectly calculate were should the objects appear</param>
        public void ConfigureWindowSize(Rectangle rectangle, Viewport viewPort)
        {
            //Setting the window size and start point
            windowWidth = rectangle.Width;
            windowHeight = rectangle.Height;
            windowStartPosition.X = rectangle.X;
            windowStartPosition.Y = rectangle.Y;

            // Getting the maximum possible difference between the experiment objects
            float maxDifferenceX = Aquarium.Location.X - Predator.Location.X;
            float maxDifferenceY = Math.Max(Prey.Location.Y, Math.Max(Aquarium.Location.Y, Predator.Location.Y));

            // Mapping the meters to pixels to configure how will the real world be mapped to the screen
            pixelsPerMeter.Y = windowHeight / maxDifferenceY;
            pixelsPerMeter.X = windowWidth / maxDifferenceX;


        }

        /// <summary>
        /// Maps The position of a general real world value to be drawn inside the drawing window
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Alzayat </para>   
        /// <para>DATE WRITTEN: May, 19 </para>
        /// <para>DATE MODIFIED: May, 20  </para>
        /// </remarks>
        /// <param name="unMappedPosition">The real world position</param>
        /// <returns> The mapped position vector</returns>
        public Vector2 PositionMapper(Vector2 unMappedPosition)
        {
            Vector2 mappedPosition;
            mappedPosition = unMappedPosition * pixelsPerMeter;
            mappedPosition.Y = windowHeight - mappedPosition.Y;
            mappedPosition += windowStartPosition;
            return mappedPosition;

        }
        /// <summary>
        /// This method will draw a gray line 
        /// implemented initially to connect the GUI objects with the x,y axises
        /// </summary>
        ///  <remarks>
        /// <para>AUTHOR: Mohamed Alzayat </para>   
        /// <para>DATE WRITTEN: April, 20 </para>
        /// <para>DATE MODIFIED: May, 20  </para>
        /// </remarks>
        /// <param name="SpriteBatch">Takes the SpriteBatch that will draw the line </param>
        /// <param name="lineTexture">Takes the texture of the line to be drawn</param>
        /// <param name="width">Determines the width of the line to be drawn</param>
        /// <param name="color">Determines the color to be drawn</param>
        /// <param name="point1">Determines the start point of the line to be drawn</param>
        /// <param name="point2">Determines the end point of the line to be drawn</param>
        void DrawLine(SpriteBatch SpriteBatch, Texture2D lineTexture,
              float width, Color color, Vector2 point1, Vector2 point2)
        {
            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            float length = Vector2.Distance(point1, point2);

            SpriteBatch.Draw(lineTexture, point1, null, color,
                       angle, Vector2.Zero, new Vector2(length, width),
                       SpriteEffects.None, 0);
        }

        /// <summary>
        /// This is to be called when the game needs drawing the  X,Y axis Connectors and position the values on the x,y Axises.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Alzayat </para>   
        /// <para>DATE WRITTEN: April, 22 </para>
        /// <para>DATE MODIFIED: May, 20  </para>
        /// </remarks>
        private void DrawConnectors()
        {
            Vector2 predatorPosition = PositionMapper(Predator.Location);
            Vector2 preyPosition = PositionMapper(Prey.Location);
            Vector2 aquariumPosition = PositionMapper(Aquarium.Location);

            DrawLine(spriteBatch, lineConnector, 2, Color.LightGray, predatorPosition, new Vector2(predatorPosition.X, 0));
            DrawLine(spriteBatch, lineConnector, 2, Color.LightGray, predatorPosition, new Vector2(0, predatorPosition.Y));

            DrawLine(spriteBatch, lineConnector, 2, Color.LightGray, preyPosition, new Vector2(preyPosition.X, 0));
            DrawLine(spriteBatch, lineConnector, 2, Color.LightGray, preyPosition, new Vector2(0, preyPosition.Y));

            DrawLine(spriteBatch, lineConnector, 2, Color.LightGray, aquariumPosition, new Vector2(aquariumPosition.X, 0));
            DrawLine(spriteBatch, lineConnector, 2, Color.LightGray, aquariumPosition, new Vector2(0, aquariumPosition.Y));

            spriteBatch.DrawString(spriteFont, (Math.Round(Predator.Location.X, 2) + ""), new Vector2(predatorPosition.X, 0), Color.Red, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(spriteFont, (Math.Round(Predator.Location.Y, 2) + ""), new Vector2(0, predatorPosition.Y), Color.Red, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

            spriteBatch.DrawString(spriteFont, (Math.Round(Prey.Location.X, 2) + ""), new Vector2(preyPosition.X, 0), Color.Red, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(spriteFont, (Math.Round(Predator.Location.Y, 2) + ""), new Vector2(0, preyPosition.Y), Color.Red, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

            spriteBatch.DrawString(spriteFont, (Math.Round(Aquarium.Location.X, 2) + ""), new Vector2(aquariumPosition.X, 0), Color.Red, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(spriteFont, (Math.Round(Aquarium.Location.Y, 2) + ""), new Vector2(0, aquariumPosition.Y), Color.Red, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

        }


        
    }
}

