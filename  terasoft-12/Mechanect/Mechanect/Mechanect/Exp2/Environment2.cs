using System;
using System.Windows;
using Mechanect.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
//using NUnit.Framework.Constraints;
using Point = Microsoft.Xna.Framework.Point;

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
        #region: For generating the GUI
        private Texture2D xyAxisTexture;
        private Texture2D backgroundTexture;
        private Texture2D lineConnector;
        public bool DrawBackground { set; get; }
        private Vector2 predatorScaling;
        private Vector2 preyScaling;
        private Vector2 aquariumScaling;
        private Vector2 initialPredatorLocation;
        private float windowWidth;
        private float windowHeight;
        private Vector2 pixelsPerMeter;
        private Vector2 windowStartPosition;
        private float axisesPercentage;
        private MySpriteBatch mySpriteBatch;
        private SpriteBatch spriteBatch;
        private SpriteFont labelsFont;
        private GraphicsDevice graphicsDevice;
        private Viewport viewPort;
        #endregion

        #region InstanceVariables + gettersAndSetters/Tamer
        public Prey Prey { get; set; }
        public Predator Predator { get; set; }
        public Aquarium Aquarium { get; set; }
        public Aquarium StartAquarium { get; set; }
        private readonly Random random;
        private double velocity;
        public double Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }
        private double angle;

        /// <summary>
        /// getAngle,returns the angle in degree.
        /// </summary>
        public double Angle
        {
            get { return angle * (180 / Math.PI); }
        }
        #endregion

        #region Constructor + generatingSolvablePoints/Tamer

        public Environment2()
        {
            random = new Random();
            velocity = 0;
            angle = 0;
            GetSolvablePoints();
            initialPredatorLocation = new Vector2(Predator.Location.X, Predator.Location.Y);
            StartAquarium = new Aquarium(initialPredatorLocation, Aquarium.Width, Aquarium.Length);

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
            initialPredatorLocation = new Vector2(position.X, position.Y);
            StartAquarium = new Aquarium(initialPredatorLocation, aquriaum.Width, aquriaum.Height);

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
        public Environment2(Vector2 position, Rect prey, Rect aquriaum)
        {
            Predator = new Predator(position);
            Prey = new Prey(new Vector2((float)prey.X, (float)prey.Y), (float)prey.Width, (float)prey.Height);
            Aquarium = new Aquarium(new Vector2((float)aquriaum.X, (float)aquriaum.Y),
            (float)aquriaum.Width, (float)aquriaum.Height);
            initialPredatorLocation = new Vector2(position.X, position.Y);
            StartAquarium = new Aquarium(initialPredatorLocation, (float)aquriaum.Width, (float)aquriaum.Height);
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
            predatorLocation.Y = random.Next(0, 20);
            double angleInDegree = GetRandomAngle();
            angle = angleInDegree * (Math.PI / 180);
            velocity = GetRandomVelocity();
            double totalTime = GetTotalTime(predatorLocation.Y);


            double timeSlice = totalTime / 3;
            double timePrey = GetRandomNumber(timeSlice - 10 / 100, (timeSlice * 2) -
            (timeSlice + timeSlice * Tools2.tolerance * 2 / 100));
            double timeAquarium = GetRandomNumber(timePrey +
            (timePrey * Tools2.tolerance * 2 / 100), totalTime);

            preyLocation.X = GetX(timePrey);
            preyLocation.Y = (float)((velocity * Math.Sin(angle) * timePrey) +
            (0.5 * Tools2.gravity * Math.Pow(timePrey, 2)) + predatorLocation.Y);

            aquariumLocation.X = GetX(timeAquarium);
            aquariumLocation.Y = (float)((velocity * Math.Sin(angle) * timeAquarium) +
            (0.5 * Tools2.gravity * Math.Pow(timeAquarium, 2)) + predatorLocation.Y);

            if (preyLocation.X <= 0 || preyLocation.Y < 0 || aquariumLocation.Y < 0)
            {
                GetSolvablePoints();
            }
            if (aquariumLocation.X <= 0 || float.IsNaN(aquariumLocation.Y) || float.IsNaN(aquariumLocation.X))
                GetSolvablePoints();
            if (float.IsNaN(preyLocation.Y) || float.IsNaN(preyLocation.X))
                GetSolvablePoints();

            float minimumHeight = Math.Min(predatorLocation.Y, aquariumLocation.Y -
            aquariumLocation.Y * ((float)Tools2.tolerance / 100));


            predatorLocation.Y = predatorLocation.Y - minimumHeight;
            preyLocation.Y = preyLocation.Y - minimumHeight;
            aquariumLocation.Y = aquariumLocation.Y - minimumHeight;


            Predator = new Predator(predatorLocation);
            Aquarium = new Aquarium(aquariumLocation, aquariumLocation.X * ((float)Tools2.tolerance / 100),
            aquariumLocation.Y * ((float)Tools2.tolerance / 100));
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
            var secondqudrantInFormula = velocity * Math.Sin(angle);
            //solving quadratic formula for time
            double totalTime = (-secondqudrantInFormula + Math.Sqrt(Math.Pow(secondqudrantInFormula, 2) -
            (4 * 0.5 * Tools2.gravity * predatorLocationY))) / (2 * 0.5 * Tools2.gravity);
            if (totalTime < 0)
            {
                totalTime = (-secondqudrantInFormula - Math.Sqrt(Math.Pow(secondqudrantInFormula, 2) -
                  (4 * 0.5 * Tools2.gravity * predatorLocationY))) / (2 * 0.5 * Tools2.gravity);
            }
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
            var xPosition = (float)(velocity * (Math.Cos(angle)) * time);
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
            return (max - min) * random.NextDouble() + min;
        }
        #endregion


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
                Predator.Movable = !(isAquariumReached() || Predator.Location.Y < 0);
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
        /// <para>DATE MODIFIED: May, 21  </para>
        /// </remarks>
        /// <param name="contentManager">A content Manager to get the texture from the directories</param>
        /// <param name="graphicsDevice">A graphics device to be able to create the line connector</param>
        /// <param name="contentManager">A content Manager to get the texture from the directories</param>  
        public void LoadContent(ContentManager contentManager, GraphicsDevice graphicsDevice, Viewport viewPort)
        {

            this.graphicsDevice = graphicsDevice;

            this.viewPort = viewPort;

            xyAxisTexture = contentManager.Load<Texture2D>("Textures/Experiment2/ImageSet1/xyAxis");
            backgroundTexture = contentManager.Load<Texture2D>("Textures/Experiment2/ImageSet1/background");
            lineConnector = new Texture2D(graphicsDevice, 1, 1, false, SurfaceFormat.Color);
            lineConnector.SetData(new[] { Color.Gray });
            labelsFont = contentManager.Load<SpriteFont>("Ariel");
            
            Predator.setTexture(contentManager);
            Prey.setTexture(contentManager);
            Aquarium.setTexture(contentManager);
            StartAquarium.setTexture(contentManager);

            //TBC
            //aquariumScaling = Aquarium.Width / pixelsPerMeter.X;
            //predatorScaling = aquariumScaling;
            //preyScaling = Prey.Width / pixelsPerMeter.X;

            
        }



        /// <summary>
        /// Draw the basic elements of the experiment (x and y axises, predator, prey, aquariums, connectors)
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Alzayat </para>   
        /// <para>DATE WRITTEN: May, 18 </para>
        /// <para>DATE MODIFIED: May, 18  </para>
        /// </remarks>
        ///<param name="rectangle">A rectangle that represents the size of the window that will contain the basic experiment elements</param>
        ///<param name="spriteBatch">The spriteBatch that will be used in drawing</param>
        public void Draw(Rectangle rectangle, SpriteBatch spriteBatch)
        {
            if (DrawBackground)
                spriteBatch.Draw(backgroundTexture, new Rectangle(0, 0, viewPort.Width, viewPort.Height), Color.White);

            if (this.mySpriteBatch == null)
                this.mySpriteBatch = new MySpriteBatch(spriteBatch);
            if (this.spriteBatch == null)
                this.spriteBatch = spriteBatch;

            this.axisesPercentage = 0.08f * rectangle.Width / viewPort.Width;

            rectangle = new Rectangle(rectangle.X + 50, rectangle.Y, Math.Min(rectangle.Width, viewPort.Width), (int)Math.Min(rectangle.Height, viewPort.Height-rectangle.Y-2*labelsFont.MeasureString("0").Y));

            spriteBatch.Draw(xyAxisTexture, rectangle, Color.White);
            Rectangle smallerRrectangle = new Rectangle((int)(rectangle.X + 0.8f * rectangle.Width * axisesPercentage), (int)(rectangle.Y + rectangle.Height * 4.15f * axisesPercentage), (int)(rectangle.Width - rectangle.Width * 5.5f * axisesPercentage), (int)(rectangle.Height - rectangle.Height * 5 * axisesPercentage));

            ConfigureWindowSize(smallerRrectangle, viewPort);
            DrawObjects(smallerRrectangle, mySpriteBatch);
        }


        /// <summary>
        /// Draw the basic elements of the experiment (x and y axises, predator, prey, aquariums, connectors)
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Alzayat </para>   
        /// <para>DATE WRITTEN: May, 18 </para>
        /// <para>DATE MODIFIED: May, 19  </para>
        /// </remarks>
        ///<param name="rectangle">A rectangle that represents the size of the window that will contain the basic experiment elements</param>
        ///<param name="mySpriteBatch">The MySpriteBatch that will be used in drawing</param>
        private void DrawObjects(Rectangle rectangle, MySpriteBatch mySpriteBatch)
        {
            StartAquarium.Draw(mySpriteBatch, PositionMapper(initialPredatorLocation), aquariumScaling);
            Aquarium.Draw(mySpriteBatch, PositionMapper(Aquarium.Location), aquariumScaling);
            if (rectangle.Contains(new Microsoft.Xna.Framework.Point((int)PositionMapper(Predator.Location).X, (int)PositionMapper(Predator.Location).Y))) ;
            Predator.Draw(mySpriteBatch, PositionMapper(Predator.Location), predatorScaling);
            if (!Prey.Eaten)
                Prey.Draw(mySpriteBatch, PositionMapper(Prey.Location), preyScaling);



            DrawConnectors(rectangle);

        }

        /// <summary>
        /// Configure the size of the window that the basic objects will be drawn in.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Alzayat </para>   
        /// <para>DATE WRITTEN: May, 18 </para>
        /// <para>DATE MODIFIED: May, 21  </para>
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
            float maxDifferenceX = Math.Max(Aquarium.Location.X, Predator.Location.X);
            float maxDifferenceY = Math.Max(Prey.Location.Y, Math.Max(Aquarium.Location.Y, Predator.Location.Y));

            // Mapping the meters to pixels to configure how will the real world be mapped to the screen
            pixelsPerMeter.Y = windowHeight / maxDifferenceY;
            pixelsPerMeter.X = Math.Min(windowWidth / maxDifferenceX, pixelsPerMeter.Y);
            pixelsPerMeter.Y = Math.Min(pixelsPerMeter.X, pixelsPerMeter.Y);

            aquariumScaling = Aquarium.Width * pixelsPerMeter;
            predatorScaling = Aquarium.Width * pixelsPerMeter;
            preyScaling = Prey.Width * pixelsPerMeter;
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
            if (unMappedPosition.X<0||unMappedPosition.Y<0)
            {
                unMappedPosition = new Vector2(Math.Abs(unMappedPosition.X),Math.Abs(unMappedPosition.Y));

            }
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
        ///<param name="rectangle">A rectangle that represents the size of the window that will contain the basic experiment elements</param>
        private void DrawConnectors(Rectangle rectangle)
        {
            Vector2 predatorPosition = PositionMapper(Predator.Location);
            Vector2 preyPosition = PositionMapper(Prey.Location);
            Vector2 aquariumPosition = PositionMapper(Aquarium.Location);
            Vector2 axis = PositionMapper(Vector2.Zero);

            DrawLine(spriteBatch, lineConnector, 2, Color.LightGray, predatorPosition, new Vector2(predatorPosition.X, axis.Y + 3 * axisesPercentage * rectangle.Y));
            DrawLine(spriteBatch, lineConnector, 2, Color.LightGray, predatorPosition, new Vector2(axis.X - 4 * axisesPercentage * rectangle.X, predatorPosition.Y));

            DrawLine(spriteBatch, lineConnector, 2, Color.LightGray, preyPosition, new Vector2(preyPosition.X, axis.Y + 3 * axisesPercentage * rectangle.Y));
            DrawLine(spriteBatch, lineConnector, 2, Color.LightGray, preyPosition, new Vector2(axis.X - 4 * axisesPercentage * rectangle.X, preyPosition.Y));

            DrawLine(spriteBatch, lineConnector, 2, Color.LightGray, aquariumPosition, new Vector2(aquariumPosition.X, axis.Y + 3 * axisesPercentage * rectangle.Y));
            DrawLine(spriteBatch, lineConnector, 2, Color.LightGray, aquariumPosition, new Vector2(axis.X - 4 * axisesPercentage * rectangle.X, aquariumPosition.Y));

            spriteBatch.DrawString(labelsFont, (Math.Round(Predator.Location.X, 1) + ""), new Vector2(predatorPosition.X - labelsFont.MeasureString((Math.Round(Predator.Location.X, 1) + "")).X / 2, axis.Y + 2 * axisesPercentage * rectangle.Height + (Math.Max(labelsFont.MeasureString((Math.Round(Prey.Location.X, 1) + "")).Y, labelsFont.MeasureString((Math.Round(Aquarium.Location.X, 1) + "")).Y))), Color.Red, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(labelsFont, (Math.Round(Predator.Location.Y, 1) + ""), new Vector2(axis.X - 2 * axisesPercentage * rectangle.Height - labelsFont.MeasureString((Math.Round(Predator.Location.Y, 1) + "")).X - (Math.Max(labelsFont.MeasureString((Math.Round(Prey.Location.X, 1) + "")).X, labelsFont.MeasureString((Math.Round(Aquarium.Location.X, 1) + "")).X)), predatorPosition.Y - labelsFont.MeasureString((Math.Round(Predator.Location.Y, 1) + "")).Y / 2), Color.Red, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

            spriteBatch.DrawString(labelsFont, (Math.Round(Prey.Location.X, 1) + ""), new Vector2(preyPosition.X - labelsFont.MeasureString((Math.Round(Prey.Location.X, 1) + "")).X / 2, axis.Y + 2 * axisesPercentage * rectangle.Height), Color.Red, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(labelsFont, (Math.Round(Prey.Location.Y, 1) + ""), new Vector2(axis.X - 2 * axisesPercentage * rectangle.Height - labelsFont.MeasureString((Math.Round(Prey.Location.Y, 1) + "")).X, preyPosition.Y - labelsFont.MeasureString((Math.Round(Prey.Location.Y, 1) + "")).Y / 2), Color.Red, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

            spriteBatch.DrawString(labelsFont, (Math.Round(Aquarium.Location.X, 1) + ""), new Vector2(aquariumPosition.X - labelsFont.MeasureString((Math.Round(Aquarium.Location.X, 1) + "")).X / 2, axis.Y + 2 * axisesPercentage * rectangle.Height), Color.Red, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(labelsFont, (Math.Round(Aquarium.Location.Y, 1) + ""), new Vector2(axis.X - 2 * axisesPercentage * rectangle.Height - labelsFont.MeasureString((Math.Round(Aquarium.Location.Y, 1) + "")).X, aquariumPosition.Y - labelsFont.MeasureString((Math.Round(Aquarium.Location.Y, 1) + "")).Y / 2), Color.Red, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

        }

        public Vector2 GetPixelsPerMeter()
        {
            return pixelsPerMeter;
        }

    }
}

