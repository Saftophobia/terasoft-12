using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Mechanect.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Mechanect.Cameras;
using Mechanect.Common;
using System.Windows.Forms;

namespace Mechanect.Screens
{

    /// <summary>
    /// This Class is responsible for running Experiment2 and displaying it's GUI
    /// </summary>
    /// <remarks>
    /// <para>AUTHOR: Mohamed Alzayat, Mohamed Abdelazim </para>
    /// </remarks>
    class Experiment2 : Mechanect.Common.GameScreen
    {



        VoiceCommands voiceCommand;
        User2 user;
        Boolean aquariumReached;
        MKinect mKinect;
        Button button;
        Vector2 buttonPosition;
        int tolerance;
        Boolean ended;
        int milliSeconds;
        Boolean isCopied;

        
        /// <summary>
        /// Instance Variables
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Alzayat </para>   
        /// <para>DATE WRITTEN: April, 20 </para>
        /// <para>DATE MODIFIED: April, 24  </para>
        /// </remarks>


        // Variables that will be used as getters; to get some inherited objects
        Viewport ViewPort
        {
            get
            {
                return ScreenManager.GraphicsDevice.Viewport;
            }
        }
        ContentManager Content
        {
            get
            {
                return ScreenManager.Game.Content;
            }
        }
        SpriteBatch SpriteBatch
        {
            get
            {
                return ScreenManager.SpriteBatch;
            }
        }

        // An instance of the environment2 Class (acts as an engine for this class)
        private Environment2 environment;

        // Variables for fontSprites
        private SpriteFont spriteFont;
        private SpriteFont velAngleFont;

        // Variables Contaiing the Textures Definintion 
        private Texture2D backgroundTexture;
        private Texture2D xyAxisTexture;
        private Texture2D grayTexture;
        private Texture2D predatorTexture;
        private Texture2D preyTexture;
        private Texture2D bowlTexture;
        private Texture2D velocityTexture;
        private Texture2D angleTexture;
        private Texture2D lineConnector;


        // Variables That will be used as scaling for the Textures
        private float backgroundTextureScaling;
        private float xyAxisTextureScaling;
        private float preyTextureScaling;
        private float bowlTextureScaling;
        private float grayTextureScaling;
        private float velocityTextureScaling;
        private float angleTextureScaling;
        private float predatorTextureScaling;


        // Variables that will change how the Gui will look

        // Variables defining the appearence of some objects
        private Boolean grayScreen = true;
        private Boolean preyEaten = false;
        private float pixelsPerMeterX;
        private float pixelsPerMeterY;

        // Variable defining the x-axis percentage (up from the whole screen)
        private float xAxisPercentage;
        // Variable defining the y-axis percentage (right from the whole screen)
        private float yAxisPercentage;
        // Variables defining the percentage of the screen to be drawn so that the notifications don't get corrupted with the actual experiment
        private float xDrawingPercentage;
        private float yDrawingPercentage;
        //Variables definng the translation that would be applied for any object when mapping from real world to the screen
        float translationX;
        float translationY;

        // Variables defining the screen Width and Height that will be used in drawing the objects of the experiment
        private int screenWidth;
        private int screenHeight;

        // Variables that will be used to know the positions of the main objects(predator,prey,aquariums) in the experiment
        private Vector2 predatorPosition;
        private Vector2 preyPosition;
        private Vector2 startAquariumPosition;
        private Vector2 destinationAquariumPosition;

        

        /// <summary>
        /// This is a constructor that will initialize the grphicsDeviceManager and define the Content directory.
        /// in adition to initializing some instance variables
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Alzayat </para>   
        /// <para>DATE WRITTEN: April, 20 </para>
        /// <para>DATE MODIFIED: April, 20  </para>
        /// </remarks>
        /// <param name="user">Takes an instance of User2 </param>
        /// <param name="mKinect">takes an instance of mKinect</param>
        public Experiment2(User2 user)
        {
            tolerance = 20;
            environment = new Environment2(tolerance);

            this.user = user;
            this.mKinect = user.Kinect;
            isCopied = false;
        }



        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of the Content (all Textures)
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Alzayat </para>   
        /// <para>DATE WRITTEN: April, 20 </para>
        /// <para>DATE MODIFIED: April, 24  </para>
        /// </remarks>

        public override void LoadContent()
        {
            // initializing the screen width and height (just initial values and will be changed later)
            screenWidth = ViewPort.Width;
            screenHeight = ViewPort.Height;

            //Load the theme by spesifying an image set
            LoadTextures(1);
            //These are variables that won't change during the game will not be u
            //if ((environment.Predator.Location.X < Vector2.Zero.X || environment.Prey.Location.X < Vector2.Zero.X || environment.Aquarium.Location.X < Vector2.Zero.X) || (environment.Predator.Location.Y < Vector2.Zero.Y || environment.Prey.Location.Y < Vector2.Zero.Y || environment.Aquarium.Location.Y < Vector2.Zero.Y))
            //    MessageBox.Show("Sorry these Variables are not representable",
            //        "Wrong Points Representation Error",
            //         MessageBoxButtons.OK);
            configureScreen();


            // modifying the textures scalings (final modification)
            bowlTextureScaling *= (float)environment.Aquarium.getHeight() * (float)pixelsPerMeterY / (float)bowlTexture.Height;
            predatorTextureScaling *= (float)bowlTextureScaling;
            preyTextureScaling *= (float)environment.Prey.getHeight() * (float)pixelsPerMeterY / (float)preyTexture.Height;
           
            startAquariumPosition = MapPointsToScreen(environment.Predator.Location);
            preyPosition = MapPointsToScreen(environment.Prey.Location);
            destinationAquariumPosition = MapPointsToScreen(environment.Aquarium.Location);

            // giving initial value for the predator throug the method that will be used to update it's position
            LoadPredatorWithNewPosition(environment.Predator.Location);

            //Loading Fonts
            spriteFont = Content.Load<SpriteFont>("Ariel");
            velAngleFont = Content.Load<SpriteFont>("Ariel");

            buttonPosition = new Vector2(screenWidth - screenWidth / 2.7f, 0);
            button = Tools3.OKButton(Content, buttonPosition, screenWidth, screenHeight, user);
            //TBC
            voiceCommand = new VoiceCommands(mKinect._KinectDevice, "go");

        }


        /// <summary>
        /// This Method will be used to update the value of the predator position in each frame
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Alzayat </para>   
        /// <para>DATE WRITTEN: April, 22 </para>
        /// <para>DATE MODIFIED: April, 24  </para>
        /// </remarks>
        /// <param name="predator">Takes the vector2 position of the predator</param>

        private void LoadPredatorWithNewPosition(Vector2 predator)
        {
            predatorPosition = MapPointsToScreen(predator);
        }

        /// <summary>
        /// Allows the game to initialize all the textures 
        /// Initializing the Background and x,y Axises textures
        /// And give initial scaling according to the screen configuration used
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Alzayat </para>   
        /// <para>DATE WRITTEN: April, 20 </para>
        /// <para>DATE MODIFIED: April, 24  </para>
        /// </remarks>

        public void LoadTextures(int setNumber)
        {
            switch (setNumber)
            {
                case 1: ImageSet1(); break;
                default: ImageSet2(); break;
            }

             //base.Initialize();
        }


        /// <summary>
        /// This method checks if the resolution is acceptable or not, then configures the final screen width and height.
        /// And give initial scaling according to the screen configuration used
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Alzayat </para>   
        /// <para>DATE WRITTEN: April, 22 </para>
        /// <para>DATE MODIFIED: April, 24  </para>
        /// </remarks>
        private void configureScreen()
        {

            //if (Math.Abs(((float)ViewPort.Width / (float)ViewPort.Height) - ((float)4 / 3)) >= 0.01 || screenWidth < 800)
            //{
            //    DialogResult result1 = MessageBox.Show("The current Resolution is not acceptable! However, we will do our best!",
            //        "Resolution Error",
            //         MessageBoxButtons.OK);

            //}

            if (ViewPort.Width > ViewPort.Height)
            {
                backgroundTextureScaling = ((float)ViewPort.Height / (float)backgroundTexture.Height);
                xyAxisTextureScaling = ((float)ViewPort.Height / (float)xyAxisTexture.Height);
                grayTextureScaling = ((float)ViewPort.Height / (float)grayTexture.Height);
            }
            else
            {

                backgroundTextureScaling = ((float)ViewPort.Width / (float)backgroundTexture.Width);
                xyAxisTextureScaling = ((float)ViewPort.Width / (float)xyAxisTexture.Width);
                grayTextureScaling = ((float)ViewPort.Width / (float)grayTexture.Width);

            }

            //final screen width and height
            screenWidth = Convert.ToInt32(backgroundTexture.Width * backgroundTextureScaling);
            screenHeight = Convert.ToInt32(backgroundTexture.Height * backgroundTextureScaling);

            // backgroundTecture scaling will be used as arefference to scale all other objects thus scaling is not allowed to be more than 1
            if (backgroundTextureScaling > 1)
            {
                backgroundTextureScaling = 1;
            }

            // Getting the maximum possible difference between the experiment objects
            float maxDifferenceX = environment.Aquarium.Location.X - environment.Predator.Location.X;
            float maxDifferenceY = Math.Max(environment.Prey.Location.Y, Math.Max(environment.Aquarium.Location.Y, environment.Predator.Location.Y));// - Math.Min(environment.Prey.Location.Y, Math.Min(environment.Aquarium.Location.Y, environment.Predator.Location.Y));
            // Mapping the meters to pixels to configure how will the real world be mapped to the screen
            pixelsPerMeterY = (float)screenHeight * yDrawingPercentage / maxDifferenceY;
            pixelsPerMeterX = (float)screenWidth * xDrawingPercentage / maxDifferenceX;
            // pixelsPerMeterY = (float)screenHeight * yDrawingPercentage / maxDifferenceY;
            //To make the x and y axis have the same scaling
            pixelsPerMeterX = Math.Min(pixelsPerMeterX, pixelsPerMeterY);
            pixelsPerMeterY = Math.Min(pixelsPerMeterX, pixelsPerMeterY);

            // Assigning the Translation to be added to all points when mapping
            translationX = screenWidth * yAxisPercentage;
            translationY = screenHeight * xAxisPercentage;

            //Making all the scalings relative to the background scaling
            velocityTextureScaling *= backgroundTextureScaling;
            angleTextureScaling *= backgroundTextureScaling;

            //predatorTextureScaling *= backgroundTextureScaling;
            //preyTextureScaling *= backgroundTextureScaling;
            //bowlTextureScaling *= backgroundTextureScaling;



        }
        /// <summary>
        /// Allows the game to initialize all the textures 
        /// Initializing the Background and x,y Axises textures
        /// Using a set of images called ImageSet1
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Alzayat </para>   
        /// <para>DATE WRITTEN: April, 20 </para>
        /// <para>DATE MODIFIED: April, 22  </para>
        /// </remarks>
        private void ImageSet1()
        {

            backgroundTexture = Content.Load<Texture2D>("Textures/Experiment2/ImageSet1/background");
            xyAxisTexture = Content.Load<Texture2D>("Textures/Experiment2/ImageSet1/xyAxis");
            predatorTexture = Content.Load<Texture2D>("Textures/Experiment2/ImageSet1/fish");
            preyTexture = Content.Load<Texture2D>("Textures/Experiment2/ImageSet1/worm");
            //bowlTexture = Content.Load<Texture2D>("Textures/Experiment2/ImageSet1/bowl2");
            bowlTexture = Content.Load<Texture2D>("Textures/Experiment2/ImageSet1/Fishbowl");
            grayTexture = Content.Load<Texture2D>("Textures/Experiment2/ImageSet1/GrayScreen");
            velocityTexture = Content.Load<Texture2D>("Textures/Experiment2/ImageSet1/VelocityGauge");
            angleTexture = Content.Load<Texture2D>("Textures/Experiment2/ImageSet1/AngleGauge");
            lineConnector = new Texture2D(ScreenManager.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            lineConnector.SetData(new[] { Color.Gray });

            // Initial scalings
            backgroundTextureScaling = 1;
            xyAxisTextureScaling = 1;
            preyTextureScaling = 0.75f;
            bowlTextureScaling = 3f;

            predatorTextureScaling = 1.5f;
            velocityTextureScaling = 0.7f;
            angleTextureScaling = 1.25f;

            xAxisPercentage = 0.05f;
            yAxisPercentage = 0.05f;

            xDrawingPercentage = 0.75f;
            yDrawingPercentage = 0.5f;
        }
        /// <summary>
        /// Allows the game to initialize all the textures 
        /// Initializing the Background and x,y Axises textures
        /// Using a set of images called ImageSet2
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Alzayat </para>   
        /// <para>DATE WRITTEN: April, 20 </para>
        /// <para>DATE MODIFIED: April, 22  </para>
        /// </remarks>
        private void ImageSet2()
        {
            backgroundTexture = Content.Load<Texture2D>("Textures/Experiment2/ImageSet2/sea2");
            xyAxisTexture = Content.Load<Texture2D>("Textures/Experiment2/ImageSet2/xyAxis");
            predatorTexture = Content.Load<Texture2D>("Textures/Experiment2/ImageSet2/fish");
            preyTexture = Content.Load<Texture2D>("Textures/Experiment2/ImageSet2/Worm");
            bowlTexture = Content.Load<Texture2D>("Textures/Experiment2/ImageSet2/bowl");
            grayTexture = Content.Load<Texture2D>("Textures/Experiment2/ImageSet2/GrayScreen");
            velocityTexture = Content.Load<Texture2D>("Textures/Experiment2/ImageSet2/VelocityGauge");
            angleTexture = Content.Load<Texture2D>("Textures/Experiment2/ImageSet2/AngleGauge");
            lineConnector = new Texture2D(ScreenManager.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            lineConnector.SetData(new[] { Color.Gray });

            // Initial scalings
            backgroundTextureScaling = 0.75f;
            xyAxisTextureScaling = 1;
            preyTextureScaling = 0.5f;
            bowlTextureScaling = 2f;

            predatorTextureScaling = 1.5f;
            velocityTextureScaling = 0.5f;
            angleTextureScaling = 1f;

            xAxisPercentage = 0.05f;
            yAxisPercentage = 0.05f;

            xDrawingPercentage = 0.75f;
            yDrawingPercentage = 0.55f;
        }



        /// <summary>
        /// This is to be called when the game should draw itself.
        /// Here all the GUI is drawn
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Alzayat </para>   
        /// <para>DATE WRITTEN: April, 20 </para>
        /// <para>DATE MODIFIED: April, 22  </para>
        /// </remarks>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(Color.CornflowerBlue);
            LoadPredatorWithNewPosition(environment.Predator.Location);
            DrawEnvironment();
            if (grayScreen)
            {
                DrawGrayScreen();
            }
            if (ended && milliSeconds > 1000)
                Tools3.DislayIsWin(ScreenManager.SpriteBatch, Content, new Vector2(ViewPort.Width/2, ViewPort.Height/2), aquariumReached && preyEaten);
            DrawAngVelLabels();
            //each  array contains the information of one of the objects needing connectors to be drawn as follows: [positionToBeWrittenOnXandYAxises,Texture,PositionOfObject,ScalingOfTexture]
            object[] startAquariumArray = new object[4] { environment.Predator.Location, bowlTexture, startAquariumPosition, bowlTextureScaling };
            object[] preyArray = new object[4]  {environment.Prey.Location,preyTexture,preyPosition,preyTextureScaling};
            object[] destAquariumArray = new object[4] { environment.Aquarium.Location, bowlTexture, destinationAquariumPosition, bowlTextureScaling };
            DrawConnectors(new object[3][] {startAquariumArray,preyArray,destAquariumArray});


        }

        /// <summary>
        /// This Method is to be called whenDrawing The basic environment elements.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Alzayat </para>   
        /// <para>DATE WRITTEN: April, 22 </para>
        /// <para>DATE MODIFIED: April, 22  </para>
        /// </remarks>
        private void DrawEnvironment()
        {
            SpriteBatch.Begin();
            SpriteBatch.Draw(backgroundTexture, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, backgroundTextureScaling, SpriteEffects.None, 0f);
            SpriteBatch.Draw(xyAxisTexture, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, xyAxisTextureScaling, SpriteEffects.None, 0f);
            SpriteBatch.Draw(bowlTexture, DrawAtRectangleMidPoint(bowlTexture, startAquariumPosition, bowlTextureScaling), null, Color.White, 0f, Vector2.Zero, bowlTextureScaling, SpriteEffects.None, 0f);
            //TBA >> degrees
            SpriteBatch.Draw(predatorTexture, DrawAtRectangleMidPoint(predatorTexture, predatorPosition, predatorTextureScaling), null, Color.White, MathHelper.ToRadians(-1*(float)environment.Predator.Angle), Vector2.Zero, predatorTextureScaling, SpriteEffects.None, 0f);
            SpriteBatch.Draw(bowlTexture, DrawAtRectangleMidPoint(bowlTexture, destinationAquariumPosition, bowlTextureScaling), null, Color.White, 0f, Vector2.Zero, bowlTextureScaling, SpriteEffects.None, 0f);
            string meters = "meters";
            SpriteBatch.DrawString(velAngleFont, meters, new Vector2(screenWidth * yAxisPercentage - 0.75f * spriteFont.MeasureString(meters).Y, 2 * spriteFont.MeasureString(meters).X * backgroundTextureScaling), Color.Red, MathHelper.ToRadians(-90), Vector2.Zero, backgroundTextureScaling, SpriteEffects.None, 0f);
            SpriteBatch.DrawString(velAngleFont, meters, new Vector2(screenWidth - spriteFont.MeasureString(meters).X / 1.55f, screenHeight - spriteFont.MeasureString(meters).Y / 1.5f), Color.Red, 0f, Vector2.Zero,  backgroundTextureScaling, SpriteEffects.None, 0f);
            if (!preyEaten)
                SpriteBatch.Draw(preyTexture, DrawAtRectangleMidPoint(preyTexture, preyPosition, preyTextureScaling), null, Color.White, 0f, Vector2.Zero, preyTextureScaling, SpriteEffects.None, 0f);
            SpriteBatch.End();
        }

        /// <summary>
        /// This method will draw a gray Screen with all it's components
        /// That is the screen to be displayed when the user will be testing the velocity and angle
        /// </summary>
        ///  <remarks>
        /// <para>AUTHOR: Mohamed Alzayat </para>   
        /// <para>DATE WRITTEN: April, 21 </para>
        /// <para>DATE MODIFIED: April, 22  </para>
        /// </remarks>
        private void DrawGrayScreen()
        {
            SpriteBatch.Begin();

            SpriteBatch.Draw(grayTexture, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, grayTextureScaling, SpriteEffects.None, 0f);
            SpriteBatch.Draw(velocityTexture, new Vector2(screenWidth * yAxisPercentage, screenHeight * xAxisPercentage), null, Color.White, 0f, Vector2.Zero, velocityTextureScaling, SpriteEffects.None, 0f);
            SpriteBatch.Draw(angleTexture, new Vector2(screenWidth - screenWidth * yAxisPercentage - angleTexture.Width * angleTextureScaling, screenHeight * xAxisPercentage), null, Color.White, 0f, Vector2.Zero, angleTextureScaling, SpriteEffects.None, 0f);
            string testString = "Test angle and Velocity";
            string sayString = "Say 'GO' or press OK";
            SpriteBatch.DrawString(spriteFont, testString, new Vector2((screenWidth / 2), 0), Color.Red, 0f, new Vector2((screenWidth / 4), 0), backgroundTextureScaling, SpriteEffects.None, 0f);
            SpriteBatch.DrawString(spriteFont, sayString, new Vector2((screenWidth / 2), spriteFont.MeasureString(testString).Y /*+ spriteFont.MeasureString(sayString).Y*/), Color.Red, 0f, new Vector2((screenWidth / 4), 0), backgroundTextureScaling, SpriteEffects.None, 0f);

            SpriteBatch.End();
            // ask hegazy to make it scalable !
            button.Draw(SpriteBatch);
        }

        /// <summary>
        /// This Method is to be called whenDrawing The angle and velocity Labels.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Alzayat </para>   
        /// <para>DATE WRITTEN: April, 22 </para>
        /// <para>DATE MODIFIED: April, 22  </para>
        /// </remarks>
        private void DrawAngVelLabels()
        {
            String velString = "Velocity = " + Math.Round(user.MeasuredVelocity,2);
            String angString = "Angle = " + Math.Round(user.MeasuredAngle,2);
            SpriteBatch.Begin();

            if (grayScreen)
            {
                SpriteBatch.DrawString(velAngleFont, velString, new Vector2(screenWidth * yAxisPercentage + velocityTexture.Width * velocityTextureScaling - (spriteFont.MeasureString(velString).X * velocityTextureScaling) , screenHeight * xAxisPercentage + velocityTexture.Height * velocityTextureScaling - (spriteFont.MeasureString(velString).Y * velocityTextureScaling) / 2), Color.Red, 0f, new Vector2(velocityTexture.Width * velocityTextureScaling / 2, velocityTexture.Height * velocityTextureScaling / 2), velocityTextureScaling , SpriteEffects.None, 0f);
                SpriteBatch.DrawString(velAngleFont, angString, new Vector2(screenWidth - (screenWidth * yAxisPercentage + angleTexture.Width * angleTextureScaling - spriteFont.MeasureString(angString).X * velocityTextureScaling/2), screenHeight * xAxisPercentage + angleTexture.Height * angleTextureScaling - spriteFont.MeasureString(angString).Y * velocityTextureScaling/4), Color.Red, 0f, new Vector2(angleTexture.Width * angleTextureScaling / 2, angleTexture.Height * angleTextureScaling / 2), velocityTextureScaling , SpriteEffects.None, 0f);
            }
            else
            {
                SpriteBatch.DrawString(velAngleFont, velString + Math.Round(environment.Velocity,2), new Vector2(screenWidth - spriteFont.MeasureString(velString + angString).X / 2, 0), Color.Red, 0f, new Vector2(spriteFont.MeasureString(velString + environment.Velocity+"                    ").X / 2, 0), velocityTextureScaling, SpriteEffects.None, 0f);
                SpriteBatch.DrawString(velAngleFont, angString + Math.Round(environment.Angle,2), new Vector2(screenWidth - spriteFont.MeasureString(angString).X / 2, 0), Color.Red, 0f, new Vector2(spriteFont.MeasureString(velString + environment.Velocity).X / 2, 0), velocityTextureScaling, SpriteEffects.None, 0f);
            }
            SpriteBatch.End();
        }

        /// <summary>
        ///This Method takes a position and returns a new position that will be used in presenting the real world point on screen
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Alzayat </para>   
        /// <para>DATE WRITTEN: April, 22 </para>
        /// <para>DATE MODIFIED: April, 22  </para>
        /// </remarks>
        /// <param name="position">Takes the Vector2 position of the point to be mapped to this screen</param>
        /// <returns>A Vecto2 position to be drawed on the screen</returns>
        Vector2 MapPointsToScreen(Vector2 position)
        {
            return new Vector2((position.X * pixelsPerMeterX + translationX), (screenHeight - (position.Y * pixelsPerMeterY + translationY)));

        }
        /// <summary>
        /// This is to be called when the game needs drawing the  X,Y axis Connectors and position the values on the x,y Axises.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Alzayat </para>   
        /// <para>DATE WRITTEN: April, 22 </para>
        /// <para>DATE MODIFIED: April, 22  </para>
        /// </remarks>
        /// <param name="objects">takes an array of arrays of objects where each inner array contains the information of one of the objects ,needing connectors, to be drawn as follows: [positionToBeWrittenOnXandYAxises,Texture,PositionOfObject,ScalingOfTexture] </param>
        private void DrawConnectors(object[][] objects)
        {
            SpriteBatch.Begin();

            for (int i = 0; i < objects.Length; i++)
            {
                Vector2 position = DrawAxisConnector(((Texture2D)objects[i][1]), ((Vector2)objects[i][2]), ((float)objects[i][3]));
                Vector2 valuePositionX = DrawValueOnAxis(((Texture2D)objects[i][1]), ((Vector2)objects[i][2]), ((float)objects[i][3]), false);
                Vector2 valuePositionY = DrawValueOnAxis(((Texture2D)objects[i][1]), ((Vector2)objects[i][2]), ((float)objects[i][3]), true);
                DrawLine(SpriteBatch, lineConnector, 2f, Color.LightGray,position , new Vector2(screenWidth * xAxisPercentage - spriteFont.MeasureString(((Vector2)objects[i][0]).X + "").Y*backgroundTextureScaling, position.Y));
                SpriteBatch.DrawString(velAngleFont, (Math.Round(((Vector2)objects[i][0]).X,2) + ""), valuePositionX, Color.Red, 0f, Vector2.Zero, velocityTextureScaling, SpriteEffects.None, 0f);
                DrawLine(SpriteBatch, lineConnector, 2f, Color.LightGray,position, new Vector2(position.X, screenHeight - spriteFont.MeasureString(((Vector2)objects[i][0]).X + "").Y*backgroundTextureScaling ));
                SpriteBatch.DrawString(velAngleFont, (Math.Round(((Vector2)objects[i][0]).Y,2) + ""), valuePositionY, Color.Red, 0f, Vector2.Zero, velocityTextureScaling, SpriteEffects.None, 0f);
            }

            SpriteBatch.End();
        }

        /// <summary>
        /// Enables you to draw the X or Y Axis Connector with the object texture sent;.
        /// N.B: This Method doesn't draw, it just returns a position Vector2 that will be used in drawing.
        /// </summary>
        ///  <remarks>
        /// <para>AUTHOR: Mohamed Alzayat </para>   
        /// <para>DATE WRITTEN: April, 21 </para>
        /// <para>DATE MODIFIED: April, 22  </para>
        /// </remarks>
        /// <param name="texture">Texture that needs to be drawn with the position at it's center</param>
        /// <param name="position">The position of the center of the texture</param>
        /// <param name="scale">Takes the scaling of the Texture2D</param>
        /// <returns>A Vector2 that will be used as a position for drawing</returns>

        Vector2 DrawAxisConnector(Texture2D texture, Vector2 position, float scale)
        {
            position = DrawAtRectangleMidPoint(texture, position, scale);
            return new Vector2(position.X + texture.Width * scale / 2, position.Y + texture.Height * scale / 2);
        }
        /// <summary>
        /// Enables you to draw the X or Y AxisNumbers by specifiying the position vector of the number to be drawn
        /// N.B: This Method doesn't draw, it just returns a position Vector2 that will be used in drawing.
        /// </summary>
        ///  <remarks>
        /// <para>AUTHOR: Mohamed Alzayat </para>   
        /// <para>DATE WRITTEN: April, 22 </para>
        /// <para>DATE MODIFIED: April, 22  </para>
        /// </remarks>
        /// <param name="texture">Texture that needs its coordinate to be mapped on the x or y axis
        /// <param name="position">The position of the center of the texture</param>
        /// <param name="scale">Takes the scaling of the Texture2D</param>
        /// <param name="y">Gives a check to know if the axis used is x or y axis</param>
        /// <returns>A Vector2 that will be used as a position for drawing</returns>
        /// 
        Vector2 DrawValueOnAxis(Texture2D texture, Vector2 position, float scale, Boolean y)
        {
            position = DrawAtRectangleMidPoint(texture, position, scale);
            if (y)
                return new Vector2(2, position.Y + texture.Height * scale / 2 - spriteFont.MeasureString(position.Y + "").Y / 4);
            else
                return new Vector2(position.X + texture.Width * scale / 2 - spriteFont.MeasureString(position.X + "").X / 16, screenHeight - (spriteFont.MeasureString(position.X + "").Y / 1.4f) * backgroundTextureScaling);
        }
        /// <summary>
        /// This method will draw a gray line 
        /// implemented initially to connect the GUI objects with the x,y axises
        /// </summary>
        ///  <remarks>
        /// <para>AUTHOR: Mohamed Alzayat </para>   
        /// <para>DATE WRITTEN: April, 20 </para>
        /// <para>DATE MODIFIED: April, 20  </para>
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
        /// Enables you to draw the Texture2D by specifiying the position vector of it's center
        /// N.B: This Method doesn't draw, it just returns a position Vector2 that will be used in drawing.
        /// </summary>
        ///  <remarks>
        /// <para>AUTHOR: Mohamed Alzayat </para>   
        /// <para>DATE WRITTEN: April, 22 </para>
        /// <para>DATE MODIFIED: April, 22  </para>
        /// </remarks>
        /// <param name="texture">Texture that needs to be drawn with the position at it's center</param>
        /// <param name="position">The position of the center of the texture</param>
        /// <param name="scale">Takes the scaling of the Texture2D</param>
        /// <returns>A Vector2 that will be used as a position for drawing</returns>
        Vector2 DrawAtRectangleMidPoint(Texture2D texture, Vector2 position, float scale)
        {

            return new Vector2(position.X - (texture.Width * scale / 2), position.Y - (texture.Height * scale / 2));

        }





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
            Boolean isHit = false;
            Vector2 position = environment.Predator.getLocation();
            Prey prey = environment.Prey;
            if (position.X >= prey.Location.X - prey.Width / 2
                && position.X <= prey.Location.X + prey.Width / 2
                && position.Y >= prey.Location.Y - prey.Length / 2
                && position.Y <= prey.Location.Y + prey.Length / 2)
                isHit = true;
            return isHit;
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
            Boolean isReached = false;
            Vector2 position = environment.Predator.getLocation();
            Aquarium aquarium = environment.Aquarium;
            if (position.X >= aquarium.Location.X - aquarium.Width / 2
                && position.X <= aquarium.Location.X + aquarium.Width / 2
                && position.Y >= aquarium.Location.Y - aquarium.Length / 2
                && position.Y <= aquarium.Location.Y + aquarium.Length / 2)
                isReached = true;
            return isReached;
        }


        /// <summary>
        /// Runs at every frame, Updates game parameters and checks for user's actions
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed AbdelAzim </para>
        /// </remarks>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// <param name="covered">determines whether there exist another screen covering this one or not.</param>
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, bool covered)
        { 
            if (ended)
            {
                milliSeconds += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (milliSeconds > 3000)
                {
                    this.ExitScreen();
                    ScreenManager.AddScreen(new LastScreen(user, 2));
                    this.Remove();
                }
            }
            else
            {
                if (!grayScreen && user.MeasuredVelocity != 0 && !aquariumReached)
                {
                    if (!isCopied)
                    {
                        isCopied = true;
                        environment.Predator.Velocity = new Vector2((float)(user.MeasuredVelocity * Math.Cos(user.MeasuredAngle * Math.PI / 180)), (float)(user.MeasuredVelocity * Math.Sin(user.MeasuredAngle * Math.PI / 180)));
                    }
                    environment.Predator.UpdatePosition(gameTime);
                    if (!preyEaten) preyEaten = isPreyEaten();
                    if (!aquariumReached) aquariumReached = isAquariumReached();
                    if (aquariumReached)
                    {
                        environment.Predator.Location = new Vector2(environment.Aquarium.Location.X, environment.Aquarium.Location.Y);
                        environment.Predator.Velocity = Vector2.Zero;
                        ended = true;
                    }
                    else if (environment.Predator.Location.Y <= 0)
                    {
                        environment.Predator.Velocity = Vector2.Zero;
                        ended = true;
                    }

                }

                else
                {
                    if (button != null)
                    {
                        button.Update(gameTime);
                        if (button.IsClicked() || voiceCommand.getHeared("go"))
                        {
                            grayScreen = false;
                            button = null;
                            voiceCommand = null;
                            user.Reset();
                        }
                    }
                    user.setSkeleton();
                    if (user.USER != null && user.USER.Position.Z != 0)
                        user.MeasureVelocityAndAngle(gameTime);

                }
                base.Update(gameTime, covered);

            }

        }

    }
}
