using ButtonsAndSliders;
using Mechanect.Common;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Mechanect.Exp3;
using Mechanect.Exp1;
using Mechanect.Exp2;
using Microsoft.Xna.Framework;
namespace Mechanect.Screens
{
    /// <summary>
    /// This class represents the screen where the user picks an experiment.
    /// </summary>
    /// <remarks><para>AUTHOR: Ahmed Badr</para></remarks>
    class AllExperiments : Mechanect.Common.GameScreen
    {
        User user;
        ContentManager content;
        SpriteBatch batch;
        GraphicsDevice device;
        Texture2D backgroundTexture;
        int screenWidth;
        int screenHeight;
        Button experiment1;
        Button experiment2;
        Button experiment3;

        [System.Obsolete("Will be repaced by ", false)]
        public AllExperiments()
        {

        }
        /// <summary>
        /// Creates a new instance of AllExperiments
        /// </summary>
        /// <param name="user">The user that will be tracked when this screen is active</param>
        public AllExperiments(User user)
        {
            this.user = user;
        }
        /// <summary>
        /// Loads the content of this screen.
        /// </summary>
        public override void LoadContent()
        {
            this.content = ScreenManager.Game.Content;
            this.batch = ScreenManager.SpriteBatch;
            this.device = ScreenManager.GraphicsDevice;
            this.screenWidth = ScreenManager.GraphicsDevice.Viewport.Width;
            this.screenHeight = ScreenManager.GraphicsDevice.Viewport.Height;
            screenWidth = ScreenManager.GraphicsDevice.Viewport.Width;
            screenHeight = ScreenManager.GraphicsDevice.Viewport.Height;

            experiment1 = Tools3.OKButton(content, new Vector2(0, device.Viewport.Height / 2),
           screenWidth, screenHeight, user);
            experiment2 = Tools3.OKButton(content, new Vector2((device.Viewport.Width / 2) - 70, device.Viewport.Height / 2),
           screenWidth, screenHeight, user);
            experiment3 = Tools3.OKButton(content, new Vector2((device.Viewport.Width) - 150, device.Viewport.Height / 2),
           screenWidth, screenHeight, user);
            backgroundTexture = ScreenManager.Game.Content.Load<Texture2D>(@"Resources/Images/background");
        }
        /// <summary>
        /// perfroms the necessary updates for the AllExperiments screen.
        /// </summary>
        /// <param name="gameTime">represents the time of the game.</param>
        public override void Update(GameTime gameTime)
        {
            experiment1.Update(gameTime);
            experiment2.Update(gameTime);
            experiment3.Update(gameTime);
            if (experiment1.IsClicked())
            {
                Remove();
                //I commented this line to have a compilation-error free repo
               // ScreenManager.AddScreen(new Experiment1(new User1(1), new User1(2), new MKinect()));
            }
            if (experiment2.IsClicked())
            {
                Remove();
                ScreenManager.AddScreen(new Settings2(new User2()));
            }
            if (experiment3.IsClicked())
            {
                Remove();
                ScreenManager.AddScreen(new InstructionsScreen3(new User3()));
            }
        }

        /// <summary>
        /// draws the AllExperiments screen.
        /// </summary>
        /// <param name="gameTime">represents the time of the game.</param>
        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(Color.White);
            ScreenManager.SpriteBatch.Begin();
            ScreenManager.SpriteBatch.Draw(backgroundTexture, new Rectangle(0, 0, ScreenManager.GraphicsDevice.Viewport.Width, ScreenManager.GraphicsDevice.Viewport.Height), Color.White);
            experiment1.DrawHand(ScreenManager.SpriteBatch);
            experiment2.DrawHand(ScreenManager.SpriteBatch);
            experiment3.DrawHand(ScreenManager.SpriteBatch);
            ScreenManager.SpriteBatch.End();
            experiment1.Draw(ScreenManager.SpriteBatch);
            experiment2.Draw(ScreenManager.SpriteBatch);
            experiment3.Draw(ScreenManager.SpriteBatch);

        }

        /// <summary>
        /// removes this screen from the screens that should be managed by the screenManager
        /// </summary>
        public override void Remove()
        {
            base.Remove();

        }


        
    }
}
