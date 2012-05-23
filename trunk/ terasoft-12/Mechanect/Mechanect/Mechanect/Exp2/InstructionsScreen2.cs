 using Microsoft.Xna.Framework;
using Mechanect.Common;
using Microsoft.Xna.Framework.Graphics;
using Mechanect.Classes;
using Mechanect.Screens;
using ButtonsAndSliders;
using Mechanect.Exp3;

namespace Mechanect.Exp2
{
    class InstructionsScreen2 : GameScreen
    {
        string instructions;
        User2 user;
        Button button;

        public InstructionsScreen2(User2 user2)
        {
            instructions = " Hey there instructions : \n \nThe Goal of the game is to throw the fish in the aquarium passing by the prey. " + 
                "You should move your arm from back to front " + 
                "to a certain angle with some velocity to determine the angle and velocity of projectile.";
            this.user = user2;
        }
        public InstructionsScreen2(string instructions, User2 user)
        {
            this.instructions = instructions;
            this.user = user;
        }

        public override void LoadContent()
        {
            
            float screenWidth = ScreenManager.GraphicsDevice.Viewport.Width;
            float screenHeight = ScreenManager.GraphicsDevice.Viewport.Height;
            button = Tools3.OKButton(ScreenManager.Game.Content, new Vector2(screenWidth / 1.75f, 0.5f * screenHeight),
                ScreenManager.GraphicsDevice.Viewport.Width, ScreenManager.GraphicsDevice.Viewport.Height, user);
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            user.setSkeleton();
            button.Update(gameTime);
            if (button.IsClicked())
            {
                Remove();
                ScreenManager.AddScreen(new AdjustPosition(user, 150, 350, -40, 40, 2));
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.SpriteBatch.Begin();
            int screenWidth = ScreenManager.GraphicsDevice.Viewport.Width;
            int screenHeight = ScreenManager.GraphicsDevice.Viewport.Height;
            UI.UILib.Write(instructions, new Rectangle(screenWidth / 5, screenHeight / 5, 3 * screenWidth / 5, 3 * screenHeight / 5), ScreenManager.SpriteBatch, ScreenManager.Game.Content);
            button.Draw(ScreenManager.SpriteBatch,0.7f);
            button.DrawHand(ScreenManager.SpriteBatch);
            ScreenManager.SpriteBatch.End();
            base.Draw(gameTime);
        }
        public override void Remove()
        {
            base.Remove();

        }


    }
}