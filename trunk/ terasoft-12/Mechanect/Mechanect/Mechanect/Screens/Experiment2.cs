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
        /// <para>DATE MODIFIED: April, 21  </para>
        /// </remarks>

        Viewport viewPort;
        ContentManager content;
        SpriteBatch spriteBatch;

        SpriteFont spriteFont;
        SpriteFont velAngleFont;
        GraphicsDevice graphicsDevice;

        private Texture2D backgroundTexture;
        private Texture2D xyAxisTexture;
        private Texture2D preyTexture;
        private Texture2D bowlTexture;
        private Texture2D grayTexture;
        private Texture2D velocityTexture;
        private Texture2D angleTexture;
        Texture2D lineConnector;

        //list of models to be drawn
        private List<CustomModel> models = new List<CustomModel>();
        private Camera camera;

        //Variables that will change how the Gui will look
        private Boolean preyEaten = false;
        private Boolean grayScreen = true;
        
        private int screenWidth;
        private int screenHeight;
        private Vector2 velGauge;
        private Vector2 angGauge;
        private Vector3 predetorPosition;
        private Vector2 preyPosition;
        private Vector2 startAquariumPosition;
        private Vector2 destinationAquariumPosition;


        VoiceCommands voiceCommand;
        User2 user;
        Boolean aquariumReached;
        /// <summary>
        /// This is a constructor that will initialize the grphicsDeviceManager and define the content directory.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Alzayat </para>   
        /// <para>DATE WRITTEN: April, 20 </para>
        /// <para>DATE MODIFIED: April, 20  </para>
        /// </remarks>
        public Experiment2(User2 user)
        {

            env = new Environment2();
            graphicsDevice = ScreenManager.GraphicsDevice;
            this.user = user;
            

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        ///
        /// Initializing the Background and x,y Axises
        /// Changed the default resolution to 1024X720
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Alzayat </para>   
        /// <para>DATE WRITTEN: April, 20 </para>
        /// <para>DATE MODIFIED: April, 20  </para>
        /// </remarks>

        public void loadTextures()
        {
            
            backgroundTexture = content.Load<Texture2D>("Textures/background");
            xyAxisTexture = content.Load<Texture2D>("Textures/xyAxis");
            preyTexture = content.Load<Texture2D>("Textures/worm");
            bowlTexture = content.Load<Texture2D>("Textures/bowl2");
            grayTexture = content.Load<Texture2D>("Textures/screen");
            velocityTexture = content.Load<Texture2D>("Textures/VelocityGauge");
            angleTexture = content.Load<Texture2D>("Textures/AngleGauge");
            lineConnector = new Texture2D(graphicsDevice, 1, 1, false, SurfaceFormat.Color);
            lineConnector.SetData(new[] { Color.Gray });
            
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// Loaded the Fish Model
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Alzayat </para>   
        /// <para>DATE WRITTEN: April, 20 </para>
        /// <para>DATE MODIFIED: April, 21  </para>
        /// </remarks>

        public override void LoadContent()
        {
           // Create a new SpriteBatch, which can be used to draw textures.
            viewPort = ScreenManager.GraphicsDevice.Viewport;
            content = ScreenManager.Game.Content;
            spriteBatch = ScreenManager.SpriteBatch;
            //add model to array of models
            //models constructor takes the actual model, position vector, rotation vector(x, y, z rotation angels), scaling vector(x, y, z scales) and GraphicsDevice         
            models.Add(new CustomModel(content.Load<Model>("Models/fish"), new Vector3(-500, -500, -1050), new Vector3(MathHelper.ToRadians(-35), MathHelper.ToRadians(0), 0), new Vector3(0.007f), graphicsDevice));
           // predetorPosition = vect

            //create still camera
            camera = new TargetCamera(new Vector3(-3000, 100, 0), new Vector3(100, 100, 0), graphicsDevice);
            //cameras constructor takes position vector and target vector(the point where the camera is looking) 

            spriteFont = content.Load<SpriteFont>("Ariel");
            velAngleFont = content.Load<SpriteFont>("angleVelFont");

        }
        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Alzayat </para>   
        /// <para>DATE WRITTEN: April, 20 </para>
        /// <para>DATE MODIFIED: April, 20  </para>
        /// </remarks>
        public override void UnloadContent()
        {
           
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
                && position.Y >= aquarium.Location.Y - aquarium.Length  / 2
                && position.Y <= aquarium.Location.Y + aquarium.Length  / 2)
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
        {
            
            camera.Update();

            if (!grayScreen && user.MeasuredVelocity != 0 && !aquariumReached)
            {
                if (env.Predator.Velocity == null)
                    env.Predator.Velocity = new Vector2((float)(user.MeasuredVelocity * Math.Cos(user.MeasuredAngle)), (float)(user.MeasuredVelocity * Math.Sin(user.MeasuredAngle)));
                env.Predator.UpdatePosition(gameTime);
                if (!preyEaten) preyEaten = isPreyEaten();
                if (!aquariumReached) aquariumReached = isAquariumReached();
            }
            
            else
            {
                user.MeasureVelocityAndAngle();
            }

            base.Update(gameTime, covered);
        }
        /// <summary>
        /// This is to be called when the game should draw itself.
        /// Here all the GUI is drawn
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Alzayat </para>   
        /// <para>DATE WRITTEN: April, 20 </para>
        /// <para>DATE MODIFIED: April, 20  </para>
        /// </remarks>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            graphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            spriteBatch.Draw(backgroundTexture, Vector2.Zero, Color.White);
            spriteBatch.Draw(xyAxisTexture, Vector2.Zero, Color.White);
            spriteBatch.Draw(bowlTexture, new Vector2(40f, 430f), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(velAngleFont, "meters", new Vector2(0f, 0f), Color.Red);
            spriteBatch.DrawString(velAngleFont, "meters", new Vector2(900f, 680f), Color.Red);

            if (!preyEaten)
                spriteBatch.Draw(preyTexture, new Vector2(500f, 200f), null, Color.White, 0f, Vector2.Zero, 0.1f, SpriteEffects.None, 0f);
            if (grayScreen)
            {
                spriteBatch.Draw(grayTexture, Vector2.Zero, Color.White);
                spriteBatch.Draw(velocityTexture, new Vector2(55f, 50f), null, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f);
                spriteBatch.Draw(angleTexture, new Vector2(812f, 50f), null, Color.White, 0f, Vector2.Zero, 0.85f, SpriteEffects.None, 0f);

                spriteBatch.DrawString(spriteFont, "Test angle and Velocity", new Vector2((screenWidth / 4), 0), Color.Red);

                spriteBatch.DrawString(spriteFont, "Say 'GO' or press OK", new Vector2((screenWidth / 4), 600), Color.Red);

                spriteBatch.DrawString(velAngleFont, "Velocity = " + env.Velocity, new Vector2(110f, 185f), Color.Red);

                spriteBatch.DrawString(velAngleFont, "Angle = " + env.Angle, new Vector2(820f, 185f), Color.Red);

            }
            else
            {
                spriteBatch.DrawString(velAngleFont, "Velocity = " + env.Velocity, new Vector2(870f, 30f), Color.Red);

                spriteBatch.DrawString(velAngleFont, "Angle = " + env.Angle, new Vector2(780f, 30f), Color.Red);

            }
            spriteBatch.Draw(bowlTexture, new Vector2(750f, 430f), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            //DrawLine(spriteBatch, lineConnector, 2f, Color.LightGray, new Vector2(500f, 200f), new Vector2(0, 200f));
            //DrawLine(spriteBatch, lineConnector, 2f, Color.LightGray, new Vector2(500f, 200f), new Vector2(500f, screenHeight));
            //DrawLine(spriteBatch, lineConnector, 2f, Color.LightGray, new Vector2(80f, 450f), new Vector2(0, 450f));
            //DrawLine(spriteBatch, lineConnector, 2f, Color.LightGray, new Vector2(80f, 450f), new Vector2(80f, screenHeight));
            //DrawLine(spriteBatch, lineConnector, 2f, Color.Gray, new Vector2(900f, 680f), new Vector2(0, 200f));
            //DrawLine(spriteBatch, lineConnector, 2f, Color.Gray, new Vector2(900f, 680f), new Vector2(500f, screenHeight));

            spriteBatch.End();



            foreach (CustomModel model in models)
            {
                model.Draw(camera);
                //takes the camera instance and draws the model 
            }

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
        /// <param name="batch">Takes the spriteBatch that will draw the line </param>
        /// <param name="lineTexture">Takes the texture of the line to be drawn</param>
        /// <param name="width">Determines the width of the line to be drawn</param>
        /// <param name="color">Determines the color to be drawn</param>
        /// <param name="point1">Determines the start point of the line to be drawn</param>
        /// <param name="point2">Determines the end point of the line to be drawn</param>
        void DrawLine(SpriteBatch spriteBatch, Texture2D lineTexture,
              float width, Color color, Vector2 point1, Vector2 point2)
        {
            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            float length = Vector2.Distance(point1, point2);

            spriteBatch.Draw(lineTexture, point1, null, color,
                       angle, Vector2.Zero, new Vector2(length, width),
                       SpriteEffects.None, 0);
        }


        public override void Remove()
        {
            base.Remove();
        }
    }
}
