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
    /// This Class is responsible for fully generating the GUI of Experiment2
    /// </summary>
    /// <remarks>
    /// <para>AUTHOR: Mohamed Alzayat, Mohamed Abdelazim </para>   
    /// <para>DATE WRITTEN: April, 20 </para>
    /// <para>DATE MODIFIED: April, 24  </para>
    /// </remarks>
    class Experiment2 : Mechanect.Common.GameScreen
    {



        VoiceCommands voiceCommand;
        User2 user;
        Boolean aquariumReached;
        MKinect mKinect;
        Button button;
        Vector2 buttonPosition;
        
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
        private Environment2 env;

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
        public Experiment2(User2 user, MKinect mKinect)
        {
            env = new Environment2();

            this.user = user;
            this.mKinect = mKinect;
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
            startAquariumPosition = MapPointsToScreen(env.Predator.Location);
            preyPosition = MapPointsToScreen(env.Prey.Location);
            destinationAquariumPosition = MapPointsToScreen(env.Aquarium.Location);

            // giving initial value for the predator throug the method that will be used to update it's position
            LoadPredatorWithNewPosition(env.Predator.Location);

            //Loading Fonts
            spriteFont = Content.Load<SpriteFont>("Ariel");
            velAngleFont = Content.Load<SpriteFont>("angleVelFont");

            // zayat you can edit the button place as you wish
            buttonPosition = new Vector2(screenWidth - screenWidth / 5, screenHeight -screenHeight / 5);
            button = new OKButton(Content, buttonPosition, screenWidth, screenHeight);
            //TBC
           voiceCommand = new VoiceCommands(mKinect._KinectDevice, "ok");

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

            configureScreen();


            // modifying the textures scalings (final modification)
            bowlTextureScaling *= env.Aquarium.getHeight() * pixelsPerMeterY / bowlTexture.Height;
            predatorTextureScaling *= bowlTextureScaling;
            preyTextureScaling *= env.Prey.getHeight() * pixelsPerMeterY / preyTexture.Height;
            base.Initialize();
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

            if (Math.Abs(((float)ViewPort.Width / (float)ViewPort.Height) - ((float)4 / 3)) >= 0.01 || screenWidth < 800)
            {
                DialogResult result1 = MessageBox.Show("The current Resolution is not acceptable! However, we will do our best!",
                    "Resolution Error",
                     MessageBoxButtons.OK);

            }

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
                backgroundTextureScaling =1;
            }

            // Getting the maximum possible difference between the experiment objects
            float maxDifferenceX = env.Aquarium.Location.X - env.Predator.Location.X;
            float maxDifferenceY = Math.Max(env.Prey.Location.Y, Math.Max(env.Aquarium.Location.Y, env.Predator.Location.Y)) - Math.Min(env.Prey.Location.Y, Math.Min(env.Aquarium.Location.Y, env.Predator.Location.Y));
            // Mapping the meters to pixels to configure how will the real world be mapped to the screen
            pixelsPerMeterX = (float)screenWidth * xDrawingPercentage / maxDifferenceX;
            pixelsPerMeterY = (float)screenHeight * yDrawingPercentage / maxDifferenceY;
            //To make the x and y axis have the same scaling
            pixelsPerMeterX = Math.Min(pixelsPerMeterX,pixelsPerMeterY);
            pixelsPerMeterY = Math.Min(pixelsPerMeterX, pixelsPerMeterY);

            // Assigning the Translation to be added to all points when mapping
            translationX = screenWidth * yAxisPercentage;
            translationY = screenHeight * xAxisPercentage;

            //Making all the scalings relative to the background scaling
            velocityTextureScaling *= backgroundTextureScaling;
            angleTextureScaling *= backgroundTextureScaling;

            predatorTextureScaling *= backgroundTextureScaling;
            preyTextureScaling *= backgroundTextureScaling;
            bowlTextureScaling *= backgroundTextureScaling;



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
            bowlTexture = Content.Load<Texture2D>("Textures/Experiment2/ImageSet1/bowl2");
            bowlTexture = Content.Load<Texture2D>("Textures/Experiment2/ImageSet1/Fishbowl");
            grayTexture = Content.Load<Texture2D>("Textures/Experiment2/ImageSet1/GrayScreen");
            velocityTexture = Content.Load<Texture2D>("Textures/Experiment2/ImageSet1/VelocityGauge");
            angleTexture = Content.Load<Texture2D>("Textures/Experiment2/ImageSet1/AngleGauge");
            lineConnector = new Texture2D(ScreenManager.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            lineConnector.SetData(new[] { Color.Gray });

            // Initial scalings
            backgroundTextureScaling = 1;
            xyAxisTextureScaling = 1;
            preyTextureScaling =1f;
            bowlTextureScaling = 1f;

            predatorTextureScaling = 2f;
            velocityTextureScaling = 0.7f;
            angleTextureScaling = 1.25f;

            xAxisPercentage = 0.05f;
            yAxisPercentage = 0.05f;

            xDrawingPercentage = 0.75f;
            yDrawingPercentage = 0.55f;
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
            backgroundTextureScaling = 1f;
            
            xyAxisTextureScaling = 1f;
            preyTextureScaling = 0.05f;
            bowlTextureScaling = 0.05f;

            predatorTextureScaling = 0.2f;
            velocityTextureScaling = 0.3f;
            angleTextureScaling = 0.5f;

            xAxisPercentage = 0.04f;
            yAxisPercentage = 0.056f;

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

            DrawEnvironment();
            if (grayScreen)
            {
                DrawGrayScreen();
            }
            DrawAngVelLabels();
            DrawConnectors(new object[3][] {new object[4]{env.Predator.Location,bowlTexture, startAquariumPosition, bowlTextureScaling},new object[4]{env.Prey.Location,preyTexture,preyPosition,preyTextureScaling},new object[4]{env.Aquarium.Location,bowlTexture,destinationAquariumPosition,bowlTextureScaling}});


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
            SpriteBatch.Draw(predatorTexture, DrawAtRectangleMidPoint(predatorTexture, predatorPosition, predatorTextureScaling), null, Color.White, MathHelper.ToRadians((float)env.Predator.Angle), Vector2.Zero, predatorTextureScaling, SpriteEffects.None, 0f);
            SpriteBatch.Draw(bowlTexture, DrawAtRectangleMidPoint(bowlTexture, destinationAquariumPosition, bowlTextureScaling), null, Color.White, 0f, Vector2.Zero, bowlTextureScaling, SpriteEffects.None, 0f);
            string meters = "meters";
            SpriteBatch.DrawString(velAngleFont, meters, new Vector2(screenWidth * yAxisPercentage - 0.75f * spriteFont.MeasureString(meters).Y, 2 * spriteFont.MeasureString(meters).X * backgroundTextureScaling), Color.Red, MathHelper.ToRadians(-90), Vector2.Zero, 2 * backgroundTextureScaling, SpriteEffects.None, 0f);
            SpriteBatch.DrawString(velAngleFont, meters, new Vector2(screenWidth - spriteFont.MeasureString(meters).X / 1.75f, screenHeight - spriteFont.MeasureString(meters).Y / 1.5f), Color.Red, 0f, Vector2.Zero, 2 * backgroundTextureScaling, SpriteEffects.None, 0f);
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
            SpriteBatch.Draw(velocityTexture, new Vector2(screenWidth*yAxisPercentage,screenHeight*xAxisPercentage), null, Color.White, 0f, Vector2.Zero, velocityTextureScaling, SpriteEffects.None, 0f);
            SpriteBatch.Draw(angleTexture, new Vector2(screenWidth- screenWidth * yAxisPercentage- angleTexture.Width*angleTextureScaling, screenHeight * xAxisPercentage), null, Color.White, 0f, Vector2.Zero, angleTextureScaling, SpriteEffects.None, 0f);
            string testString = "Test angle and Velocity";
            string sayString = "Say 'GO' or press OK";
            SpriteBatch.DrawString(spriteFont, testString, new Vector2((screenWidth / 2), 0), Color.Red, 0f, new Vector2((screenWidth / 4), 0), backgroundTextureScaling, SpriteEffects.None, 0f);
            SpriteBatch.DrawString(spriteFont, sayString, new Vector2((screenWidth / 2), spriteFont.MeasureString(testString).Y /*+ spriteFont.MeasureString(sayString).Y*/), Color.Red, 0f, new Vector2((screenWidth / 4), 0), backgroundTextureScaling, SpriteEffects.None, 0f);
          
            SpriteBatch.End();
            // ask hegazy to make it scalable !
            button.draw(SpriteBatch);
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
            String velString = "Velocity = " + env.Velocity;
            String angString = "Angle = " + env.Angle;
            SpriteBatch.Begin();

            if (grayScreen)
            {
                SpriteBatch.DrawString(velAngleFont, velString, new Vector2(screenWidth * yAxisPercentage + velocityTexture.Width * velocityTextureScaling - spriteFont.MeasureString(velString).X * velocityTextureScaling / 2, screenHeight * xAxisPercentage + velocityTexture.Height * velocityTextureScaling + spriteFont.MeasureString(velString).Y * velocityTextureScaling / 2), Color.Red, 0f, new Vector2(velocityTexture.Width * velocityTextureScaling / 2, velocityTexture.Height * velocityTextureScaling / 2), velocityTextureScaling * 2, SpriteEffects.None, 0f);
                SpriteBatch.DrawString(velAngleFont, angString, new Vector2(screenWidth - (screenWidth * yAxisPercentage + angleTexture.Width * angleTextureScaling - spriteFont.MeasureString(angString).X * velocityTextureScaling), screenHeight * xAxisPercentage + angleTexture.Height * angleTextureScaling + spriteFont.MeasureString(angString).Y * velocityTextureScaling), Color.Red, 0f, new Vector2(angleTexture.Width * angleTextureScaling / 2, angleTexture.Height * angleTextureScaling / 2), velocityTextureScaling * 2, SpriteEffects.None, 0f);
             }
            else
            {
                SpriteBatch.DrawString(velAngleFont, velString + env.Velocity, new Vector2(screenWidth - spriteFont.MeasureString(velString + angString).X / 2, 0), Color.Red);
                SpriteBatch.DrawString(velAngleFont, angString + env.Angle, new Vector2(screenWidth - spriteFont.MeasureString(angString).X / 2, 0), Color.Red);
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
        /// <param name="objects">takes an array of arrays of objects where each inner array contains the information of one of the objects to be drawn as follows: [positionToBeWrittenOnXandYAxises,Texture,PositionOfObject,ScalingOfTexture] </param>
        private void DrawConnectors(object[][] objects)
        {SpriteBatch.Begin();
                   
                for (int i = 0; i < objects.Length; i++)
                {
                    DrawLine(SpriteBatch, lineConnector, 2f, Color.LightGray, DrawAxisConnector(((Texture2D)objects[i][1]), ((Vector2)objects[i][2]), ((float)objects[i][3])), new Vector2(screenWidth * xAxisPercentage - spriteFont.MeasureString(((Vector2)objects[i][0]).X + "").Y / 2 * backgroundTextureScaling, DrawAxisConnector(((Texture2D)objects[i][1]), ((Vector2)objects[i][2]), ((float)objects[i][3])).Y));
                    SpriteBatch.DrawString(velAngleFont, ((Vector2)objects[i][0]).Y + "", DrawValueOnAxis(((Texture2D)objects[i][1]), ((Vector2)objects[i][2]), ((float)objects[i][3]), true), Color.Red, 0f, Vector2.Zero, velocityTextureScaling * 2, SpriteEffects.None, 0f);
                    DrawLine(SpriteBatch, lineConnector, 2f, Color.LightGray, DrawAxisConnector(((Texture2D)objects[i][1]), ((Vector2)objects[i][2]), ((float)objects[i][3])), new Vector2(DrawAxisConnector(((Texture2D)objects[i][1]), ((Vector2)objects[i][2]), ((float)objects[i][3])).X, screenHeight - spriteFont.MeasureString(((Vector2)objects[i][0]).X + "").Y / 2 * backgroundTextureScaling));
                    SpriteBatch.DrawString(velAngleFont, ((Vector2)objects[i][0]).X + "", DrawValueOnAxis(((Texture2D)objects[i][1]), ((Vector2)objects[i][2]), ((float)objects[i][3]), false), Color.Red, 0f, Vector2.Zero, velocityTextureScaling * 2, SpriteEffects.None, 0f);
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
                return new Vector2(2, position.Y + texture.Height * scale /2 - spriteFont.MeasureString(position.Y + "").Y / 4);
            else
                return new Vector2(position.X + texture.Width * scale / 2 - spriteFont.MeasureString(position.X + "").X/16, screenHeight - (spriteFont.MeasureString(position.X + "").Y / 1.4f) * backgroundTextureScaling);
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

     
        

  
        private Boolean isPreyEaten()
        {
            Boolean isHit = false;
            Vector2 position = env.Predator.getLocation();
            Prey prey = env.Prey;
            if (position.X >= prey.Location.X - prey.Width / 2
                && position.X <= prey.Location.X + prey.Width / 2
                && position.Y >= prey.Location.Y - prey.Length / 2
                && position.Y <= prey.Location.Y + prey.Length / 2)
                isHit = true;
            return isHit;
        }

        private Boolean isAquariumReached()
        {
            Boolean isReached = false;
            Vector2 position = env.Predator.getLocation();
            Aquarium aquarium = env.Aquarium;
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
        /// <para>DATE WRITTEN: April, 20 </para>
        /// <para>DATE MODIFIED: April, 21  </para>
        /// </remarks>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, bool covered)
        { //camera.Update();
            //TBC


            if (!grayScreen && user.MeasuredVelocity != 0 && !aquariumReached)
            {
                if (env.Predator.Velocity == null)
                    env.Predator.Velocity = new Vector2((float)(user.MeasuredVelocity * Math.Cos(user.MeasuredAngle)), (float)(user.MeasuredVelocity * Math.Sin(user.MeasuredAngle)));
                env.Predator.UpdatePosition(gameTime);
                if (!preyEaten) preyEaten = isPreyEaten();
                if (!aquariumReached) aquariumReached = isAquariumReached();
                if (aquariumReached)
                {
                    env.Predator.Location = new Vector2(env.Aquarium.Location.X, env.Aquarium.Location.Y);
                    env.Predator.Velocity = Vector2.Zero;
                }
            }

            else if (button.isClicked() || voiceCommand.getHeared("ok"))
            {
                grayScreen = false;
                button = null;
                voiceCommand = null;
                user.MeasuredAngle = 0;
                user.MeasuredVelocity = 0;
            }

            else
            {
                user.MeasureVelocityAndAngle();
            }

            base.Update(gameTime, covered);
        }



    }
}
