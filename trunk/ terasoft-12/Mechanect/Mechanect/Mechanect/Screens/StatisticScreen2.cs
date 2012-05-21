using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Mechanect.Exp2;


namespace Mechanect.Screens
{

    
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class StatisticScreen2 : Mechanect.Common.GameScreen
    {

        Simulation s;
        public StatisticScreen2()
        {
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
           
        }

        public override void LoadContent()
        {
            s = new Simulation(Vector2.Zero, new Rectangle(5, 5, 1, 1), new Rectangle(10, 3, 2, 2), 12, 60);
            s.LoadContent(ScreenManager.Game.Content, ScreenManager.GraphicsDevice.Viewport, ScreenManager.GraphicsDevice);

            
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            s.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            ScreenManager.SpriteBatch.Begin();

            // TODO: Add your update code here
            s.Draw(new Rectangle(100, 50, 500, 300),ScreenManager.Game.Content, ScreenManager.SpriteBatch, ScreenManager.GraphicsDevice.Viewport);
            ScreenManager.SpriteBatch.End();
        }
    }
}
