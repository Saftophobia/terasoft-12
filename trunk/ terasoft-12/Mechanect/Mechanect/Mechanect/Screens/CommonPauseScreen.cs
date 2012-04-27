using Microsoft.Xna.Framework;
using Mechanect.Common;
using Microsoft.Xna.Framework.Graphics;
using Mechanect.Classes;
namespace Mechanect.Screens
{
    class CommonPauseScreen : GameScreen
    {
        private string instructions = "User not detected by kinect device please stand in correct position";
        private Instruction instruction;
        private User user;

        public CommonPauseScreen(User user)
        {
            this.user = user;
        }
        public CommonPauseScreen(string instructions, User user)
        {
            this.instructions = instructions;
            this.user = user;
        }
        /// <summary>
        /// LoadContent will be called only once before drawing and its the place to load
        /// all of your content.
        /// </summary>
        /// <remarks>
        ///<para>AUTHOR: Khaled Salah </para>
        ///</remarks>

        public override void LoadContent()
        {
            instruction = new Instruction(instructions, ScreenManager.Game.Content, ScreenManager.SpriteBatch,
                ScreenManager.GraphicsDevice, user);
            instruction.SpriteFont = ScreenManager.Game.Content.Load<SpriteFont>("SpriteFont1");
            instruction.MyTexture = ScreenManager.Game.Content.Load<Texture2D>(@"Textures/screen");

        }

        /// <summary>
        /// Allows the game screen to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <remarks>
        ///<para>AUTHOR: Khaled Salah </para>
        ///</remarks>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// <param name="covered">Determines whether you want this screen to be covered by another screen or not.</param>

        public override void Update(GameTime gameTime, bool covered)
        {
            if (instruction.Button.IsClicked())
            {
                ExitScreen();
            }
            instruction.Button.Update(gameTime);
            base.Update(gameTime, false);
        }
        /// <summary>
        /// This is called when the game screen should draw itself.
        /// </summary>
        /// <remarks>
        ///<para>AUTHOR: Khaled Salah </para>
        ///</remarks>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>    
        public override void Draw(GameTime gameTime)
        {
            instruction.Draw(gameTime);
        }
        /// <summary>
        /// This is called when you want to exit the screen.
        /// </summary>
        /// <remarks>
        ///<para>AUTHOR: Khaled Salah </para>
        ///</remarks>  
        public override void Remove()
        {
            base.Remove();

        }

    }
}
