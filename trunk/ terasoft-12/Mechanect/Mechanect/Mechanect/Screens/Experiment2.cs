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
        private List<CustomModel> models = new List<CustomModel>();
        private Camera camera;

        //Variables that will change how the Gui will look
        private Boolean preyEaten = false;
        private Boolean grayScreen = true;

        private int screenWidth;
        private int screenHeight;
        private Vector2 velGauge;
        private Vector2 angGauge;

        private Vector2 predatorPosition2D = new Vector2(140f, 630f);
        private Vector2 preyPosition = new Vector2(500f, 200f);
        private Vector2 startAquariumPosition = new Vector2(140f, 630f);
        private Vector2 destinationAquariumPosition = new Vector2(870f, 630f);
        private Vector3 predatorPosition = new Vector3(50, 50, 0);
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
            //graphicsDevice = ScreenManager.GraphicsDevice;
            this.user = user;
        }



        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of the content.
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
            screenWidth = viewPort.Width;
            screenHeight = viewPort.Height;
            content = ScreenManager.Game.Content;
            spriteBatch = ScreenManager.SpriteBatch;
            LoadTextures();
            LoadModels();

            spriteFont = content.Load<SpriteFont>("Ariel");
            velAngleFont = content.Load<SpriteFont>("angleVelFont");

        }

        /// <summary>
        /// Allows the game to draw all the textures 
        /// Initializing the Background and x,y Axises
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Alzayat </para>   
        /// <para>DATE WRITTEN: April, 20 </para>
        /// <para>DATE MODIFIED: April, 21  </para>
        /// </remarks>

        public void LoadTextures()
        {
            backgroundTexture = content.Load<Texture2D>("Textures/background");
            xyAxisTexture = content.Load<Texture2D>("Textures/xyAxis");
            predatorTexture = content.Load<Texture2D>("Textures/fish");
            preyTexture = content.Load<Texture2D>("Textures/worm");
            bowlTexture = content.Load<Texture2D>("Textures/bowl2");
            grayTexture = content.Load<Texture2D>("Textures/GrayScreen");
            velocityTexture = content.Load<Texture2D>("Textures/VelocityGauge");
            angleTexture = content.Load<Texture2D>("Textures/AngleGauge");
            lineConnector = new Texture2D(ScreenManager.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            lineConnector.SetData(new[] { Color.Gray });

            backgroundTextureScaling = 1;
            xyAxisTextureScaling = 1;
            preyTextureScaling = 0.1f;
            bowlTextureScaling = 1;
            
            predatorScaling = 0.2f;
            velocityTextureScaling = 0.5f;
            angleTextureScaling = 0.85f;
            backgroundTextureScaling = ((float)viewPort.Height / (float)backgroundTexture.Height);
            xyAxisTextureScaling = ((float)viewPort.Height / (float)xyAxisTexture.Height);
            grayTextureScaling = ((float)viewPort.Height / (float)grayTexture.Height);
            screenWidth = Convert.ToInt32(backgroundTexture.Width * backgroundTextureScaling);
            

            //backgroundTextureScaling = ((float)viewPort.Width / viewPort.Height) / ((float)backgroundTexture.Width / backgroundTexture.Height);
            //xyAxisTextureScaling = ((float)viewPort.Width / viewPort.Height) / ((float)xyAxisTexture.Width / xyAxisTexture.Height);
            //grayTextureScaling = ((float)viewPort.Width / viewPort.Height) / ((float)grayTexture.Width / grayTexture.Height);

            base.Initialize();
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
            //models.Add(new CustomModel(content.Load<Model>("Models/fish"), new Vector3(-500, -500, -1050), new Vector3(MathHelper.ToRadians(-35), MathHelper.ToRadians(0), 0), new Vector3(fishModelScaling), ScreenManager.GraphicsDevice));

            ////create still camera
            //camera = new TargetCamera(new Vector3(-3000, 0, 0), new Vector3(0, 0, 0), ScreenManager.GraphicsDevice);

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
        {

            //camera.Update();

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
            else
            {
                spriteBatch.Begin();
                String velString = "Velocity = ";
                String angString = "Angle = ";
                spriteBatch.DrawString(velAngleFont, velString + env.Velocity, new Vector2(screenWidth - spriteFont.MeasureString(velString + angString).X / 2, 0), Color.Red);
                spriteBatch.DrawString(velAngleFont, angString + env.Angle, new Vector2(screenWidth - spriteFont.MeasureString(angString).X / 2, 0), Color.Red);
                spriteBatch.End();
            }
            DrawConnectors();

            
           


            // Only Used If 3D Models are Used.

            //foreach (CustomModel model in models)
            //{
            //    model.Draw(camera);
            //    //takes the camera instance and draws the model 
            //}

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
            spriteBatch.Begin();
            spriteBatch.Draw(backgroundTexture, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, backgroundTextureScaling, SpriteEffects.None, 0f);
            spriteBatch.Draw(xyAxisTexture, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, xyAxisTextureScaling, SpriteEffects.None, 0f);
            spriteBatch.Draw(bowlTexture, DrawAtRectangleMidPoint(bowlTexture, startAquariumPosition, bowlTextureScaling), null, Color.White, 0f, Vector2.Zero, bowlTextureScaling, SpriteEffects.None, 0f);
            spriteBatch.Draw(predatorTexture, DrawAtRectangleMidPoint(predatorTexture, predatorPosition2D, predatorScaling), null, Color.White, 0f, Vector2.Zero, predatorScaling, SpriteEffects.None, 0f);
            spriteBatch.Draw(bowlTexture, DrawAtRectangleMidPoint(bowlTexture, destinationAquariumPosition, bowlTextureScaling), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            string meters = "meters";
            spriteBatch.DrawString(velAngleFont, meters, new Vector2(0f, 0f), Color.Red);
            spriteBatch.DrawString(velAngleFont, meters, new Vector2(screenWidth - spriteFont.MeasureString(meters).X / 2, screenHeight - spriteFont.MeasureString(meters).Y / 2), Color.Red);

            if (!preyEaten)
                spriteBatch.Draw(preyTexture, DrawAtRectangleMidPoint(preyTexture, preyPosition, preyTextureScaling), null, Color.White, 0f, Vector2.Zero, preyTextureScaling, SpriteEffects.None, 0f);
            spriteBatch.End();
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
            spriteBatch.Begin();
            DrawLine(spriteBatch, lineConnector, 2f, Color.LightGray, DrawAxisConnector(bowlTexture, startAquariumPosition, bowlTextureScaling), new Vector2(30, DrawAxisConnector(bowlTexture, startAquariumPosition, bowlTextureScaling).Y));
            spriteBatch.DrawString(velAngleFont, startAquariumPosition.Y + "", DrawAtAxisNaming(bowlTexture, startAquariumPosition, bowlTextureScaling, true), Color.Red);
            DrawLine(spriteBatch, lineConnector, 2f, Color.LightGray, DrawAxisConnector(bowlTexture, startAquariumPosition, bowlTextureScaling), new Vector2(DrawAxisConnector(bowlTexture, startAquariumPosition, bowlTextureScaling).X, screenHeight - 30));
            spriteBatch.DrawString(velAngleFont, startAquariumPosition.X + "", DrawAtAxisNaming(bowlTexture, startAquariumPosition, bowlTextureScaling, false), Color.Red);

            DrawLine(spriteBatch, lineConnector, 2f, Color.LightGray, DrawAxisConnector(preyTexture, preyPosition, preyTextureScaling), new Vector2(30, DrawAxisConnector(preyTexture, preyPosition, preyTextureScaling).Y));
            spriteBatch.DrawString(velAngleFont, preyPosition.Y + "", DrawAtAxisNaming(preyTexture, preyPosition, preyTextureScaling, true), Color.Red);

            DrawLine(spriteBatch, lineConnector, 2f, Color.LightGray, DrawAxisConnector(preyTexture, preyPosition, preyTextureScaling), new Vector2(DrawAxisConnector(preyTexture, preyPosition, preyTextureScaling).X, screenHeight - 30));
            spriteBatch.DrawString(velAngleFont, preyPosition.X + "", DrawAtAxisNaming(preyTexture, preyPosition, preyTextureScaling, false), Color.Red);

            DrawLine(spriteBatch, lineConnector, 2f, Color.LightGray, DrawAxisConnector(bowlTexture, destinationAquariumPosition, bowlTextureScaling), new Vector2(30, DrawAxisConnector(bowlTexture, destinationAquariumPosition, bowlTextureScaling).Y));
            spriteBatch.DrawString(velAngleFont, destinationAquariumPosition.Y + "", DrawAtAxisNaming(bowlTexture, destinationAquariumPosition, bowlTextureScaling, true), Color.Red);
            DrawLine(spriteBatch, lineConnector, 2f, Color.LightGray, DrawAxisConnector(bowlTexture, destinationAquariumPosition, bowlTextureScaling), new Vector2(DrawAxisConnector(bowlTexture, destinationAquariumPosition, bowlTextureScaling).X, screenHeight - 30));
            spriteBatch.DrawString(velAngleFont, destinationAquariumPosition.X + "", DrawAtAxisNaming(bowlTexture, destinationAquariumPosition, bowlTextureScaling, false), Color.Red);
            spriteBatch.End();
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
            spriteBatch.Begin();
            
            
            spriteBatch.Draw(grayTexture, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, grayTextureScaling, SpriteEffects.None, 0f);
            spriteBatch.Draw(velocityTexture, new Vector2(screenWidth / 16, screenHeight / 14), null, Color.White, 0f, Vector2.Zero, velocityTextureScaling, SpriteEffects.None, 0f);
            spriteBatch.Draw(angleTexture, new Vector2(screenWidth - screenWidth * 2 / 9, screenHeight / 14), null, Color.White, 0f, Vector2.Zero, angleTextureScaling, SpriteEffects.None, 0f);
            string testString = "Test angle and Velocity";
            spriteBatch.DrawString(spriteFont, testString, new Vector2((screenWidth / 4), 0), Color.Red);
            string sayString = "Say 'GO' or press OK";
            spriteBatch.DrawString(spriteFont, sayString, new Vector2((screenWidth / 4), screenHeight - 2*spriteFont.MeasureString(sayString).Y), Color.Red);
            String velString = "Velocity = " + env.Velocity;
            String angString = "Angle = " + env.Angle;
            spriteBatch.DrawString(velAngleFont, velString, new Vector2((screenWidth / 4)  +velocityTexture.Width*velocityTextureScaling/2  - spriteFont.MeasureString(velString).X, (screenHeight / 14) + velocityTexture.Height * velocityTextureScaling - spriteFont.MeasureString(velString).Y), Color.Red);
            spriteBatch.DrawString(velAngleFont, angString, new Vector2(screenWidth - ((screenWidth*2 / 9) +angleTexture.Width* angleTextureScaling  - spriteFont.MeasureString(angString).X), (screenHeight / 14) + velocityTexture.Height * velocityTextureScaling - spriteFont.MeasureString(velString).Y), Color.Red);
            spriteBatch.End();
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
        Vector2 DrawAtRectangleMidPoint(Texture2D texture,Vector2 position, float scale)
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
               return  new Vector2(3, position.Y + texture.Height * scale / 2 - spriteFont.MeasureString(position.Y + "").X / 4);
            else
               return  new Vector2(position.X + texture.Width * scale / 2 - spriteFont.MeasureString(position.X + "").Y / 4, screenHeight - 40);
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

        public override void Remove()
        {
            base.Remove();
        }
    }
}
