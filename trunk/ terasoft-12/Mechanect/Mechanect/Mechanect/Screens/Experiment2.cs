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
    class Experiment2 : Mechanect.Common.GameScreen
    {
        Environment2 env;

        /// <summary>
        /// Defining the Textures that will contain the images and will represent the objects in the experiment
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Alzayat </para>   
        /// <para>DATE WRITTEN: April, 20 </para>
        /// <para>DATE MODIFIED: April, 22  </para>
        /// </remarks>

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

        SpriteFont spriteFont;
        SpriteFont velAngleFont;
        //GraphicsDevice graphicsDevice;

        private Texture2D backgroundTexture;
        private Texture2D xyAxisTexture;
        private Texture2D preyTexture;
        private Texture2D bowlTexture;
        private Texture2D grayTexture;
        private Texture2D velocityTexture;
        private Texture2D angleTexture;
        private Texture2D predatorTexture;
        Texture2D lineConnector;



        //Variables that will change how the Gui will look
        private Boolean preyEaten = false;
        private Boolean grayScreen = true;
        private float translationX;
        private float translationY;
        private float pixelsPerMeterX;
        private float pixelsPerMeterY;
        // defines the x-axis percentage up from the whole screen
        private float xAxisPercentage;
        // defines the y-axis percentage right from the whole screen
        private float yAxisPercentage;
        // defines the percentage of the screen to be drawn so that the notifications don't get corrupted with the actual experiment
        private float xDrawingPercentage;
        private float yDrawingPercentage;
        private int screenWidth;
        private int screenHeight;
        //To be used in sprint 2
        //private Vector2 velGauge;
        //private Vector2 angGauge;
        private Vector2 predatorOrigin;
        private Vector2 predatorPosition;
        private Vector2 preyPosition;
        private Vector2 startAquariumPosition;
        private Vector2 destinationAquariumPosition;

        float backgroundTextureScaling;
        float xyAxisTextureScaling;
        float preyTextureScaling;
        float bowlTextureScaling;
        float grayTextureScaling;
        float velocityTextureScaling;
        float angleTextureScaling;
        float predatorTextureScaling;
        //Only if 3D is used
        //float fishModelScaling;
        //Mohamed Alzayat Variables end
        VoiceCommands voiceCommand;
        User2 user;
        Boolean aquariumReached;
        MKinect mKinect;
        Button button;
        Vector2 buttonPosition;
        /// <summary>
        /// This is a constructor that will initialize the grphicsDeviceManager and define the Content directory.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Alzayat </para>   
        /// <para>DATE WRITTEN: April, 20 </para>
        /// <para>DATE MODIFIED: April, 20  </para>
        /// </remarks>
        public Experiment2(User2 user, MKinect mKinect)
        {

            env = new Environment2();
            //graphicsDevice = ScreenManager.GraphicsDevice;
            this.user = user;
            this.mKinect = mKinect;
        }



        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of the Content.
        /// Loaded the Fish Model
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Alzayat </para>   
        /// <para>DATE WRITTEN: April, 20 </para>
        /// <para>DATE MODIFIED: April, 22  </para>
        /// </remarks>

        public override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.

            screenWidth = ViewPort.Width;
            screenHeight = ViewPort.Height;


            LoadTextures(1);
            startAquariumPosition = MapPointsToScreen(env.Predator.Location);

            LoadObjectsWithValues(env.Predator.Location, env.Prey.Location, env.Aquarium.Location);

            spriteFont = Content.Load<SpriteFont>("Ariel");
            velAngleFont = Content.Load<SpriteFont>("angleVelFont");

            // zayat you can edit the button place as you wish
            buttonPosition = new Vector2(450, 10);
            button = new OKButton(Content, buttonPosition, screenWidth, screenHeight);
            //TBC
            voiceCommand = new VoiceCommands(mKinect._KinectDevice, "ok");

        }


        /// <summary>
        /// This Method will get the Values(Givens) of the Game Setup at the beginning
        /// Then it will be used to get the instantaneous values
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Alzayat </para>   
        /// <para>DATE WRITTEN: April, 22 </para>
        /// <para>DATE MODIFIED: April, 22  </para>
        /// </remarks>
        /// <param name="predator">Takes the vector2 position of the predator</param>
        /// <param name="prey">Takes the vector2 position of the prey</param>
        /// <param name="destination">Takes the vector2 position of the destination aquarium</param>
        private void LoadObjectsWithValues(Vector2 predator, Vector2 prey, Vector2 destination)
        {
            predatorPosition = MapPointsToScreen(predator);
            preyPosition = MapPointsToScreen(prey);
            destinationAquariumPosition = MapPointsToScreen(destination);


        }

        /// <summary>
        /// Allows the game to initialize all the textures 
        /// Initializing the Background and x,y Axises textures
        /// And add initial scaling 
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Alzayat </para>   
        /// <para>DATE WRITTEN: April, 20 </para>
        /// <para>DATE MODIFIED: April, 22  </para>
        /// </remarks>

        public void LoadTextures(int setNumber)
        {
            switch (setNumber)
            {
                case 1: ImageSet1(); break;
                default: ImageSet2(); break;
            }

            predatorOrigin = (new Vector2(predatorTexture.Width * predatorTextureScaling / 2, predatorTexture.Height * predatorTextureScaling / 2));

            configureScreen();


            base.Initialize();
        }

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
            //original width and height 
            //this is to check if scaling is correct or not
            screenWidth = Convert.ToInt32(backgroundTexture.Width * backgroundTextureScaling);
            screenHeight = Convert.ToInt32(backgroundTexture.Height * backgroundTextureScaling);

            if (backgroundTextureScaling > 1)
            {
                backgroundTextureScaling =1;
            }


            float maxDifferenceX = env.Aquarium.Location.X - env.Predator.Location.X;
            float maxDifferenceY = (env.Prey.Location.Y) - Math.Min(env.Aquarium.Location.Y, env.Predator.Location.Y);

            pixelsPerMeterX = (float)screenWidth * xDrawingPercentage / maxDifferenceX;
            pixelsPerMeterY = (float)screenHeight * yDrawingPercentage / maxDifferenceY;
            //to make the same scaling
            pixelsPerMeterX = Math.Min(pixelsPerMeterX,pixelsPerMeterY);
            pixelsPerMeterY = Math.Min(pixelsPerMeterX, pixelsPerMeterY);
            translationX = screenWidth * xAxisPercentage;
            translationY = screenHeight * yAxisPercentage;

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

            backgroundTextureScaling = 1;
            xyAxisTextureScaling = 1;
            preyTextureScaling = 0.1f;
            bowlTextureScaling = 0.2f;

            predatorTextureScaling = 0.2f;
            velocityTextureScaling = 0.7f;
            angleTextureScaling = 1.25f;

            xAxisPercentage = 0.04f;
            yAxisPercentage = 0.056f;

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
            // not more than 1
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
        /// UnloadContent will be called once per game and is the place to unload
        /// all Content.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Alzayat </para>   
        /// <para>DATE WRITTEN: April, 22 </para>
        /// <para>DATE MODIFIED: April, 22  </para>
        /// </remarks>
        Vector2 MapPointsToScreen(Vector2 position)
        {
            return new Vector2((position.X * pixelsPerMeterX + translationX), (screenHeight - (position.Y * pixelsPerMeterY + translationY)));
            // return new Vector2(150, 600);

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
           
            //TBA >> degrees

            SpriteBatch.Draw(bowlTexture, DrawAtRectangleMidPoint(bowlTexture, startAquariumPosition, bowlTextureScaling), null, Color.White, 0f, Vector2.Zero, bowlTextureScaling, SpriteEffects.None, 0f);
            SpriteBatch.Draw(predatorTexture, DrawAtRectangleMidPoint(predatorTexture, predatorPosition, predatorTextureScaling), null, Color.White, MathHelper.ToRadians(0), Vector2.Zero, predatorTextureScaling, SpriteEffects.None, 0f);

            SpriteBatch.Draw(bowlTexture, DrawAtRectangleMidPoint(bowlTexture, destinationAquariumPosition, bowlTextureScaling), null, Color.White, 0f, Vector2.Zero, bowlTextureScaling, SpriteEffects.None, 0f);
            string meters = "meters";
            if (backgroundTextureScaling <= 1)
            {
                SpriteBatch.DrawString(velAngleFont, meters, new Vector2(screenWidth * yAxisPercentage - 1f * spriteFont.MeasureString(meters).Y , 2 * spriteFont.MeasureString(meters).X * backgroundTextureScaling), Color.Red, MathHelper.ToRadians(-90), Vector2.Zero, 2 * backgroundTextureScaling, SpriteEffects.None, 0f);
                SpriteBatch.DrawString(velAngleFont, meters, new Vector2(screenWidth - spriteFont.MeasureString(meters).X / 1.75f, screenHeight - spriteFont.MeasureString(meters).Y / 1.5f), Color.Red, 0f, Vector2.Zero, 2 * backgroundTextureScaling, SpriteEffects.None, 0f);
            }
            else
            {
                SpriteBatch.DrawString(velAngleFont, meters, new Vector2(screenWidth * yAxisPercentage - 0.5f * spriteFont.MeasureString(meters).Y * backgroundTextureScaling, 2 * spriteFont.MeasureString(meters).X/2 ), Color.Red, MathHelper.ToRadians(-90), Vector2.Zero, 1f, SpriteEffects.None, 0f);
                SpriteBatch.DrawString(velAngleFont, meters, new Vector2(screenWidth - spriteFont.MeasureString(meters).X / 1.75f, screenHeight - spriteFont.MeasureString(meters).Y / 1.5f), Color.Red, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            
            }
            if (!preyEaten)
                SpriteBatch.Draw(preyTexture, DrawAtRectangleMidPoint(preyTexture, preyPosition, preyTextureScaling), null, Color.White, 0f, Vector2.Zero, preyTextureScaling, SpriteEffects.None, 0f);
            SpriteBatch.End();
        }
        
        /// <summary>
        /// This method will draw a gray Screen 
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
            if (backgroundTextureScaling <= 1)
            {
                SpriteBatch.DrawString(spriteFont, testString, new Vector2((screenWidth / 2), 0), Color.Red, 0f, new Vector2((screenWidth / 4), 0), backgroundTextureScaling, SpriteEffects.None, 0f);

                SpriteBatch.DrawString(spriteFont, sayString, new Vector2((screenWidth / 2), spriteFont.MeasureString(testString).Y /*+ spriteFont.MeasureString(sayString).Y*/), Color.Red, 0f, new Vector2((screenWidth / 4), 0), backgroundTextureScaling, SpriteEffects.None, 0f);
            }
            else
            {
                SpriteBatch.DrawString(spriteFont, testString, new Vector2((screenWidth / 2), 0), Color.Red, 0f, new Vector2((screenWidth / 4), 0), 1, SpriteEffects.None, 0f);

                SpriteBatch.DrawString(spriteFont, sayString, new Vector2((screenWidth / 2), spriteFont.MeasureString(testString).Y /*+ spriteFont.MeasureString(sayString).Y*/), Color.Red, 0f, new Vector2((screenWidth / 4), 0), 1, SpriteEffects.None, 0f);
            
            }
            SpriteBatch.End();
            // ask hegazy to make it scalable !
            //button.draw(SpriteBatch);
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
                //SpriteBatch.DrawString(velAngleFont, angString, new Vector2(screenWidth - ((screenWidth * 2 / 9) + angleTexture.Width * angleTextureScaling - spriteFont.MeasureString(angString).X), (screenHeight / 14) + velocityTexture.Height * velocityTextureScaling - spriteFont.MeasureString(velString).Y), Color.Red);
                //SpriteBatch.DrawString(velAngleFont, velString, new Vector2(screenWidth * xAxisPercentage + velocityTexture.Width * velocityTextureScaling - spriteFont.MeasureString(velString).X * velocityTextureScaling / 2, screenHeight * yAxisPercentage + velocityTexture.Height * velocityTextureScaling + spriteFont.MeasureString(velString).Y * velocityTextureScaling / 2), Color.Red, 0f, new Vector2(velocityTexture.Width * velocityTextureScaling / 2, velocityTexture.Height * velocityTextureScaling / 2), velocityTextureScaling * 2, SpriteEffects.None, 0f);
            }
            else
            {
                SpriteBatch.DrawString(velAngleFont, velString + env.Velocity, new Vector2(screenWidth - spriteFont.MeasureString(velString + angString).X / 2, 0), Color.Red);
                SpriteBatch.DrawString(velAngleFont, angString + env.Angle, new Vector2(screenWidth - spriteFont.MeasureString(angString).X / 2, 0), Color.Red);
            }
            SpriteBatch.End();
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
            if (backgroundTextureScaling <= 1)
            {
                
                for (int i = 0; i < objects.Length; i++)
                {
                    DrawLine(SpriteBatch, lineConnector, 2f, Color.LightGray, DrawAxisConnector(((Texture2D)objects[i][1]), ((Vector2)objects[i][2]), ((float)objects[i][3])), new Vector2(screenWidth * xAxisPercentage - spriteFont.MeasureString(((Vector2)objects[i][0]).X + "").Y / 2 * backgroundTextureScaling, DrawAxisConnector(((Texture2D)objects[i][1]), ((Vector2)objects[i][2]), ((float)objects[i][3])).Y));
                    SpriteBatch.DrawString(velAngleFont, ((Vector2)objects[i][0]).Y + "", DrawValueOnAxis(((Texture2D)objects[i][1]), ((Vector2)objects[i][2]), ((float)objects[i][3]), true), Color.Red, 0f, Vector2.Zero, velocityTextureScaling * 2, SpriteEffects.None, 0f);
                    DrawLine(SpriteBatch, lineConnector, 2f, Color.LightGray, DrawAxisConnector(((Texture2D)objects[i][1]), ((Vector2)objects[i][2]), ((float)objects[i][3])), new Vector2(DrawAxisConnector(((Texture2D)objects[i][1]), ((Vector2)objects[i][2]), ((float)objects[i][3])).X, screenHeight - spriteFont.MeasureString(((Vector2)objects[i][0]).X + "").Y / 2 * backgroundTextureScaling));
                    SpriteBatch.DrawString(velAngleFont, ((Vector2)objects[i][0]).X + "", DrawValueOnAxis(((Texture2D)objects[i][1]), ((Vector2)objects[i][2]), ((float)objects[i][3]), false), Color.Red, 0f, Vector2.Zero, velocityTextureScaling * 2, SpriteEffects.None, 0f);
                }
            }
            else
            {
                for (int i = 0; i < objects.Length; i++)
                {
                    DrawLine(SpriteBatch, lineConnector, 2f, Color.LightGray, DrawAxisConnector(((Texture2D)objects[i][1]), ((Vector2)objects[i][2]), ((float)objects[i][3])), new Vector2(screenWidth * yAxisPercentage - spriteFont.MeasureString(((Vector2)objects[i][0]).X + "").Y / 2 * backgroundTextureScaling, DrawAxisConnector(((Texture2D)objects[i][1]), ((Vector2)objects[i][2]), ((float)objects[i][3])).Y));
                    SpriteBatch.DrawString(velAngleFont, ((Vector2)objects[i][0]).Y + "", DrawValueOnAxis(((Texture2D)objects[i][1]), ((Vector2)objects[i][2]), ((float)objects[i][3]), true), Color.Red, 0f, Vector2.Zero, velocityTextureScaling * 2, SpriteEffects.None, 0f);
                    DrawLine(SpriteBatch, lineConnector, 2f, Color.LightGray, DrawAxisConnector(((Texture2D)objects[i][1]), ((Vector2)objects[i][2]), ((float)objects[i][3])), new Vector2(DrawAxisConnector(((Texture2D)objects[i][1]), ((Vector2)objects[i][2]), ((float)objects[i][3])).X, screenHeight - spriteFont.MeasureString(((Vector2)objects[i][0]).X + "").Y / 2 * backgroundTextureScaling));
                    SpriteBatch.DrawString(velAngleFont, ((Vector2)objects[i][0]).X + "", DrawValueOnAxis(((Texture2D)objects[i][1]), ((Vector2)objects[i][2]), ((float)objects[i][3]), false), Color.Red, 0f, Vector2.Zero, velocityTextureScaling * 2, SpriteEffects.None, 0f);
                }
            }
            //DrawLine(SpriteBatch, lineConnector, 2f, Color.LightGray, DrawAxisConnector(preyTexture, preyPosition, preyTextureScaling), new Vector2(30, DrawAxisConnector(preyTexture, preyPosition, preyTextureScaling).Y));
            //SpriteBatch.DrawString(velAngleFont, env.Prey.Location.Y + "", DrawValueOnAxis(preyTexture, preyPosition, preyTextureScaling, true), Color.Red);
            //DrawLine(SpriteBatch, lineConnector, 2f, Color.LightGray, DrawAxisConnector(preyTexture, preyPosition, preyTextureScaling), new Vector2(DrawAxisConnector(preyTexture, preyPosition, preyTextureScaling).X, screenHeight - 30));
            //SpriteBatch.DrawString(velAngleFont, env.Prey.Location.X + "", DrawValueOnAxis(preyTexture, preyPosition, preyTextureScaling, false), Color.Red);

            //DrawLine(SpriteBatch, lineConnector, 2f, Color.LightGray, DrawAxisConnector(bowlTexture, destinationAquariumPosition, bowlTextureScaling), new Vector2(30, DrawAxisConnector(bowlTexture, destinationAquariumPosition, bowlTextureScaling).Y));
            //SpriteBatch.DrawString(velAngleFont, env.Aquarium.Location.Y + "", DrawValueOnAxis(bowlTexture, destinationAquariumPosition, bowlTextureScaling, true), Color.Red);
            //DrawLine(SpriteBatch, lineConnector, 2f, Color.LightGray, DrawAxisConnector(bowlTexture, destinationAquariumPosition, bowlTextureScaling), new Vector2(DrawAxisConnector(bowlTexture, destinationAquariumPosition, bowlTextureScaling).X, screenHeight - 30));
            //SpriteBatch.DrawString(velAngleFont, env.Aquarium.Location.X + "", DrawValueOnAxis(bowlTexture, destinationAquariumPosition, bowlTextureScaling, false), Color.Red);

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
        /// 
        Vector2 DrawAxisConnector(Texture2D texture, Vector2 position, float scale)
        {
            position = DrawAtRectangleMidPoint(texture, position, scale);
            return new Vector2(position.X + texture.Width * scale / 2, position.Y + texture.Height * scale / 2);
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
                return new Vector2(2, position.Y + texture.Height * scale / 2 - spriteFont.MeasureString(position.Y + "").Y/4);
            else
                return new Vector2(position.X + texture.Width * scale / 2 - spriteFont.MeasureString(position.X + "").Y / 4, screenHeight - spriteFont.MeasureString(position.X + "").Y / 1.4f * backgroundTextureScaling);
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
