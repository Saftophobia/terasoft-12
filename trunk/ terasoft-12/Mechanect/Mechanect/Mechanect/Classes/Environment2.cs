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

namespace Mechanect
{
    public class Environment2 : Microsoft.Xna.Framework.Game
    {


        Prey prey;

        Predator predator;

        Aquarium aquarium;
        Random rand = new Random();

        int tolerance = 10;
        int velocity;

        double angleInDegree;
        double angle;
        double TotalTime;

        /// <summary>
        /// Defining the Textures that will contain the images and will represent the objects in the experiment
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Alzayat </para>   
        /// <para>DATE WRITTEN: April, 20 </para>
        /// <para>DATE MODIFIED: April, 20  </para>
        /// </remarks>
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private Texture2D backgroundTexture;
        private Texture2D xyAxisTexture;
        private Texture2D preyTexture;
        private Texture2D bowlTexture;
        //list of models to be drawn
        private List<CustomModel> models = new List<CustomModel>();
        private Camera camera;
        //Variables that will change how the Gui will look
        private Boolean preyEaten = false;
        private Boolean grayScreen = false;


        /// <summary>
        /// This is a constructor that will initialize the grphicsDeviceManager and define the content directory.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Alzayat </para>   
        /// <para>DATE WRITTEN: April, 20 </para>
        /// <para>DATE MODIFIED: April, 20  </para>
        /// </remarks>
        public Environment2()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }


        /// <summary>
        /// generates random angle between 10 and 90
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Tamer Nabil </para>
        /// </remarks>
        /// <returns></returns>
        private double getRandomAngle()
        {
            return rand.Next(10, 90);
        }
        /// <summary>
        /// generates random velocity between 10 and 90
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Tamer Nabil </para>
        /// </remarks>
        /// <returns></returns>
        private int getRandomVelocity()
        {
            return rand.Next(10, 70);
        }

        /// <summary>
        /// getSolve : Generate solvable enviroment by setting solvable points for predator,Prey and Aquarium 
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Tamer Nabil </para>
        /// </remarks>
        public void getSolvablePoints()
        {
            double xPredator;
            double yPredator;

            double xPrey;
            double yPrey;

            double xAquarium;
            double yAquarium;

            xPredator = 0;
            yPredator = rand.Next(0, 70);
            angleInDegree = getRandomAngle();
            angle = angleInDegree * (Math.PI / 180);
            velocity = getRandomVelocity();



            double b = velocity * Math.Sin(angle);

            double a = 0.5 * -9.8;

            double c = yPredator;

            double Timeneeded = (-b + Math.Sqrt(Math.Pow(b, 2) - (4 * a * c))) / (2 * a);

            double Timeneeded2 = (-b - Math.Sqrt(Math.Pow(b, 2) - (4 * a * c))) / (2 * a);

            if (Timeneeded > 0)
            {
                TotalTime = Timeneeded;
            }
            else
            {
                TotalTime = Timeneeded2;
            }


            Double TimeSlice = TotalTime / 3;

            int TimeSlice2 = (int)(TimeSlice * 10);

            int randomTimeForPrey = rand.Next(TimeSlice2, TimeSlice2 * 2);

            int randomTimeforAquarium = rand.Next(randomTimeForPrey + (randomTimeForPrey * 10 / 100), TimeSlice2 * 3);


            Double TimePrey = (Double)randomTimeForPrey / 10;

            Double TimeAquarium = (Double)randomTimeforAquarium / 10;

            xPrey = getX(TimePrey);

            yPrey = (velocity * Math.Sin(angle) * TimePrey) - (0.5 * 9.8 * Math.Pow(TimePrey, 2)) + yPredator;

            xAquarium = getX(TimeAquarium);

            yAquarium = (velocity * Math.Sin(angle) * TimeAquarium) - (0.5 * 9.8 * Math.Pow(TimeAquarium, 2)) + yPredator;

            // Sorry had to change Point to System.Windows.Point to solve a conflict

            setPredator(new Predator(new System.Windows.Point(xPredator, yPredator)));

            setPrey(new Prey(new System.Windows.Point(xPrey, yPrey), (int)xPrey * (tolerance / 100), (int)yPrey * (tolerance / 100)));

            setAquarium(new Aquarium(new System.Windows.Point(xAquarium, yAquarium), (int)xAquarium * (tolerance / 100), (int)yAquarium * ((tolerance / 2) / 100)));




        }

        /// <summary>
        /// getX is method which return the horizontal displacment of a projectile at certain time. 
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Tamer Nabil </para>
        /// </remarks>
        /// <param name="caseH"></param>
        /// <param name="time"></param>
        /// <returns></returns>

        public Double getX(Double time)
        {

            return CheckPositive(velocity * (Math.Cos(angle)) * time);

        }


        /// <summary>
        /// CheckPostive is a method which check if number is positive or not.If positive it return it and if negative 
        /// it multiply it by -1 to be positive and return it.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Tamer Nabil </para>
        /// </remarks>
        /// <param name="number"></param>
        /// <returns></returns>
        private double CheckPositive(Double number)
        {
            if (number >= 0)
                return number;
            else
                return number * -1;
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

        protected override void Initialize()
        {

            backgroundTexture = this.Content.Load<Texture2D>("Textures/background");
            xyAxisTexture = this.Content.Load<Texture2D>("Textures/xyAxis");
            preyTexture = this.Content.Load<Texture2D>("Textures/worm");
            bowlTexture = this.Content.Load<Texture2D>("Textures/bowl2");
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 720;
            graphics.ApplyChanges();
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
        /// <para>DATE MODIFIED: April, 20  </para>
        /// </remarks>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //add model to array of models
            //models constructor takes the actual model, position vector, rotation vector(x, y, z rotation angels), scaling vector(x, y, z scales) and GraphicsDevice         
            models.Add(new CustomModel(Content.Load<Model>("Models/fish"), new Vector3(-500, -500, -1050), new Vector3(MathHelper.ToRadians(35), MathHelper.ToRadians(-35), 0), new Vector3(0.007f), GraphicsDevice));

            //create still camera
            camera = new TargetCamera(new Vector3(-3000, 100, 0), new Vector3(100, 100, 0), GraphicsDevice);
            //cameras constructor takes position vector and target vector(the point where the camera is looking) 

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
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }


        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Alzayat </para>   
        /// <para>DATE WRITTEN: April, 20 </para>
        /// <para>DATE MODIFIED: April, 20  </para>
        /// </remarks>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            camera.Update();

            // TODO: Add your update logic here

            base.Update(gameTime);
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

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            spriteBatch.Draw(backgroundTexture, Vector2.Zero, Color.White);
            spriteBatch.Draw(xyAxisTexture, Vector2.Zero, Color.White);
            spriteBatch.Draw(bowlTexture, new Vector2(40f, 430f), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            if (!preyEaten)
                spriteBatch.Draw(preyTexture, new Vector2(500f, 200f), null, Color.White, 0f, Vector2.Zero, 0.1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(bowlTexture, new Vector2(750f, 430f), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            spriteBatch.End();

            //GraphicsDevice.BlendState = BlendState.Opaque;
            //GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            //GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;


            foreach (CustomModel model in models)
            {
                model.Draw(camera);
                //takes the camera instance and draws the model 
            }

            base.Draw(gameTime);
        }


        /// <summary>
        /// sets the instance variable of prey to the prey given as parameter
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Tamer Nabil </para>
        /// </remarks>
        /// <param name="prey"></param>
        private void setPrey(Prey prey)
        {
            this.prey = prey;
        }
        /// <summary>
        /// sets the instance variable of Predator to the predator given as parameter
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Tamer Nabil </para>
        /// </remarks>
        /// <param name="predator"></param>
        private void setPredator(Predator predator)
        {
            this.predator = predator;
        }
        /// <summary>
        /// set the instance variable of Aquarium to the Aquarium given as parameter
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Tamer Nabil </para>
        /// </remarks>
        /// <param name="aquarium"></param>

        private void setAquarium(Aquarium aquarium)
        {
            this.aquarium = aquarium;
        }


    }
}
