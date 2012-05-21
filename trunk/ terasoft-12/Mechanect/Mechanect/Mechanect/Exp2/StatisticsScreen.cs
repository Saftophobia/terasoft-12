using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mechanect.Common;
using ButtonsAndSliders;
//using System.Windows.Shapes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Mechanect.Exp3;

namespace Mechanect.Exp2
{
    class StatisticsScreen : GameScreen
    {
        private Simulation simulation;
        private Button mainMenue;
        private Button retry;
        private Button solution;
        private Button newGame;
        private bool winOrLose;
        private Texture2D hand;
        private ContentManager content;
        private SpriteBatch spritebatch;
        private GifAnimation.GifAnimation texture, animation, stopped;
        private Texture2D hand;
        Vector2 mainMenuePositionWin;
        Vector2 mainMenusePositiionLost;
        Vector2 retryWin;
        Vector2 retryLost;
        Vector2 newGamePositionLost;
        Vector2 newGamePositionWin;
        Vector2 seeResultsPosition;
        User user;

        public StatisticsScreen(Vector2 predatorPosition, Rectangle preyPosition, Rectangle aquariumPosition, float velocity,
            float angle, bool winOrLose,Game game)
        {
            this.simulation = new Simulation(predatorPosition, preyPosition, aquariumPosition, velocity, angle);
            this.content = game.Content;
            winOrLose = false;
        }
        public void Initialize()
        {
             mainMenuePositionWin = new Vector2((float)0.1665*ScreenManager.GraphicsDevice.Viewport.Width,
                (float)0.8325*ScreenManager.GraphicsDevice.Viewport.Height);

             mainMenusePositiionLost = new Vector2((float)0.1665 * ScreenManager.GraphicsDevice.Viewport.Width,
                (float)0.8325 * ScreenManager.GraphicsDevice.Viewport.Height); 

             retryWin = new Vector2((float)0.1665 * ScreenManager.GraphicsDevice.Viewport.Width,
                (float)0.8325 * ScreenManager.GraphicsDevice.Viewport.Height); 

             retryLost = new Vector2((float)0.1665 * ScreenManager.GraphicsDevice.Viewport.Width,
                (float)0.8325 * ScreenManager.GraphicsDevice.Viewport.Height); 

             newGamePositionLost = new Vector2((float)0.1665 * ScreenManager.GraphicsDevice.Viewport.Width,
                (float)0.8325 * ScreenManager.GraphicsDevice.Viewport.Height); 

             newGamePositionWin = new Vector2((float)0.1665 * ScreenManager.GraphicsDevice.Viewport.Width,
                (float)0.8325 * ScreenManager.GraphicsDevice.Viewport.Height); 

             seeResultsPosition = new Vector2((float)0.1665 * ScreenManager.GraphicsDevice.Viewport.Width,
                (float)0.8325 * ScreenManager.GraphicsDevice.Viewport.Height); 

            
        }
        //Still waiting for Hegazy to do the buttons for me.
        public void LoadContent()
        {
            simulation.LoadContent(content, ScreenManager.GraphicsDevice.Viewport);
            if (!winOrLose)
            {
                mainMenue = Tools3.MainMenuButton(content, mainMenuePositionWin, ScreenManager.GraphicsDevice.Viewport.Width,
                    ScreenManager.GraphicsDevice.Viewport.Width, user);

                retry = Tools3.MainMenuButton(content, mainMenuePositionWin, ScreenManager.GraphicsDevice.Viewport.Width,
                    ScreenManager.GraphicsDevice.Viewport.Width, user);

                solution = Tools3.MainMenuButton(content, mainMenuePositionWin, ScreenManager.GraphicsDevice.Viewport.Width,
                    ScreenManager.GraphicsDevice.Viewport.Width, user);
            }
            else
            {
                newGame = Tools3.NewGameButton(content, mainMenuePositionWin, ScreenManager.GraphicsDevice.Viewport.Width,
                    ScreenManager.GraphicsDevice.Viewport.Width, user);

                mainMenue = Tools3.MainMenuButton(content, mainMenuePositionWin, ScreenManager.GraphicsDevice.Viewport.Width,
                    ScreenManager.GraphicsDevice.Viewport.Width, user);

                retry = Tools3.MainMenuButton(content, mainMenuePositionWin, ScreenManager.GraphicsDevice.Viewport.Width,
                    ScreenManager.GraphicsDevice.Viewport.Width, user);
            }
        }

        public void Draw()
        {

        }

        public void Update(GameTime gametime)
        {
            simulation.Update(gametime);
        }
       
    }

    
}
