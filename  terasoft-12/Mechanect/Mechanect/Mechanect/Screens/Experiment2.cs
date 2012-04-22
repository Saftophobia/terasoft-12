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

        //list of models to be drawn
        //private List<CustomModel> models = new List<CustomModel>();
        //private Camera camera;

        //Variables that will change how the Gui will look
        private Boolean preyEaten = false;
        private Boolean grayScreen = true;
        private float translation;
        private int screenWidth;
        private int screenHeight;
        //To be used in sprint 2
        //private Vector2 velGauge;
        //private Vector2 angGauge;
        private Vector2 predatorOrigin;
        private Vector2 predatorPosition2D ;
        private Vector2 preyPosition;
        private Vector2 startAquariumPosition;
        private Vector2 destinationAquariumPosition;
        //private Vector3 predatorPosition = new Vector3(50, 50, 0);
        float backgroundTextureScaling;
        float xyAxisTextureScaling;
        float preyTextureScaling;
        float bowlTextureScaling;
        float grayTextureScaling;
        float velocityTextureScaling;
        float angleTextureScaling;
        float predatorScaling;

        //Only if 3D is used
        //float fishModelScaling;

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

           
            LoadTextures(2);
            LoadModels();
            //give initial values
            startAquariumPosition = env.Predator.Location;
            LoadObjectsWithValues(env.Predator.Location, env.Prey.Location,env.Aquarium.Location);
          

            spriteFont = Content.Load<SpriteFont>("Ariel");
            velAngleFont = Content.Load<SpriteFont>("angleVelFont");

            // zayat you can edit the button place as you wish
            buttonPosition = new Vector2(450,10);
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
            predatorPosition2D = (predator);
            preyPosition = (prey);
            destinationAquariumPosition = (destination);

            //predatorPosition2D = MapPointsToScreen(predator);
            //preyPosition = MapPointsToScreen(prey);
            //destinationAquariumPosition = MapPointsToScreen(destination);
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
            switch(setNumber){
                case 1: ImageSet1(); break;
                default: ImageSet2(); break;
            }

            predatorOrigin = (new Vector2(predatorTexture.Width*predatorScaling / 2, predatorTexture.Height*predatorScaling / 2));
            backgroundTextureScaling = ((float)ViewPort.Height / (float)backgroundTexture.Height);
            xyAxisTextureScaling = ((float)ViewPort.Height / (float)xyAxisTexture.Height);
            grayTextureScaling = ((float)ViewPort.Height / (float)grayTexture.Height);
            screenWidth = Convert.ToInt32(backgroundTexture.Width * backgroundTextureScaling);
            screenHeight = Convert.ToInt32(backgroundTexture.Height * backgroundTextureScaling);
            translation = 40 * xyAxisTextureScaling;

            //backgroundTextureScaling = ((float)ViewPort.Width / ViewPort.Height) / ((float)backgroundTexture.Width / backgroundTexture.Height);
            //xyAxisTextureScaling = ((float)ViewPort.Width / ViewPort.Height) / ((float)xyAxisTexture.Width / xyAxisTexture.Height);
            //grayTextureScaling = ((float)ViewPort.Width / ViewPort.Height) / ((float)grayTexture.Width / grayTexture.Height);

            base.Initialize();
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
            grayTexture = Content.Load<Texture2D>("Textures/Experiment2/ImageSet1/GrayScreen");
            velocityTexture = Content.Load<Texture2D>("Textures/Experiment2/ImageSet1/VelocityGauge");
            angleTexture = Content.Load<Texture2D>("Textures/Experiment2/ImageSet1/AngleGauge");
            lineConnector = new Texture2D(ScreenManager.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            lineConnector.SetData(new[] { Color.Gray });

            backgroundTextureScaling = 1;
            xyAxisTextureScaling = 1;
            preyTextureScaling = 0.1f;
            bowlTextureScaling = 1;

            predatorScaling = 0.2f;
            velocityTextureScaling = 0.5f;
            angleTextureScaling = 0.85f;
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

            backgroundTextureScaling = 1;
            xyAxisTextureScaling = 1;
            preyTextureScaling = 0.1f;
            bowlTextureScaling = 0.12f;

            predatorScaling = 0.5f;
            velocityTextureScaling = 0.5f;
            angleTextureScaling = 0.85f;
        }
        /// <summary>
        /// LoadModels will be called once per game and is the place to load
        /// all of the Models.
        /// Only Used If 3D Models are Used.
        /// Loaded the Fish Model
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Alzayat </para>   
        /// <para>DATE WRITTEN: April, 20 </para>
        /// <para>DATE MODIFIED: April, 21  </para>
        /// </remarks>
        private void LoadModels()
        {
            //add model to array of models
            //fishModelScaling = 0.007f;
            //models.Add(new CustomModel(Content.Load<Model>("Models/fish"), new Vector3(-500, -500, -1050), new Vector3(MathHelper.ToRadians(-35), MathHelper.ToRadians(0), 0), new Vector3(fishModelScaling), ScreenManager.GraphicsDevice));

            ////create still camera
            //camera = new TargetCamera(new Vector3(-3000, 0, 0), new Vector3(0, 0, 0), ScreenManager.GraphicsDevice);

        }


        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all Content.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Alzayat </para>   
        /// <para>DATE WRITTEN: April, 20 </para>
        /// <para>DATE MODIFIED: April, 20  </para>
        /// </remarks>
        public override void UnloadContent()
        {

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
            return new Vector2(position.X+translation,screenHeight-(position.Y+translation));
            

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
            DrawConnectors();
            // Only Used If 3D Models are Used.

            //foreach (CustomModel model in models)
            //{
            //    model.Draw(camera);
            //    //takes the camera instance and draws the model 
            //}

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
                SpriteBatch.DrawString(velAngleFont, velString, new Vector2((screenWidth / 16) + velocityTexture.Width * velocityTextureScaling  - spriteFont.MeasureString(velString).X*3/5, (screenHeight / 14) + velocityTexture.Height * velocityTextureScaling - spriteFont.MeasureString(velString).Y), Color.Red);
                SpriteBatch.DrawString(velAngleFont, angString, new Vector2(screenWidth - ((screenWidth * 2 / 9) + angleTexture.Width * angleTextureScaling - spriteFont.MeasureString(angString).X), (screenHeight / 14) + velocityTexture.Height * velocityTextureScaling - spriteFont.MeasureString(velString).Y), Color.Red);
            }
            else
            {
                SpriteBatch.DrawString(velAngleFont, velString + env.Velocity, new Vector2(screenWidth - spriteFont.MeasureString(velString + angString).X / 2, 0), Color.Red);
                SpriteBatch.DrawString(velAngleFont, angString + env.Angle, new Vector2(screenWidth - spriteFont.MeasureString(angString).X / 2, 0), Color.Red);
            }
            SpriteBatch.End();
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
            SpriteBatch.Draw(predatorTexture, DrawAtRectangleMidPoint(predatorTexture, predatorPosition2D, predatorScaling), null, Color.White, MathHelper.ToRadians(0), Vector2.Zero, predatorScaling, SpriteEffects.None, 0f);
            SpriteBatch.Draw(bowlTexture, DrawAtRectangleMidPoint(bowlTexture, destinationAquariumPosition, bowlTextureScaling), null, Color.White, 0f, Vector2.Zero, bowlTextureScaling, SpriteEffects.None, 0f);
            string meters = "meters";
            SpriteBatch.DrawString(velAngleFont, meters, new Vector2(0f, 0f), Color.Red);
            SpriteBatch.DrawString(velAngleFont, meters, new Vector2(screenWidth - spriteFont.MeasureString(meters).X / 2, screenHeight - spriteFont.MeasureString(meters).Y / 2), Color.Red);

            if (!preyEaten)
                SpriteBatch.Draw(preyTexture, DrawAtRectangleMidPoint(preyTexture, preyPosition, preyTextureScaling), null, Color.White, 0f, Vector2.Zero, preyTextureScaling, SpriteEffects.None, 0f);
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
        private void DrawConnectors()
        {
            SpriteBatch.Begin();
            DrawLine(SpriteBatch, lineConnector, 2f, Color.LightGray, DrawAxisConnector(bowlTexture, startAquariumPosition, bowlTextureScaling), new Vector2(30, DrawAxisConnector(bowlTexture, startAquariumPosition, bowlTextureScaling).Y));
            SpriteBatch.DrawString(velAngleFont, screenHeight- startAquariumPosition.Y + "", DrawAtAxisNaming(bowlTexture, startAquariumPosition, bowlTextureScaling, true), Color.Red);
            DrawLine(SpriteBatch, lineConnector, 2f, Color.LightGray, DrawAxisConnector(bowlTexture, startAquariumPosition, bowlTextureScaling), new Vector2(DrawAxisConnector(bowlTexture, startAquariumPosition, bowlTextureScaling).X, screenHeight - 30));
            SpriteBatch.DrawString(velAngleFont, startAquariumPosition.X + "", DrawAtAxisNaming(bowlTexture, startAquariumPosition, bowlTextureScaling, false), Color.Red);

            DrawLine(SpriteBatch, lineConnector, 2f, Color.LightGray, DrawAxisConnector(preyTexture, preyPosition, preyTextureScaling), new Vector2(30, DrawAxisConnector(preyTexture, preyPosition, preyTextureScaling).Y));
            SpriteBatch.DrawString(velAngleFont, screenHeight - preyPosition.Y + "", DrawAtAxisNaming(preyTexture, preyPosition, preyTextureScaling, true), Color.Red);

            DrawLine(SpriteBatch, lineConnector, 2f, Color.LightGray, DrawAxisConnector(preyTexture, preyPosition, preyTextureScaling), new Vector2(DrawAxisConnector(preyTexture, preyPosition, preyTextureScaling).X, screenHeight - 30));
            SpriteBatch.DrawString(velAngleFont, preyPosition.X + "", DrawAtAxisNaming(preyTexture, preyPosition, preyTextureScaling, false), Color.Red);

            DrawLine(SpriteBatch, lineConnector, 2f, Color.LightGray, DrawAxisConnector(bowlTexture, destinationAquariumPosition, bowlTextureScaling), new Vector2(30, DrawAxisConnector(bowlTexture, destinationAquariumPosition, bowlTextureScaling).Y));
            SpriteBatch.DrawString(velAngleFont, screenHeight - destinationAquariumPosition.Y + "", DrawAtAxisNaming(bowlTexture, destinationAquariumPosition, bowlTextureScaling, true), Color.Red);
            DrawLine(SpriteBatch, lineConnector, 2f, Color.LightGray, DrawAxisConnector(bowlTexture, destinationAquariumPosition, bowlTextureScaling), new Vector2(DrawAxisConnector(bowlTexture, destinationAquariumPosition, bowlTextureScaling).X, screenHeight - 30));
            SpriteBatch.DrawString(velAngleFont, destinationAquariumPosition.X + "", DrawAtAxisNaming(bowlTexture, destinationAquariumPosition, bowlTextureScaling, false), Color.Red);
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
            SpriteBatch.Draw(velocityTexture, new Vector2(screenWidth / 16, screenHeight / 14), null, Color.White, 0f, Vector2.Zero, velocityTextureScaling, SpriteEffects.None, 0f);
            SpriteBatch.Draw(angleTexture, new Vector2(screenWidth - screenWidth * 2 / 9, screenHeight / 14), null, Color.White, 0f, Vector2.Zero, angleTextureScaling, SpriteEffects.None, 0f);
            string testString = "Test angle and Velocity";
            SpriteBatch.DrawString(spriteFont, testString, new Vector2((screenWidth / 4), 0), Color.Red);
            string sayString = "Say 'GO' or press OK";
            SpriteBatch.DrawString(spriteFont, sayString, new Vector2((screenWidth / 4), screenHeight - 2 * spriteFont.MeasureString(sayString).Y), Color.Red);
            SpriteBatch.End();
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
        /// <param name="batch">Takes the SpriteBatch that will draw the line </param>
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
        /// <param name="texture">Texture that needs to be drawn with the position at it's center</param>
        /// <param name="position">The position of the center of the texture</param>
        /// <param name="scale">Takes the scaling of the Texture2D</param>
        /// <param name="x">Gives a check to know if the axis used is x or y axis</param>
        /// <returns>A Vector2 that will be used as a position for drawing</returns>
        /// 
        Vector2 DrawAtAxisNaming(Texture2D texture, Vector2 position, float scale, Boolean y)
        {
            position = DrawAtRectangleMidPoint(texture, position, scale);
            if (y)
                return new Vector2(3, position.Y + texture.Height * scale / 2 - spriteFont.MeasureString(position.Y + "").X / 4);
            else
                return new Vector2(position.X + texture.Width * scale / 2 - spriteFont.MeasureString(position.X + "").Y / 4, screenHeight - 40);
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
        /// <param name="x">Gives a check to know if the axis used is x or y axis</param>
        /// <returns>A Vector2 that will be used as a position for drawing</returns>
        /// 
        Vector2 DrawAxisConnector(Texture2D texture, Vector2 position, float scale)
        {
            position = DrawAtRectangleMidPoint(texture, position, scale);
            return new Vector2(position.X + texture.Width * scale / 2, position.Y + texture.Height * scale / 2);
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

            else if (/*button.clicked() ||*/ voiceCommand.getHeared("ok"))
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

        
        public override void Remove()
        {
            base.Remove();
        }
    }
}
