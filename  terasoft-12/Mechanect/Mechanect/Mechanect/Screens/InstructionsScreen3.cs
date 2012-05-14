using Microsoft.Xna.Framework;
using Mechanect.Common;
using Microsoft.Xna.Framework.Graphics;
using Mechanect.Classes;
namespace Mechanect.Screens
{
    class InstructionsScreen3 : GameScreen
    {
        private string instructions = " Welcome to football Mechanect game made by terasoft, The point of this game is to shoot the ball such that it reaches the hole with zero velocity, in the beginning you're shown a screen where you can test shooting and see your angle and velocity, press Ok when you're ready";
        private Instruction instruction;
        private User3 user3;

        public InstructionsScreen3(User3 user3)
        {
            this.user3 = user3;
        }
        public InstructionsScreen3(string instructions, User3 user)
            
        {
            this.instructions = instructions;
            user3 = user;
        }
        /// <summary>
        /// LoadContent will be called only once before drawing and its the place to load
        /// all of your content.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Khaled Salah </para>
        /// </remarks>
        
        
        public override void LoadContent()

        {
            instruction = new Instruction(instructions, ScreenManager.Game.Content, ScreenManager.SpriteBatch,
                ScreenManager.GraphicsDevice, user3);
            instruction.SpriteFont = ScreenManager.Game.Content.Load<SpriteFont>("SpriteFont1");
            instruction.MyTexture = ScreenManager.Game.Content.Load<Texture2D>(@"Textures/screen");
           
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
            if (instruction.Button.IsClicked())
            {
                ExitScreen();
                ScreenManager.AddScreen(new Settings3(user3));
                Remove();
            }
            instruction.Button.Update(gameTime);
            base.Update(gameTime, false);
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

            instruction.Draw(gameTime);
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
