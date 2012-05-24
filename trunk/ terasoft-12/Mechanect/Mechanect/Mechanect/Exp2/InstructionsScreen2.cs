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
       private string instructions = "";
        string header= "\n\n\n                  .Welcome To Mechanect Projectile Game."+ '\n'
             + "                           .By TeraSoft Team."  + '\n'
               +"                                 .GUC.";
        private string title1="\n\n\n\n\n\n      Game Instructions:-";
        private string text1 = "\n\n\n\n\n\n\n\n\n\n\n          1-Goal: Use projectile equations to calculate  angle and velocity  " +
            "\n          that the fish can be thrown by to eat the prey and fall in the aquarium.  \n "
            +"\n          2-Givens: Predator Point,Prey Point,Aquarium point \n"+ 
            "            \n"
          +
            "\n          3-Settings: The next screen will tell you how to adjust your position for the game.\n";
        private string text2 = "\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n          * The avatar on the top right \n            represents your distance from\n            the screen.\n";
        private string green = "\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n                                                                             Green: Good.";
        private string white = "\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n                                                                             White: Too Far.";
        private string red = "\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n                                                                             Red: Too Near.";
        private string crossed = "\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n                                                                             Crossed: Not Detected.";
        private User2 user2;
        private Texture2D myTexture;
        private Rectangle rect;
        private Button button;
        private float scale;
        SpriteFont font1;
        SpriteFont font2;
        SpriteFont font3;

        public InstructionsScreen2(User2 user2)
        {
            this.user2 = user2;
        }

        public InstructionsScreen2(string instructions, User2 user2): this(user2)
        {
            this.instructions = instructions;
        }
        
        public override void LoadContent()
        {
            myTexture = ScreenManager.Game.Content.Load<Texture2D>(@"Textures/Screens/instructions");
            scale = ((float)(ScreenManager.GraphicsDevice.Viewport.Width) / (float)myTexture.Width);
            rect = new Rectangle(0, 0, (int)(scale * myTexture.Width), (int)(scale * myTexture.Height));
            font1 = ScreenManager.Game.Content.Load<SpriteFont>("SpriteFont4");
            font2 = ScreenManager.Game.Content.Load<SpriteFont>("SpriteFont5");
            font3 = ScreenManager.Game.Content.Load<SpriteFont>("SpriteFont6");
            button = Tools3.OKButton(ScreenManager.Game.Content,
            new Vector2(ScreenManager.GraphicsDevice.Viewport.Width - 496, ScreenManager.GraphicsDevice.Viewport.Height - 196), ScreenManager.GraphicsDevice.Viewport.Width,
            ScreenManager.GraphicsDevice.Viewport.Height, user2);
            base.LoadContent();
        }
        public override void Update(GameTime gameTime)
        {
            user2.setSkeleton();
            button.Update(gameTime);
            if (button.IsClicked())
            {
                Remove();
                ScreenManager.AddScreen(new AdjustPosition(user2, 150, 350, -40, 40, 2));
            }
            base.Update(gameTime);

        }
        public override void Draw(GameTime gameTime)
        {
            ScreenManager.SpriteBatch.Begin();
            ScreenManager.SpriteBatch.Draw(myTexture, rect, Color.White);
            button.Draw(ScreenManager.SpriteBatch, scale);
            button.DrawHand(ScreenManager.SpriteBatch);
            ScreenManager.SpriteBatch.DrawString(font1, header, Vector2.Zero, Color.DarkViolet, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
            ScreenManager.SpriteBatch.DrawString(font2, title1, Vector2.Zero, Color.DarkRed, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
            ScreenManager.SpriteBatch.DrawString(font3, text1, Vector2.Zero, Color.Black, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
            ScreenManager.SpriteBatch.DrawString(font3, text2, Vector2.Zero, Color.Black, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
            ScreenManager.SpriteBatch.DrawString(font3, green, Vector2.Zero, Color.DarkGreen, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
            ScreenManager.SpriteBatch.DrawString(font3, white, Vector2.Zero, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
            ScreenManager.SpriteBatch.DrawString(font3, red, Vector2.Zero, Color.DarkRed, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
            ScreenManager.SpriteBatch.DrawString(font3, crossed, Vector2.Zero, Color.DarkGray, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
            ScreenManager.SpriteBatch.End();
            base.Draw(gameTime);
        }
      
        public void Remove()
        {
            base.Remove();
        }

    }
}

