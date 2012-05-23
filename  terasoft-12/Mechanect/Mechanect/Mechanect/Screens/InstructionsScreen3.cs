using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mechanect.Common;
using Mechanect.Exp3;
using ButtonsAndSliders;
namespace Mechanect.Screens
{
    class InstructionsScreen3 : GameScreen
    {
        private string instructions = "\n\n\n\n Welcome to Mechanect football game made by terasoft \n The point of this game is to shoot the ball such that it reaches the hole with zero velocity. \n In the beginning you're shown a screen where you can test shooting and see your angle and velocity. \n The avatar on the right represents your distance from the kinect device, below is the color code \n Green: you're standing in the correct position, White: you're almost too far, Red: Not all your skeleton is detected.";
        private Instruction instruction;
        private User3 user3;
        private Texture2D myTexture;
        private Rectangle rect;
        private Button button;
        private float scale;
        public InstructionsScreen3(User3 user3)
        {
            this.user3 = user3;
        }
        public InstructionsScreen3(string instructions, User3 user3): this(user3)
        {
            this.instructions = instructions;
        }
        /// <summary>
        /// LoadContent will be called only once before drawing and its the place to load
        /// all of your content.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Khaled Salah </para>
        /// </remarks
        public override void LoadContent()
        {
            myTexture = ScreenManager.Game.Content.Load<Texture2D>(@"Textures/Screens/instructions");
            scale = ((float)(ScreenManager.GraphicsDevice.Viewport.Width) / (float)myTexture.Width);
            rect = new Rectangle(0, 0, (int)(scale * myTexture.Width), (int)(scale * myTexture.Height));
            instruction = new Instruction(instructions, ScreenManager.Game.Content, ScreenManager.SpriteBatch,
                ScreenManager.GraphicsDevice, user3,rect);
            instruction.SpriteFont = ScreenManager.Game.Content.Load<SpriteFont>("SpriteFont1");
            instruction.LoadContent();
            button = Tools3.OKButton(ScreenManager.Game.Content,
            new Vector2(ScreenManager.GraphicsDevice.Viewport.Width - 496, ScreenManager.GraphicsDevice.Viewport.Height - 196), ScreenManager.GraphicsDevice.Viewport.Width,
            ScreenManager.GraphicsDevice.Viewport.Height, user3);
            base.LoadContent();
        }

        /// <summary>
        /// Allows the game screen to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio
        /// and detects if the user clicked the button to skip this screen.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Khaled Salah, Bishoy Bassem </para>
        /// </remarks>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// <param name="covered">Determines whether you want this screen to be covered by another screen or not.</param>
        
        public override void Update(GameTime gameTime, bool covered)
        {
            if (button.IsClicked())
            {
                ScreenManager.AddScreen(new Settings3(user3));
                Remove();
            }
            button.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game screen should draw itself.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Khaled Salah </para>
        /// </remarks>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>    
        public override void Draw(GameTime gameTime)
        {
            ScreenManager.SpriteBatch.Begin();
            ScreenManager.SpriteBatch.Draw(myTexture, rect, Color.White);
            button.Draw(ScreenManager.SpriteBatch, scale);
            button.DrawHand(ScreenManager.SpriteBatch);
            ScreenManager.SpriteBatch.End();
            instruction.Draw(gameTime);
            base.Draw(gameTime);
        }
        /// <summary>
        /// This is called when you want to exit the screen.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Khaled Salah </para>
        /// </remarks>  
        public void Remove()
        {
            base.Remove();
            
        }

    }
}
