using ButtonsAndSliders;
using Mechanect.Common;
using Mechanect.Exp1;
using Mechanect.Exp2;
using Mechanect.Exp3;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
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
        SpriteBatch spriteBatch;
        GraphicsDevice device;
        Texture2D backgroundTexture;
        int screenWidth;
        int screenHeight;
        Button experiment1Button;
        Button experiment2Button;
        Button experiment3Button;
        Texture2D One;
        Texture2D Two;
        Texture2D Three;

        [System.Obsolete("Will be repaced by ", false)]
        public AllExperiments()
        {

            user = new User();
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
            this.spriteBatch = ScreenManager.SpriteBatch;
            this.device = ScreenManager.GraphicsDevice;
            this.screenWidth = ScreenManager.GraphicsDevice.Viewport.Width;
            this.screenHeight = ScreenManager.GraphicsDevice.Viewport.Height;
            screenWidth = ScreenManager.GraphicsDevice.Viewport.Width;
            screenHeight = ScreenManager.GraphicsDevice.Viewport.Height;
            One = content.Load<Texture2D>("Resources/Images/exp1-word");
            Two = content.Load<Texture2D>("Resources/Images/exp2-word");
            Three = content.Load<Texture2D>("Resources/Images/exp3-word");

            experiment1Button = new Button(content.Load<GifAnimation.GifAnimation>("Resources/Images/exp1-button-s"),
                content.Load<GifAnimation.GifAnimation>("Resources/Images/exp1-button-m"),
                new Vector2(screenWidth*0.1f, screenHeight * 0.35f), screenWidth, screenHeight,
                content.Load<Texture2D>("Textures/Buttons/hand"), user);

            experiment2Button = new Button(content.Load<GifAnimation.GifAnimation>("Resources/Images/exp2-button-s"),
                 content.Load<GifAnimation.GifAnimation>("Resources/Images/exp2-button-m"),
                 new Vector2(screenWidth*0.39f, screenHeight * 0.6f), screenWidth, screenHeight,
                 content.Load<Texture2D>("Textures/Buttons/hand"), user);
            
            experiment3Button = new Button(content.Load<GifAnimation.GifAnimation>("Resources/Images/exp3-button-s"),
                content.Load<GifAnimation.GifAnimation>("Resources/Images/exp3-button-m"),
                new Vector2(screenWidth*0.7f, screenHeight * 0.35f),
                screenWidth,screenHeight, content.Load<Texture2D>("Textures/Buttons/hand"),
                user);
            backgroundTexture = ScreenManager.Game.Content.Load<Texture2D>(@"Resources/Images/background");
            base.LoadContent();
        }
        /// <summary>
        /// perfroms the necessary updates for the AllExperiments screen.
        /// </summary>
        /// <param name="gameTime">represents the time of the game.</param>
        public override void Update(GameTime gameTime)
        {
            experiment1Button.Update(gameTime);
            experiment2Button.Update(gameTime);
            experiment3Button.Update(gameTime);
            if (experiment1Button.IsClicked())
            {
                Remove();
                Game1.user5 = new User1();
                Game1.user6 = new User1();
                ScreenManager.AddScreen(new InstructionsScreen1(Game1.user5, Game1.user6));
                
            }
            if (experiment2Button.IsClicked())
            {
                Remove();
                Game1.user4 = new User2();
                ScreenManager.AddScreen(new Settings2(Game1.user4));
            }
            if (experiment3Button.IsClicked())
            {
                Remove();
                Game1.user3 = new User3();
                ScreenManager.AddScreen(new InstructionsScreen3(Game1.user3));
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// draws the AllExperiments screen.
        /// </summary>
        /// <param name="gameTime">represents the time of the game.</param>
        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(Color.White);
            spriteBatch.Begin();
            spriteBatch.Draw(backgroundTexture, new Rectangle(0, 0, screenWidth, screenHeight),
                Color.White);

            experiment1Button.Draw(spriteBatch, (screenWidth * 0.65f) / 1024f);
            experiment2Button.Draw(spriteBatch, (screenWidth * 0.65f) / 1024f);
            experiment3Button.Draw(spriteBatch, (screenWidth * 0.65f) / 1024f);
            experiment1Button.DrawHand(spriteBatch);
            spriteBatch.Draw(One, new Rectangle((int)(screenWidth*0.133),
                (int)((screenHeight * 0.47) + ((screenWidth / 1024) * screenHeight * 0.05)), (int)(screenWidth * 0.12),
                (int)(screenHeight * 0.2)), Color.White);
            spriteBatch.Draw(Two, new Rectangle((int)(screenWidth * 0.423),
                (int)((screenHeight * 0.725) + ((screenWidth/1024)*screenHeight*0.05)), (int)(screenWidth * 0.12),
                (int)(screenHeight * 0.2)), Color.White);
            spriteBatch.Draw(Three, new Rectangle((int)(screenWidth * 0.733),
                (int)((screenHeight * 0.47) + ((screenWidth/1024)*screenHeight*0.05)), (int)(screenWidth * 0.12),
                (int)(screenHeight * 0.2)), Color.White);
            experiment1Button.DrawHand(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
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
