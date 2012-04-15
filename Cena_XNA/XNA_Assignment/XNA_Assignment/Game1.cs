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

namespace XNA_Assignment
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D hole;
        Texture2D ball;
        Texture2D wall;
        Texture2D arrow;
        Texture2D speed;
        Vector2 ballPosition;
        Vector2 ballVelocity;
        Vector2 ballAcceleration;
        Vector2 holePosition;
        Vector2 wallPosition;
        Vector2 arrowPosition;
        Vector2 speedPosition;
        Vector2 instructionsIPosition;
        Vector2 instructionsIIPosition;
        Vector2 instructionsIIIPosition;
        float arrowRotation;
        float currentRotation;
        float deltaRotation;
        float deltaRotationAcceleration;
        float currentScale;
        float deltaScale;
        float holeScale;
        float deltaArrowRotation;
        int screenWidth;
        int screenHeight;
        int screenArea;
        Boolean hasShot;
        Boolean BallMoved;
        Vector2 speedpSpeed;
        Vector2 speedBallVelocity;
        Vector2 speedBallAcceleration;
        SpriteFont instructionsI;
        SpriteFont instructionsII;
        SpriteFont instructionsIII;
        Color color;



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


            ballVelocity = new Vector2(0,0);
            ballAcceleration = new Vector2(0.09f, -0.09f);
            currentRotation = 0f;
            currentScale = 1f;
            deltaRotation = 0.065f;
            deltaRotationAcceleration = -0.00005f;
            screenWidth = GraphicsDevice.Viewport.Width;
            screenHeight = GraphicsDevice.Viewport.Height;
            screenArea = screenWidth * screenHeight;
            holeScale = 0.2f;
            arrowRotation = (float)(-1 * Math.PI/4);
            deltaArrowRotation = 0.01f;
            hasShot = false;
            BallMoved = false;
            speedpSpeed = new Vector2(0, -2);
            speedBallVelocity = new Vector2(0, 0);
            speedBallAcceleration = new Vector2(1, 1);
            color = Color.Red;
            

           
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

         

            hole = Content.Load<Texture2D>("hole");
            holePosition = new Vector2(screenWidth - (float)(Math.Sqrt(holeScale) * (hole.Width / 2) * 1.3), ((float)Math.Sqrt(holeScale) * (hole.Height/2 + (screenHeight * 0.05f))) * 1.4f);
            ball = Content.Load<Texture2D>("Ball");
            ballPosition = new Vector2(ball.Width, screenHeight - ball.Height / 2);
            wall = Content.Load<Texture2D>("wall");
            wallPosition = new Vector2(0,0);
            speed = Content.Load<Texture2D>("ball");
            arrow = Content.Load<Texture2D>("arrow");
            arrowPosition = new Vector2(arrow.Width * 2 , screenHeight - (arrow.Height));
            speedPosition = new Vector2(screenWidth - (speed.Width * 0.3f), screenHeight - (speed.Height * 0.3f));
            //deltaScale = (currentScale*ball.Height * ball.Width * 100) / (screenHeight);
            deltaScale = 0.01f;
            instructionsI = Content.Load<SpriteFont>("Instructions");
            instructionsII = Content.Load<SpriteFont>("Instructions");
            instructionsIII = Content.Load<SpriteFont>("Instructions");
            instructionsIPosition = new Vector2(ball.Width * 0.02f, screenHeight - ball.Height * 2f);
            instructionsIIPosition = new Vector2(screenWidth - ball.Width * 2f, screenHeight - ball.Height * 2.5f);
            instructionsIIIPosition = new Vector2(screenWidth/2, screenHeight - ball.Height );
            // TODO: use this.Content to load your game content here
        }


        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            Content.Unload();
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here


            if (!hasShot) {
                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                    if(arrowRotation < Math.PI/2)
                    arrowRotation += deltaArrowRotation;

                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                    if (arrowRotation > -1 * Math.PI/2)
                        arrowRotation -= deltaArrowRotation;
              if(Keyboard.GetState().IsKeyDown(Keys.Up))
                  if (speedPosition.Y >= screenHeight - (speed.Height * 2))
                  {

                      speedPosition += speedpSpeed;
                      speedBallVelocity += speedBallAcceleration;
                  }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Enter) && !BallMoved) 
            {
                hasShot = true;
                BallMoved = true;
                ballVelocity.X =(float) (speedBallVelocity.X * Math.Cos(arrowRotation));
                ballVelocity.Y = (float)(speedBallVelocity.Y * Math.Sin(arrowRotation));
                color = Color.Transparent;
                

            }
               


                if (ballPosition.X >=  (holePosition.X - (Math.Sqrt(holeScale) * hole.Width/2)/15) &&
                    ballPosition.X <= (holePosition.X + (Math.Sqrt(holeScale) * hole.Width /2)/15) &&
                    ballPosition.Y >= (holePosition.Y - (Math.Sqrt(holeScale) * hole.Height /2)/15) &&
                    ballPosition.Y <= (holePosition.Y + (Math.Sqrt(holeScale) * hole.Height /2)/15)  &&
                    ballVelocity.X < 0.04 && ballVelocity.X > -0.04 && ballVelocity.Y < 0.04 && ballVelocity.Y>-0.04
                   )

                {

                    if (currentScale > 0)
                        currentScale -= 0.001f;
                    else {

                        ballPosition = new Vector2(ball.Width, screenHeight - ball.Height / 2);
                        if (hasShot)
                        {
                            speedPosition = new Vector2(screenWidth - (speed.Width * 0.3f), screenHeight - (speed.Height * 0.3f));
                            speedpSpeed = new Vector2(0, -2);
                            speedBallVelocity = new Vector2(0, 0);
                            speedBallAcceleration = new Vector2(1, 1);
                            arrowRotation = (float)(-1 * Math.PI / 4);
                            color = Color.Red;
                        }
                        hasShot = false;
                        BallMoved = false;
                        ballVelocity.X = 0;
                        ballVelocity.Y = 0;
                       
                        
                    }
                }

                else
                {
                    double tempRotation = 0;
                    if (currentRotation > Math.PI / 2 && currentRotation <= Math.PI)
                        tempRotation = Math.PI - currentRotation;
                    if (currentRotation > Math.PI && currentRotation <= (4 / 3) * Math.PI)
                        tempRotation = (4 / 3) * Math.PI - currentRotation;
                    if (currentRotation > (4 / 3) * Math.PI && currentRotation <= 2 * Math.PI)
                        tempRotation = 2 * Math.PI - currentRotation;

                    double xEdge = (Math.Cos(tempRotation - Math.Atan(ball.Width / ball.Height))
                        * Math.Sqrt(Math.Pow(ball.Width / 2, 2) + (Math.Pow(ball.Height / 2, 2))));
                    double yEdge = (Math.Cos(Math.PI / 2 - tempRotation - Math.Atan(ball.Width / ball.Height))
                        * Math.Sqrt(Math.Pow(ball.Width / 2, 2) + (Math.Pow(ball.Height / 2, 2))));


             
                    if (ballPosition.X < xEdge && ballVelocity.X < 0
                        || ballPosition.X > screenWidth - (xEdge * currentScale)/5 && ballVelocity.X > 0)
                    {
                        ballVelocity.X *= -1;
                        ballAcceleration.X *= -1;
                    }

                    if (ballPosition.Y < yEdge && ballVelocity.Y < 0
                        || ballPosition.Y > screenHeight - (yEdge * currentScale)/5 && ballVelocity.Y > 0)
                    {
                        ballVelocity.Y *= -1;
                        ballAcceleration.Y *= -1;
                    }



                    if (ballVelocity.Y > 0.01 || ballVelocity.Y < -0.01
                        || ballVelocity.X > 0.01 || ballVelocity.X < -0.01)
                    {
                        currentRotation = (float)((currentRotation + deltaRotation) % (2 * Math.PI));
                        deltaRotation -= deltaRotationAcceleration;
                        if (!(ballVelocity.X > 0.01 || ballVelocity.X < -0.01))
                        {

                            ballVelocity.X = 0;
                            ballAcceleration.X = 0;
                        }
                        if (ballVelocity.Y > 0.01 || ballVelocity.Y < -0.01)
                        {
                            if (ballVelocity.Y < 0)
                            {
                                if (currentScale > 0)
                                    currentScale -= deltaScale * Math.Abs(ballVelocity.Y) / 5;
                            }
                            else
                            {
                                if (currentScale < 1)
                                    currentScale += deltaScale * Math.Abs(ballVelocity.Y) / 5;
                            }
                            if (currentScale > 1)
                                currentScale = 1;
                            if (currentScale < 0)
                                currentScale = 0.01f;

                        }
                        else
                        {
                            ballVelocity.Y = 0;
                            ballAcceleration.Y = 0;
                        }

                        ballPosition += ballVelocity;
                        ballVelocity -= ballAcceleration;
                
                    }

                    if (ballVelocity.X <= 0.01 && ballVelocity.X >= -0.01 && ballVelocity.Y <= 0.01 && ballVelocity.Y >= -0.01 || currentScale <= 0)
                    {

                        ballPosition = new Vector2(ball.Width, screenHeight - ball.Height / 2);
                        if (hasShot)
                        {
                            speedPosition = new Vector2(screenWidth - (speed.Width * 0.3f), screenHeight - (speed.Height * 0.3f));
                            speedpSpeed = new Vector2(0, -2);
                            speedBallVelocity = new Vector2(0, 0);
                            speedBallAcceleration = new Vector2(1, 1);
                            arrowRotation = (float)(-1 * Math.PI / 4);
                            color = Color.Red;

                        }
                        hasShot = false;
                        BallMoved = false;
                        currentScale = 1f;
                        ballVelocity.X = 0;
                        ballVelocity.Y = 0;
                        ballAcceleration.X = 0.01f;
                        ballAcceleration.Y = -0.01f;
                       
                        

                    } 

                
            }
     

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LawnGreen);

            // TODO: Add your drawing code here

            spriteBatch.Begin();
            spriteBatch.DrawString(instructionsIII, "Press Enter" + '\n' + "to Shoot" , instructionsIIIPosition, color);

            spriteBatch.End();

            spriteBatch.Begin();
            spriteBatch.DrawString(instructionsI, "Hold Left and" + '\n'+ "Right keys" + '\n' + "to adjust Angle",instructionsIPosition ,color);

            spriteBatch.End();

            spriteBatch.Begin();
            spriteBatch.DrawString(instructionsII, "Hold Up key" + '\n' + "to adjust Velocity", instructionsIIPosition, color);

            spriteBatch.End();

            spriteBatch.Begin();

            spriteBatch.Draw(speed, speedPosition, null, Color.White, 0, new Vector2(speed.Width / 2, speed.Height / 2), 0.3f, SpriteEffects.None, 0.0f);

            spriteBatch.End();


            spriteBatch.Begin();

            spriteBatch.Draw(arrow,arrowPosition,null,Color.White,arrowRotation,new Vector2(arrow.Width/2,arrow.Height/2),1f,SpriteEffects.None,0.0f);

            spriteBatch.End();

            spriteBatch.Begin();

            spriteBatch.Draw(wall, new Rectangle(0, 0, screenWidth, screenHeight / 6), Color.White);

            spriteBatch.End();

           
            spriteBatch.Begin();

            spriteBatch.Draw(hole, holePosition, null, Color.White, 0f
                , new Vector2(hole.Width/2,hole.Height/2), holeScale, SpriteEffects.None, 0.0f);

            spriteBatch.End();



            spriteBatch.Begin();

            spriteBatch.Draw(ball, ballPosition, null, Color.White, currentRotation
                , new Vector2(ball.Width / 2, ball.Height / 2), currentScale, SpriteEffects.None, 0.0f);


            spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
