using System.Windows;
using ButtonsAndSliders;
using Mechanect.Common;
using Mechanect.Exp2;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mechanect.Exp2
{
    /// <summary>
    /// Class statistics Screen , responsible for showing the statistics for the player after playing
    /// </summary>
    /// <remarks>
    /// <para>AUTHOR: Mohamed raafat</para>
    /// </remarks>
    class StatisticsScreen : GameScreen
    {
        /// <summary>
        /// Instance Variables
        /// </summary>
        #region:Instance Variables
        private User2 user;
        private float optimalVelocity;
        private float optimalAngle;
        private Simulation userSimulation;
        private Simulation optimalSimulation;
        private Rectangle leftSimulationPosition;
        private Rectangle rightSimulationPosition;
        private Rectangle centerSimulationPosition;
        private Rectangle currentUserSimulationPosition;
        private Vector2 mainMenuButtonPosition;
        private Vector2 retryButtonPosition;
        private Vector2 newGameButtonPosition;
        private Vector2 seeResultsButtonPosition;
        private bool correctAnswer;
        private bool solutionVisible;
        private Vector2 predatorPosition;
        private Rect preyPosition;
        private Rect aquariumPosition;
        private Button mainMenu;
        private Button retry;
        private Button solution;
        private Button newGame;
        private Texture2D background;
        private Viewport viewPort
        {
            get { return ScreenManager.GraphicsDevice.Viewport; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor to initialize the instance variables
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Raafat</para>
        /// </remarks>
        /// <param name="predatorPosition">Vector2 for predator position</param>
        /// <param name="preyPosition">Rect, containing position and dimension of the prey</param>
        /// <param name="aquariumPosition">Rect, containing position and dimension of the aquarium</param>
        /// <param name="userVelocity">float, containing the user velocity</param>
        /// <param name="userAngle">float, containing the user angle</param>
        /// <param name="user">user, instance of the User2 class</param>

        public StatisticsScreen(Vector2 predatorPosition, Rect preyPosition, Rect aquariumPosition, float userVelocity,
            float userAngle, User2 user)
        {

            this.userSimulation = new Simulation(predatorPosition, preyPosition, aquariumPosition, userVelocity, userAngle);
            this.user = user;
            this.optimalVelocity = userVelocity;
            this.optimalAngle = userAngle;
            this.predatorPosition = predatorPosition;
            this.preyPosition = preyPosition;
            this.aquariumPosition = aquariumPosition;
            correctAnswer = true;
        }

        /// <summary>
        /// Constructor to initialize the instance variables
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Raafat</para>
        /// </remarks>
        /// <param name="predatorPosition">Vector2 for predator position</param>
        /// <param name="preyPosition">Rect, containing position and dimension of the prey</param>
        /// <param name="aquariumPosition">Rect, containing position and dimension of the aquarium</param>
        /// <param name="userVelocity">float, containing the user velocity</param>
        /// <param name="userAngle">float, containing the user angle</param>
        /// <param name="optimalVelocity">float, containing the correct optimal velocity</param>
        /// <param name="optimalAngle">float, containing the correct optimal angle</param>
        /// <param name="user">user, instance of User2 class</param>

        public StatisticsScreen(Vector2 predatorPosition, Rect preyPosition, Rect aquariumPosition, float userVelocity,
            float userAngle, float optimalVelocity, float optimalAngle, User2 user)
        {
            this.userSimulation = new Simulation(predatorPosition, preyPosition, aquariumPosition, userVelocity, userAngle);
            this.optimalSimulation = new Simulation(predatorPosition, preyPosition, aquariumPosition, optimalVelocity, optimalAngle);
            this.user = user;
            this.optimalAngle = optimalAngle;
            this.optimalVelocity = optimalVelocity;
            this.predatorPosition = predatorPosition;
            this.preyPosition = preyPosition;
            this.aquariumPosition = aquariumPosition;
            this.correctAnswer = false;
        }
        #endregion

        /// <summary>
        /// Initialize the positions of the buttons and simulation graphs
        /// <remarks>
        /// <para>AUTHOR: Mohamed Raafat</para>
        /// </remarks>
        /// </summary>
        public override void Initialize()
        {
            mainMenuButtonPosition = new Vector2((float)0.01 * ScreenManager.GraphicsDevice.Viewport.Width,
               (float)0.7 * ScreenManager.GraphicsDevice.Viewport.Height);

            retryButtonPosition = new Vector2((float)0.38 * ScreenManager.GraphicsDevice.Viewport.Width,
               (float)0.7 * ScreenManager.GraphicsDevice.Viewport.Height);

            newGameButtonPosition = new Vector2((float)0.75 * ScreenManager.GraphicsDevice.Viewport.Width,
               (float)0.7 * ScreenManager.GraphicsDevice.Viewport.Height);

            seeResultsButtonPosition = new Vector2((float)0.75 * ScreenManager.GraphicsDevice.Viewport.Width,
               (float)0.7 * ScreenManager.GraphicsDevice.Viewport.Height);

            rightSimulationPosition = new Rectangle(7 * viewPort.Width / 12, viewPort.Height / 10, viewPort.Width / 2,
                5 * viewPort.Height / 10); ///
            centerSimulationPosition = new Rectangle(viewPort.Width / 3, viewPort.Height / 10, viewPort.Width / 2,
                5 * viewPort.Height / 10);
            leftSimulationPosition = new Rectangle(viewPort.Width / 12, viewPort.Height / 10, viewPort.Width / 2,
                5 * viewPort.Height / 10); ///
            currentUserSimulationPosition = centerSimulationPosition; 
        }
        /// <summary>
        /// Load the simulation and the buttons
        /// <remarks>
        /// <para>AUTHOR: Mohamed Raafat</para>
        /// </remarks>
        /// </summary>
        public override void LoadContent()
        {
            background = ScreenManager.Game.Content.Load<Texture2D>("Textures/Experiment2/ImageSet1/background");

            userSimulation.LoadContent(ScreenManager.Game.Content, ScreenManager.GraphicsDevice);
            if (!correctAnswer)
                optimalSimulation.LoadContent(ScreenManager.Game.Content, ScreenManager.GraphicsDevice);

            mainMenu = Mechanect.Exp3.Tools3.MainMenuButton(ScreenManager.Game.Content, mainMenuButtonPosition,
                ScreenManager.GraphicsDevice.Viewport.Width,
                ScreenManager.GraphicsDevice.Viewport.Height, user);

            retry = Mechanect.Exp3.Tools3.RetryButton(ScreenManager.Game.Content, retryButtonPosition,
                ScreenManager.GraphicsDevice.Viewport.Width,
                ScreenManager.GraphicsDevice.Viewport.Height, user);

            newGame = Mechanect.Exp3.Tools3.NewGameButton(ScreenManager.Game.Content, newGameButtonPosition,
                ScreenManager.GraphicsDevice.Viewport.Width,
                ScreenManager.GraphicsDevice.Viewport.Height, user);



            if (!correctAnswer)
                solution = Mechanect.Exp3.Tools3.SolutionButton(ScreenManager.Game.Content, seeResultsButtonPosition,
                    ScreenManager.GraphicsDevice.Viewport.Width,
                ScreenManager.GraphicsDevice.Viewport.Width, user);
            base.LoadContent();
        }

        /// <summary>
        /// Draw the Simulation and Buttons
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Raafat</para>
        /// </remarks>
        /// <param name="gameTime">gameTime, provide snapshot of the GameTime class</param>
        public override void Draw(GameTime gameTime)
        {
            ScreenManager.SpriteBatch.Begin();
            ScreenManager.SpriteBatch.Draw(background, new Rectangle(0, 0, viewPort.Width, viewPort.Height), Color.White);
            mainMenu.Draw(ScreenManager.SpriteBatch, viewPort.Width / 1024f);
            retry.Draw(ScreenManager.SpriteBatch, viewPort.Width / 1024f);
            userSimulation.Draw(currentUserSimulationPosition, ScreenManager.SpriteBatch);
            if (correctAnswer || solutionVisible)
                newGame.Draw(ScreenManager.SpriteBatch, viewPort.Width / 1024f);
            else
                solution.Draw(ScreenManager.SpriteBatch, viewPort.Width / 1024f);
            if (currentUserSimulationPosition.X <= leftSimulationPosition.X)
                optimalSimulation.Draw(rightSimulationPosition, ScreenManager.SpriteBatch);
            mainMenu.DrawHand(ScreenManager.SpriteBatch);
            ScreenManager.SpriteBatch.End();
            base.Draw(gameTime);

        }
        /// <summary>
        /// Update the objects drawns on the screen
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Raafat</para>
        /// </remarks>
        /// <param name="gametime">gameTime, provide snapshot of the GameTime class</param>
        public override void Update(GameTime gametime)
        {

            mainMenu.Update(gametime);
            retry.Update(gametime);
            userSimulation.Update(gametime);
            if (mainMenu.IsClicked())
            {

                ScreenManager.AddScreen(new Mechanect.Screens.AllExperiments(user));
                this.Remove();
            }
            if (retry.IsClicked())
            {

                ScreenManager.AddScreen(new Experiment2(user, predatorPosition, preyPosition, aquariumPosition,
                    optimalVelocity, optimalAngle));
                this.Remove();
            }

            if (correctAnswer || solutionVisible)
            {
                newGame.Update(gametime);
                if (newGame.IsClicked())
                {
                    ScreenManager.AddScreen(new Experiment2(user));
                    this.Remove();
                }
                if (!correctAnswer)
                {
                    if (currentUserSimulationPosition.X > leftSimulationPosition.X)
                    {
                        currentUserSimulationPosition.Offset(-2, 0);
                        if (currentUserSimulationPosition.X < leftSimulationPosition.X)
                            currentUserSimulationPosition = leftSimulationPosition;
                    }
                    else
                    {
                        optimalSimulation.Update(gametime);
                    }
                }
            }
            else
            {
                solution.Update(gametime);
                if (solution.IsClicked())
                {
                    solutionVisible = true;
                }
            }
            base.Update(gametime);
        }
    }

}
