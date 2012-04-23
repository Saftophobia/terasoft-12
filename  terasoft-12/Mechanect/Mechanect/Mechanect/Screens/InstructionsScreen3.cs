using Mechanect.Common;
using Microsoft.Xna.Framework;
using MechanectXNA;
namespace Mechanect.Screens
{
    class InstructionsScreen3 : GameScreen
    {
        private OKButton button;

        public InstructionsScreen3()
        {
        }

        /// <summary>
        /// load the screen's content
        /// </summary>
        /// <remarks>
        /// Auther : Bishoy Bassem
        /// </remarks>
        public override void LoadContent()
        {
            button = new OKButton(this.ScreenManager.Game.Content, new Vector2(300, 300), this.ScreenManager.GraphicsDevice.Viewport.Width, this.ScreenManager.GraphicsDevice.Viewport.Height);
        }

        public override void UnloadContent()
        {

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
            var position = new Vector2(100, 20);
            var instructions = "The goal of this game is to shoot the ball such that it reaches the hole with 0 velocity";
            var I = new Instruction(instructions, position);
            I.Draw2(gameTime);
            button.draw(this.ScreenManager.SpriteBatch);
        }
    
        public override void Remove()
        {
            base.Remove();
        }
    }
}
