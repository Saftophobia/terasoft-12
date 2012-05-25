using System;
using System.Threading;
using System.Windows;
using ButtonsAndSliders;
using Mechanect.Common;
using Mechanect.Exp3;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Mechanect.Exp2
{

    /// <summary>
    /// This Class is responsible for running Experiment2 and displaying it's GUI
    /// </summary>
    /// <remarks>
    /// <para>AUTHOR: Mohamed Alzayat, Mohamed AbdelAzim </para>
    /// </remarks>
    public class Experiment2 : Mechanect.Common.GameScreen
    {
        VoiceCommands voiceCommand;
        User2 user;
        Boolean aquariumReached;
        MKinect mKinect;
        Button button;
        Vector2 buttonPosition;
        Boolean ended;
        int milliSeconds;
        Boolean isCopied;

        private Vector2 initialPredatorPosition;
        private Rect initialPreyPosition;
        private Rect initialAquariumPosition;

        /// <summary>
        /// Instance Variables
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Alzayat </para>   
        /// <para>DATE WRITTEN: April, 20 </para>
        /// <para>DATE MODIFIED: April, 24  </para>
        /// </remarks>
        // Fields that will be used as getters; to get some inherited objects
        #region: Fields
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
        #endregion
        #region: GUI instance Variables
        // An instance of the environment2 Class (acts as an engine for this class)
        private Environment2 environment;

        // Variable for fontSprite
        private SpriteFont velAngleFont;

        // Variables Contaiing the Textures Definintion 
        private Texture2D grayTexture;
        private Texture2D velocityTexture;
        private Texture2D angleTexture;
        private Texture2D backgroundTexture;

        // Variables That will be used as scaling for the Textures
        private Vector2 grayTextureScaling;
        private float velocityTextureScaling;
        private float angleTextureScaling;

        // A variable to specify the percentage left and write when drawing the velocity and angle gauges
        private float velocityAngleShift;

        // Variables defining the appearence of some objects or states
        private Boolean grayScreen;
        private Boolean preyEaten;
        #endregion

        /// <summary>
        /// This is a constructor that will initialize the grphicsDeviceManager and define the Content directory.
        /// in adition to initializing some instance variables
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Alzayat(sprint1), Tamer Nabil </para>   
        /// <para>DATE WRITTEN: April, 20 </para>
        /// <para>DATE MODIFIED: May, 24  </para>
        /// </remarks>
        /// <param name="user">Takes an instance of User2 </param>
        public Experiment2(User2 user)
        {
            user.Reset();
            environment = new Environment2();
            initialPredatorPosition = environment.Predator.Location;
            initialPreyPosition = new Rect(environment.Prey.Location.X, environment.Prey.Location.Y,
                environment.Prey.Width, environment.Prey.Length);
            initialAquariumPosition = new Rect(environment.Aquarium.Location.X,
                environment.Aquarium.Location.Y, environment.Aquarium.Width, environment.Aquarium.Length);
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
            user.Reset();
            environment = new Environment2(predatorPosition, preyPosition, AquariumPosition);
            initialPredatorPosition = environment.Predator.Location;
            initialPreyPosition = new Rect(environment.Prey.Location.X, environment.Prey.Location.Y,
                environment.Prey.Width, environment.Prey.Length);
            initialAquariumPosition = new Rect(environment.Aquarium.Location.X,
                environment.Aquarium.Location.Y, environment.Aquarium.Width, environment.Aquarium.Length);
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

            LoadTextures();

            grayTextureScaling = new Vector2((float)ViewPort.Width / grayTexture.Width, (float)ViewPort.Height /
                grayTexture.Height);
            // initially
            grayScreen = true;
            preyEaten = false;
            velocityTextureScaling = 0.4f;
            angleTextureScaling = 0.65f;
            velocityAngleShift = 0.05f;
            //Loading Font
            velAngleFont = Content.Load<SpriteFont>("ArielBig");

            //Loading the content of the environment
            environment.LoadContent(Content, ScreenManager.GraphicsDevice, ViewPort);

            //loading the button with the initial position
            buttonPosition = new Vector2(ViewPort.Width/2.5f, 2 * velAngleFont.MeasureString("A").Y);
            button = Tools3.OKButton(Content, buttonPosition, ViewPort.Width, ViewPort.Height, user);

            //The sound order needs to be done only once thus will be done in the load content
            UI.UILib.SayText("Test angle and Velocity using your left hand, then say GO or press ok");

            //TBC
            voiceCommand = new VoiceCommands(mKinect._KinectDevice, "go");
            var voiceThread = new Thread(voiceCommand.StartAudioStream);
            voiceThread.Start();
            base.LoadContent();
        }




        /// <summary>
        /// Allows the game to initialize all the textures 
        /// Loading  the gray screen , velocity and angle gauges textures.
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
            backgroundTexture = Content.Load<Texture2D>("Textures/Experiment2/ImageSet1/background");
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

            //Drawing the background since this is the original experiment

            //Drawing the Experiment environment
            SpriteBatch.Begin();
            SpriteBatch.Draw(backgroundTexture, new Rectangle(0, 0, ViewPort.Width, ViewPort.Height), Color.White);
            environment.Draw(new Rectangle(0, (int)(ViewPort.Height * 0.3f), ViewPort.Width, (int)
            ViewPort.Height), SpriteBatch);
            SpriteBatch.End();

            if (grayScreen)
            {
                DrawGrayScreen();
            }

            DrawAngVelLabels();

            if (ended && milliSeconds > 1000)
                Tools3.DisplayIsWin(ScreenManager.SpriteBatch, Content, new Vector2(0.18f * ViewPort.Width,
                    0.35f * ViewPort.Height), environment.Win);

            base.Draw(gameTime);
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

            SpriteBatch.Draw(velocityTexture, new Vector2(ViewPort.Width * velocityAngleShift,
                ViewPort.Height * 0.05f), null, Color.White, 0f, Vector2.Zero,
                velocityTextureScaling, SpriteEffects.None, 0f);

            SpriteBatch.Draw(angleTexture, new Vector2(ViewPort.Width - ViewPort.Width * velocityAngleShift -
                angleTexture.Width * angleTextureScaling - ViewPort.Width*0.05f, ViewPort.Height * 0.05f), null,
                Color.White, 0f, Vector2.Zero, angleTextureScaling, SpriteEffects.None, 0f);

            string testString = "Test angle and Velocity";
            string sayString = "Say 'GO' or press OK";

            SpriteBatch.DrawString(velAngleFont, testString, new Vector2((ViewPort.Width / 2.3f), 0), Color.Red, 0f,
                new Vector2((ViewPort.Width / 4), 0), 0.7f, SpriteEffects.None, 0f);

            SpriteBatch.DrawString(velAngleFont, sayString, new Vector2((ViewPort.Width / 2.3f),
                velAngleFont.MeasureString(testString).Y), Color.Red, 0f,
                new Vector2((ViewPort.Width / 4), 0), 0.7f, SpriteEffects.None, 0f);

            //Draw The button and hand
            button.Draw(SpriteBatch, 0.45f);
            button.DrawHand(SpriteBatch);

            SpriteBatch.End();


        }

        /// <summary>
        /// This Method is to be called to draw the angle and velocity Labels.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Alzayat </para>   
        /// <para>DATE WRITTEN: April, 22 </para>
        /// <para>DATE MODIFIED: May, 22  </para>
        /// </remarks>
        private void DrawAngVelLabels()
        {
            string velString = "Velocity = " + Math.Round(user.MeasuredVelocity, 1);
            string angString = "Angle = " + Math.Round(user.MeasuredAngle, 1);

            SpriteBatch.Begin();

            if (grayScreen)
            {
                SpriteBatch.DrawString(velAngleFont, velString, new Vector2(ViewPort.Width * velocityAngleShift +
                    velocityTexture.Width * velocityTextureScaling - velAngleFont.MeasureString(velString).X *
                    velocityTextureScaling, ViewPort.Height * velocityAngleShift + velocityTexture.Height *
                    velocityTextureScaling - (velAngleFont.MeasureString(velString).Y * velocityTextureScaling) / 2),
                    Color.Red, 0f, new Vector2(velocityTexture.Width * velocityTextureScaling / 2,
                        velocityTexture.Height * velocityTextureScaling / 2), velocityTextureScaling,
                        SpriteEffects.None, 0f);


                SpriteBatch.DrawString(velAngleFont, angString, new Vector2(ViewPort.Width - (ViewPort.Width *
                    velocityAngleShift + angleTexture.Width * angleTextureScaling + ViewPort.Width * 0.054f  - velAngleFont.MeasureString
                    (angString).X * velocityTextureScaling / 2), ViewPort.Height * velocityAngleShift +
                    angleTexture.Height * angleTextureScaling -
                    velAngleFont.MeasureString(angString).Y * velocityTextureScaling / 4),
                    Color.Red, 0f, new Vector2(angleTexture.Width * angleTextureScaling / 2, angleTexture.Height *
                        angleTextureScaling / 2), velocityTextureScaling, SpriteEffects.None, 0f);
            }
            else
            {

                SpriteBatch.DrawString(velAngleFont, velString, new Vector2
                    (ViewPort.Width - velAngleFont.MeasureString(velString + angString).X / 2, 0), Color.Red, 0f,
                    new Vector2(velAngleFont.MeasureString(velString +
                        environment.Velocity + "                    ").X / 2, 0), velocityTextureScaling,
                        SpriteEffects.None, 0f);

                SpriteBatch.DrawString(velAngleFont, angString,
                    new Vector2(ViewPort.Width - velAngleFont.MeasureString(angString).X / 2, 0), Color.Red, 0f,
                    new Vector2(velAngleFont.MeasureString(velString + environment.Velocity).X / 2, 0),
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
        public override void Update(GameTime gameTime)
        {
            if (ended)
            {
                milliSeconds += gameTime.ElapsedGameTime.Milliseconds;
                if (milliSeconds > 3000)
                {
                    this.Remove();
                    if (environment.Win)
                        ScreenManager.AddScreen(new StatisticsScreen(initialPredatorPosition, 
                            initialPreyPosition, initialAquariumPosition, user.MeasuredVelocity,
                            user.MeasuredAngle, user));
                    else
                        ScreenManager.AddScreen(new StatisticsScreen(initialPredatorPosition,
                            initialPreyPosition, initialAquariumPosition, user.MeasuredVelocity,
                            user.MeasuredAngle, (float)environment.Velocity, (float)environment.Angle, user));
                    
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
                    if (button.IsClicked() || voiceCommand.GetHeard("go"))
                    {
                        grayScreen = false;
                        button = null;
                        voiceCommand = null;
                        user.Reset();
                    }
                }
                    user.MeasureVelocityAndAngle(gameTime);
            }
            base.Update(gameTime);
        }
    }

}



