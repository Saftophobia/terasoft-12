using Microsoft.Xna.Framework;
using Mechanect.Common;
using Microsoft.Xna.Framework.Graphics;
using Mechanect.Classes;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
namespace Mechanect.Screens
{
    class AllExperiments : GameScreen
    {
        string instructions = " Press A to choose Experiment1" +"\n" + " B to choose Experiment2" + "\n" + " C to choose Experiment3";
        User user;
        ContentManager content;
        SpriteBatch batch;
        GraphicsDevice device;
        int screenWidth;
        int screenHeight;
        public SpriteFont Font1 { get; set; }

        public Texture2D MyTexture { get; set; }
       // Button button1;
       // Button button2;
        // Button button3;
        public AllExperiments()
        {
        }
        public AllExperiments(ContentManager content, SpriteBatch batch, GraphicsDevice device, User user, int screenWidth, int screenHeight)
        {
            this.user = user;
            this.content = content;
            this.batch = batch;
            this.device = device;
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
        }

        /// <remarks>
        ///<para>AUTHOR: Khaled Salah </para>
        ///</remarks>
        /// <summary>
        /// LoadContent will be called only once before drawing and its the place to load
        /// all of your content.
        /// </summary>

            /// <remarks>
        ///<para>AUTHOR: Khaled Salah </para>
        ///</remarks>
        /// <summary>
        /// LoadContent will be called only once before drawing and its the place to load
        /// all of your content.
        /// </summary>
        public override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            screenWidth = ScreenManager.GraphicsDevice.Viewport.Width;
            screenHeight = ScreenManager.GraphicsDevice.Viewport.Height;
         //   b =  Tools3.OKButton(cmanager,new Vector2(device.Viewport.Width / 2, device.Viewport.Height-400),
          // screenWidth , screenHeight,user );
           Font1 = ScreenManager.Game.Content.Load<SpriteFont>(@"SpriteFont1");
            MyTexture = ScreenManager.Game.Content.Load<Texture2D>(@"Textures/screen");
            // TODO: use this.Content to load your game content here
        }

            
         //   button1 = Tools3.OKButton(content, new Vector2((device.Viewport.Width / 2)-200, device.Viewport.Height - 1200),
           //screenWidth, screenHeight, user);
           // button2 = Tools3.OKButton(content, new Vector2((device.Viewport.Width / 2)-400, device.Viewport.Height - 800),
           //screenWidth, screenHeight, user);
            //button3 = Tools3.OKButton(content, new Vector2((device.Viewport.Width / 2)-800, device.Viewport.Height - 400),
           //screenWidth, screenHeight, user);
        


        /// <remarks>
        ///<para>AUTHOR: Khaled Salah </para>
        ///</remarks>
        /// <summary>
        /// Allows the game screen to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// <param name="covered">Determines whether you want this screen to be covered by another screen or not.</param>

        public override void Update(GameTime gameTime, bool covered)
        {

            KeyboardState keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.A))
            {
                ExitScreen();
                ScreenManager.AddScreen(new Experiment1(new User1(), new User1(), new MKinect()));
            }
            if (keyState.IsKeyDown(Keys.B))
            {
                ExitScreen();
                ScreenManager.AddScreen(new Experiment2(new User2()));
            }
            if (keyState.IsKeyDown(Keys.C))
            {
                ExitScreen();
                ScreenManager.AddScreen(new InstructionsScreen2(new User2()));
            }
          //  instruction.Button.Update(gameTime);
            //button1.Update(gameTime);
            //button2.Update(gameTime);
            //button3.Update(gameTime);
         //   base.Update(gameTime, false);
        }

        /// <remarks>
        ///<para>AUTHOR: Khaled Salah </para>
        ///</remarks>
        /// <summary>
        /// This is called when the game screen should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>    
        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(Color.YellowGreen);
            // b.Draw(spriteBatch);
            ScreenManager.SpriteBatch.Begin();
            ScreenManager.SpriteBatch.DrawString(Font1, instructions, new Vector2(), Color.Black, 0, new Vector2(), 1f, SpriteEffects.None, 0.0f);
            ScreenManager.SpriteBatch.End();
            //instruction.Draw(gameTime);
          //  button1.Draw(batch);
           // button2.Draw(batch);
           // button3.Draw(batch);
        }
        public override void Remove()
        {
            base.Remove();

        }


        
    }
}
