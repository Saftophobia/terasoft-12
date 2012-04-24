using Mechanect.Common;
using Microsoft.Xna.Framework;
using MechanectXNA;
namespace Mechanect.Screens
{
    class InstructionsScreen3 : Instruction
    {
        Vector2 position = new Vector2(100, 20);
        private OKButton button;
        string instructions = " The point of this game is to shoot the ball that it reaches the hole with zero velocity";
        public InstructionsScreen3()
            : base()
        {
        }
        public InstructionsScreen3(string instructions)
            : base(instructions)
        {
            this.instructions = instructions;
        }

        /// <summary>
        /// load the screen's content
        /// </summary>
        /// <remarks>
        /// Auther : Bishoy Bassem
        /// </remarks>
        public override void LoadContent()
        {
            base.LoadContent();
            button = new OKButton(this.ScreenManager.Game.Content, new Vector2(300, 300), this.ScreenManager.GraphicsDevice.Viewport.Width, this.ScreenManager.GraphicsDevice.Viewport.Height);
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        /// <summary>
        /// updates the screen
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="covered"></param>
        /// <remarks>
        /// Auther : Bishoy Bassem
        /// </remarks>
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, bool covered)
        {

            if (button.isClicked())
            {
                this.Remove();
            }
            base.Update(gameTime, covered);
        }

        /// <remarks>
        ///<para>AUTHOR: Khaled Salah, Bishoy Bassem </para>
        ///</remarks>
        /// <summary>
        /// Draws the how to play instruction screen related to experiment3.
        /// </summary>
        /// <param name="gameTime">
        /// An instance variable of class GameTime in Microsoft.Xna.Framework.GameTime package.
        /// </param>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            button.draw(this.ScreenManager.SpriteBatch);
        }

        public override void Remove()
        {
            base.Remove();
        }
    }
}
