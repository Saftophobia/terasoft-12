using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Kinect;
using Microsoft.Speech;
using Microsoft.Speech.AudioFormat;
using Microsoft.Speech.Recognition;
using System.IO;








namespace Healthbar
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    /// 
    public class Game1 : Microsoft.Xna.Framework.Game
    {

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteBatch mBatch;
        KinectAudioSource kinectSource;
        SpeechRecognitionEngine sre;
        Stream stream;
        string RecognizerId = "SR_MS_en-US_Kinect_10.0";
        bool speechNotRecognized;
        Texture2D mHealthBar;
        int mCurrentHealth = 100;
        KinectSensor nui;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            mBatch = new SpriteBatch(this.graphics.GraphicsDevice);

            

            //Load the HealthBar image from the disk into the Texture2D object
            mHealthBar = Content.Load<Texture2D>("HealthBar");

            // TODO: use this.Content to load your game content here
            InitalizeKinect();
        }
        private void InitalizeKinect()
        {

           nui = KinectSensor.KinectSensors[0];

            nui.Start();

            kinectSource = nui.AudioSource;



            RecognizerInfo ri = GetKinectRecognizer();

           sre = new SpeechRecognitionEngine(ri.Id);

            var choices = new Choices();
            choices.Add("up");
            choices.Add("down");
            
            GrammarBuilder gb = new GrammarBuilder();
            gb.Culture = ri.Culture;
            gb.Append(choices);

            var g = new Grammar(gb);

            sre.LoadGrammar(g);
            sre.SpeechRecognized += SreSpeechRecognized;

            Console.WriteLine("Recognizing Speech");

            stream = kinectSource.Start();

            sre.SetInputToAudioStream(stream,
                          new SpeechAudioFormatInfo(
                              EncodingFormat.Pcm, 16000, 16, 1,
                              32000, 2, null));

            sre.RecognizeAsync(RecognizeMode.Multiple);

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the default game to exit on Xbox 360 and Windows

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)

                this.Exit();
           

            //Get the current keyboard state (which keys are currently pressed and released)



            KeyboardState mKeys = Keyboard.GetState();


          
            //If the Up Arrow is pressed, increase the Health bar

            if (mKeys.IsKeyDown(Keys.Up) == true)
            {

                mCurrentHealth += 10;

            }



            //If the Down Arrowis pressed, decrease the Health bar

            if (mKeys.IsKeyDown(Keys.Down) == true)
            {

                mCurrentHealth -= 10;

            }



            //Force the health to remain between 0 and 100

            mCurrentHealth = (int)MathHelper.Clamp(mCurrentHealth, 0, 100);



            base.Update(gameTime);
        }
        private  void SreSpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if(e.Result.Confidence>0.1){
            if (e.Result.Text.Equals("up"))
            {
                mCurrentHealth += 10;
            }
            else if (e.Result.Text.Equals("down"))
            {
                Console.WriteLine(e.Result.Text);
                mCurrentHealth -= 10;
            }
        }
           
        }
        private static RecognizerInfo GetKinectRecognizer()
        {
            Func<RecognizerInfo, bool> matchingFunc = r =>
            {
                string value;
                r.AdditionalInfo.TryGetValue("Kinect", out value);
                return "True".Equals(value, StringComparison.InvariantCultureIgnoreCase) && "en-US".Equals(r.Culture.Name, StringComparison.InvariantCultureIgnoreCase);
            };
            return SpeechRecognitionEngine.InstalledRecognizers().Where(matchingFunc).FirstOrDefault();
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
                 

            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

 

            //TODO: Add your drawing code here

            mBatch.Begin();

 

            //Draw the negative space for the health bar

mBatch.Draw(mHealthBar, new Rectangle(this.Window.ClientBounds.Width / 2 - mHealthBar.Width / 2,

30, mHealthBar.Width, 44), new Rectangle(0, 45, mHealthBar.Width, 44), Color.Gray);

 
            
            //Draw the current health level based on the current Health

            mBatch.Draw(mHealthBar, new Rectangle(this.Window.ClientBounds.Width / 2 - mHealthBar.Width / 2,

                 30, (int)(mHealthBar.Width * ((double)mCurrentHealth / 100)), 44), 

                 new Rectangle(0, 45, mHealthBar.Width, 44), Color.Red);

 
            //Draw the box around the health bar

              mBatch.Draw(mHealthBar, new Rectangle(this.Window.ClientBounds.Width / 2 - mHealthBar.Width / 2,

                30, mHealthBar.Width, 44), new Rectangle(0, 0, mHealthBar.Width, 44), Color.White);

            

            mBatch.End();

 

            base.Draw(gameTime);

        }

        }
    }

