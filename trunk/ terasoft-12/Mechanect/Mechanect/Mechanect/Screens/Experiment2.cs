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
using UI.Cameras;
using Mechanect.Common;
using Mechanect.Exp3;
using ButtonsAndSliders;
using Mechanect.Exp2;

namespace Mechanect.Screens
{

    /// <summary>
    /// This Class is responsible for running Experiment2 and displaying it's GUI
    /// </summary>
    /// <remarks>
    /// <para>AUTHOR: Mohamed Alzayat, Mohamed Abdelazim </para>
    /// </remarks>
    public class Experiment2 : Mechanect.Common.GameScreen
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
        private Texture2D grayTexture;
        private Texture2D velocityTexture;
        private Texture2D angleTexture;

        // Variables That will be used as scaling for the Textures

        private Vector2 grayTextureScaling;
        private float velocityTextureScaling;
        private float angleTextureScaling;

        // A variable to specify the percentage left and write when drawing the velocity and angle gauges
        private float velocityAngleShift;

        // Variables that will change how the Gui will look

        // Variables defining the appearence of some objects
        private Boolean grayScreen;
        private Boolean preyEaten;


        // Variables defining the screen Width and Height that will be used in drawing the objects of the experiment
        private int screenWidth;
        private int screenHeight;


        /// <summary>
        /// This is a constructor that will initialize the grphicsDeviceManager and define the Content directory.
        /// in adition to initializing some instance variables
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Alzayat, Tamer Nabil </para>   
        /// <para>DATE WRITTEN: April, 20 </para>
        /// <para>DATE MODIFIED: April, 20  </para>
        /// </remarks>
        /// <param name="user">Takes an instance of User2 </param>
        /// <param name="mKinect">takes an instance of mKinect</param>
        public Experiment2(User2 user)
        {

            environment = new Environment2();

            this.user = user;
            this.mKinect = user.Kinect;
            isCopied = false;


        }

        /// <summary>
        /// A constructor that specifies a special setup to the game
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed AbdelAzim </para>
        /// </remarks>
        /// <param name="user">Takes an instance of User2 </param>
        /// <param name="predatorPosition">Vector2, the center point of the predator</param>
        /// <param name="preyPosition">Rect, the rectangle that represents the position of the prey</param>
        /// <param name="aquariumPosition">Rect, the rectangle that represents the position of the aquarium</param>
        public Experiment2(User2 user, Vector2 predatorPosition, Rect preyPosition, Rect AquariumPosition)
        {

            environment = new Environment2(predatorPosition, preyPosition, AquariumPosition);

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
        /// <para>DATE MODIFIED: May, 22  </para>
        /// </remarks>

        public override void LoadContent()
        {
            // initializing the screen width and height (just initial values and will be changed later)
            screenWidth = ViewPort.Width;
            screenHeight = ViewPort.Height;
            LoadTextures();

            grayTextureScaling = new Vector2((float)ViewPort.Width / grayTexture.Width, (float)ViewPort.Height /
                grayTexture.Height);
            grayScreen = true;
            preyEaten = false;
            velocityTextureScaling = 0.4f;
            angleTextureScaling = 0.65f;
            velocityAngleShift = 0.05f;
            //Loading Fonts
            velAngleFont = Content.Load<SpriteFont>("Ariel");
            spriteFont = Content.Load<SpriteFont>("ArielBig");

            //creating a test environment
            environment = new Environment2(Vector2.Zero, new Rect(5, 10, 0.8, 0.8), new Rect(10, 3, 2, 2));
            environment.LoadContent(Content, ScreenManager.GraphicsDevice, ViewPort);

            buttonPosition = new Vector2(screenWidth - screenWidth / 2.7f, 0);
            button = Tools3.OKButton(Content, buttonPosition, screenWidth, screenHeight, user);
            //TBC
            // voiceCommand = new VoiceCommands(mKinect._KinectDevice, "go");

        }




        /// <summary>
        /// Allows the game to initialize all the textures 
        /// Initializing the Background and x,y Axises textures
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Alzayat </para>   
        /// <para>DATE WRITTEN: April, 20 </para>
        /// <para>DATE MODIFIED: May, 21 </para>
        /// </remarks>
        public void LoadTextures()
        {
            grayTexture = Content.Load<Texture2D>("Textures/Experiment2/ImageSet1/GrayScreen");
            velocityTexture = Content.Load<Texture2D>("Textures/Experiment2/ImageSet1/VelocityGauge");
            angleTexture = Content.Load<Texture2D>("Textures/Experiment2/ImageSet1/AngleGauge");
        }


        /// <summary>
        /// This is to be called when the game should draw itself.
        /// Here all the GUI is drawn
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Alzayat </para>   
        /// <para>DATE WRITTEN: April, 20 </para>
        /// <para>DATE MODIFIED: May, 22  </para>
        /// </remarks>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>

        public override void Draw(GameTime gameTime)
        {

            ScreenManager.GraphicsDevice.Clear(Color.CornflowerBlue);
            environment.DrawBackground = true;
            //Drawing the test environment
            SpriteBatch.Begin();
            environment.Draw(new Rectangle(40, (int)(screenHeight * 0.4f), (int)(screenWidth * 0.6f), (int)
            (screenHeight * 0.6f)), SpriteBatch);
            //another test
            //environment.Draw(new Rectangle(0, 0, ViewPort.Width, ViewPort.Height), SpriteBatch);
            SpriteBatch.End();
            if (grayScreen)
            {
                DrawGrayScreen();
            }

            DrawAngVelLabels();

            if (ended && milliSeconds > 1000)
                Tools3.DisplayIsWin(ScreenManager.SpriteBatch, Content, new Vector2(ViewPort.Width / 2,
                    ViewPort.Height / 2), environment.Win);


        }

        /// <summary>
        /// This method will draw a gray Screen with all it's components
        /// That is the screen to be displayed when the user will be testing the velocity and angle
        /// </summary>
        ///  <remarks>
        /// <para>AUTHOR: Mohamed Alzayat </para>   
        /// <para>DATE WRITTEN: April, 21 </para>
        /// <para>DATE MODIFIED: May, 22  </para>
        /// </remarks>
        private void DrawGrayScreen()
        {
            SpriteBatch.Begin();

            SpriteBatch.Draw(grayTexture, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, grayTextureScaling,
                SpriteEffects.None, 0f);

            SpriteBatch.Draw(velocityTexture, new Vector2(screenWidth * velocityAngleShift, screenHeight * 0.05f),
                null, Color.White, 0f, Vector2.Zero, velocityTextureScaling, SpriteEffects.None, 0f);

            SpriteBatch.Draw(angleTexture, new Vector2(screenWidth - screenWidth * velocityAngleShift -
                angleTexture.Width * angleTextureScaling, screenHeight * 0.05f), null, Color.White, 0f, Vector2.Zero,
                angleTextureScaling, SpriteEffects.None, 0f);

            string testString = "Test angle and Velocity";
            string sayString = "Say 'GO' or press OK";

            SpriteBatch.DrawString(spriteFont, testString, new Vector2((screenWidth / 2), 0), Color.Red, 0f,
                new Vector2((screenWidth / 4), 0), 0.7f, SpriteEffects.None, 0f);

            SpriteBatch.DrawString(spriteFont, sayString, new Vector2((screenWidth / 2), spriteFont.MeasureString
                (testString).Y), Color.Red, 0f, new Vector2((screenWidth / 4), 0), 0.7f, SpriteEffects.None, 0f);

            SpriteBatch.End();

            //TBC
            //button.Draw(SpriteBatch);
        }

        /// <summary>
        /// This Method is to be called whenDrawing The angle and velocity Labels.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Alzayat </para>   
        /// <para>DATE WRITTEN: April, 22 </para>
        /// <para>DATE MODIFIED: May, 22  </para>
        /// </remarks>
        private void DrawAngVelLabels()
        {
            String velString = "Velocity = " + Math.Round(user.MeasuredVelocity, 2);
            //I commented this line to have a compilation-error free repo
            String angString = "Angle = " + Math.Round(user.MeasuredAngle, 2);

            SpriteBatch.Begin();

            if (grayScreen)
            {
                SpriteBatch.DrawString(spriteFont, velString, new Vector2(screenWidth * velocityAngleShift +
                    velocityTexture.Width * velocityTextureScaling - spriteFont.MeasureString(velString).X *
                    velocityTextureScaling, screenHeight * velocityAngleShift + velocityTexture.Height *
                    velocityTextureScaling - (spriteFont.MeasureString(velString).Y * velocityTextureScaling) / 2),
                    Color.Red, 0f, new Vector2(velocityTexture.Width * velocityTextureScaling / 2,
                        velocityTexture.Height * velocityTextureScaling / 2), velocityTextureScaling,
                        SpriteEffects.None, 0f);


                SpriteBatch.DrawString(spriteFont, angString, new Vector2(screenWidth - (screenWidth *
                    velocityAngleShift + angleTexture.Width * angleTextureScaling - spriteFont.MeasureString
                    (angString).X * velocityTextureScaling / 2), screenHeight * velocityAngleShift + angleTexture.Height
                    * angleTextureScaling - spriteFont.MeasureString(angString).Y * velocityTextureScaling / 4),
                    Color.Red, 0f, new Vector2(angleTexture.Width * angleTextureScaling / 2, angleTexture.Height *
                        angleTextureScaling / 2), velocityTextureScaling, SpriteEffects.None, 0f);
            }
            else
            {

                SpriteBatch.DrawString(spriteFont, velString + Math.Round(environment.Velocity, 2), new Vector2
                    (screenWidth - spriteFont.MeasureString(velString + angString).X / 2, 0), Color.Red, 0f,
                    new Vector2(spriteFont.MeasureString(velString +
                        environment.Velocity + "                    ").X / 2, 0), velocityTextureScaling,
                        SpriteEffects.None, 0f);

                SpriteBatch.DrawString(spriteFont, angString + Math.Round(environment.Angle, 2),
                    new Vector2(screenWidth - spriteFont.MeasureString(angString).X / 2, 0), Color.Red, 0f,
                    new Vector2(spriteFont.MeasureString(velString + environment.Velocity).X / 2, 0),
                    velocityTextureScaling, SpriteEffects.None, 0f);
            }
            SpriteBatch.End();
        }

        /// <summary>
        /// Runs at every frame, Updates game parameters and checks for user's actions
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed AbdelAzim </para>
        /// </remarks>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// <param name="covered">determines whether there exist another screen covering this one or not.</param>
        public override void Update(GameTime gameTime)
        {
            if (ended)
            {
                milliSeconds += gameTime.ElapsedGameTime.Milliseconds;
                if (milliSeconds > 3000)
                {
                    this.Remove();
                    //ScreenManager.AddScreen(new StatisticScreen2());
                }
            }
            else if (!grayScreen && user.MeasuredVelocity != 0)
            {
                if (!isCopied)
                {
                    isCopied = true;
                    environment.Predator.Velocity = new Vector2(
                        (float)(user.MeasuredVelocity * Math.Cos(user.MeasuredAngle * Math.PI / 180)),
                        (float)(user.MeasuredVelocity * Math.Sin(user.MeasuredAngle * Math.PI / 180)));
                }
                ended = !environment.Update(gameTime);
                preyEaten = environment.Prey.Eaten;
                aquariumReached = !environment.Predator.Movable && environment.Predator.Location.Y > 0;
            }
            else
            {
                user.setSkeleton();
                if (button != null)
                {
                    button.Update(gameTime);
                    if (button.IsClicked())// || voiceCommand.GetHeared("go"))
                    {
                        grayScreen = false;
                        button = null;
                        voiceCommand = null;
                        user.Reset();
                    }
                }
                else
                    user.MeasureAngleAndVelocity(gameTime);
            }
        }
    }

}

    

