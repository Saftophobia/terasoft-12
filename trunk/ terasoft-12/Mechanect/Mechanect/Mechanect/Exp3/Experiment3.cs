using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using UI.Cameras;
using UI.Animation;
using UI.Components;
using Mechanect.Classes;
using Physics;
using Mechanect.Screens;
using ButtonsAndSliders;


namespace Mechanect.Exp3
{
    public class Experiment3 : Mechanect.Common.GameScreen
    {
        private Ball ball;
        private Bar bar;
        private Environment3 environment;
        private User3 user;

        private TargetCamera targetCamera;
        private BallAnimation animation;
        private Simulation simulation;

        private bool pauseScreenShowed;
        private bool firstAnimation;
        private bool hasWhistled;

        private float arriveVelocity;
        private Vector3 shootVelocity;

        private Button mainMenu;
        private Button newGame;

        private SoundEffect whistle, crowd;

        /// <summary>
        /// Creates a new Experiment3 game screen.
        /// </summary>
        /// <param name="user">User3 instance.</param>
        /// <remarks>
        /// AUTHOR : Bishoy Bassem.
        /// </remarks>
        public Experiment3(User3 user)
        {
            ball = new Ball(2.5f);
            ball.GenerateBallMass(0.004f, 0.006f);

            environment = new Environment3(user);

            arriveVelocity = 10;
            firstAnimation = true;
            user.shootingPosition = new Vector3(0, 3, 45);
            this.user = user;
        }

        /// <summary>
        /// Loads the experiment's environment and buttons.
        /// </summary>
        /// <remarks>
        /// AUTHOR : Bishoy Bassem.
        /// </remarks>
        public override void LoadContent()
        {
            targetCamera = new TargetCamera(new Vector3(0, 30, 95), new Vector3(0,20,0), 
                ScreenManager.GraphicsDevice);

            environment.InitializeUI(ScreenManager.Game.Content, ScreenManager.GraphicsDevice);
            environment.LoadContent();
           
            ball.LoadContent(ScreenManager.Game.Content.Load<Model>(@"Models/ball"));
            ball.GenerateInitialPosition(environment.TerrainWidth, environment.TerrainWidth);
           
            environment.ball = ball;

            Vector3 initialVelocity = LinearMotion.CalculateInitialVelocity(user.shootingPosition - ball.Position, 
                arriveVelocity, Environment3.Friction);

            animation = new BallAnimation(ball, environment, initialVelocity);

            bar = new Bar(new Vector2(ScreenManager.GraphicsDevice.Viewport.Width - 10, 
                ScreenManager.GraphicsDevice.Viewport.Height - 225), ScreenManager.SpriteBatch, 
                new Vector2(ball.Position.X, ball.Position.Z), new Vector2(ball.Position.X, ball.Position.Z), 
                new Vector2(user.shootingPosition.X, user.shootingPosition.Z), ScreenManager.Game.Content);

            //whistle = ScreenManager.Game.Content.Load<SoundEffect>("whistle");

            //crowd = ScreenManager.Game.Content.Load<SoundEffect>("crowd_cheer");
            //crowd.Play();

            InitializeButtons();

            base.LoadContent();
        }

        /// <summary>
        /// Updates the experiment's screen.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// <remarks>
        /// AUTHOR : Bishoy Bassem.
        /// </remarks>
        public override void Update(GameTime gameTime)
        {
            environment.PlayerModel.Update();
            environment.PlayerAnimation.Update();
            UpdateFirstAnimation(gameTime);
            UpdateSecondAnimation();
            if (simulation != null)
            {
                UpdateButtons(gameTime);
                simulation.Update(gameTime);
            }
            else
                animation.Update(gameTime.ElapsedGameTime);
            
            targetCamera.Update();
            base.Update(gameTime);
        }

        /// <summary>
        /// Updates the first animation.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// <remarks>
        /// AUTHOR : Bishoy Bassem.
        /// </remarks>
        public void UpdateFirstAnimation(GameTime gameTime)
        {
            if (!firstAnimation)
                return;
            
            float distance = animation.Displacement.Length();
            float totalDistance = (user.shootingPosition - animation.StartPosition).Length();
            if (distance / totalDistance > 0.5 && !pauseScreenShowed)
            {
                pauseScreenShowed = true;
                FreezeScreen();
                ScreenManager.AddScreen(new PauseScreen(user, arriveVelocity, ball.Mass, user.assumedLegMass, 
                    environment.HoleProperty.Position));
            }
            bar.Update(new Vector2(ball.Position.X, ball.Position.Z));
            if (ball.HasBallEnteredShootRegion())
            {
                environment.arriveVelocity = arriveVelocity;
                /*if (!hasWhistled)
                {
                    whistle.Play();
                    hasWhistled = true;
                }*/
                user.UpdateMeasuringVelocityAndAngle(gameTime);
                Vector3 shootVelocity = user.velocity;
                if (user.hasShot && shootVelocity.Length() != 0)
                {
                    firstAnimation = false;
                    this.shootVelocity = environment.GetVelocityAfterCollision(shootVelocity);
                    animation = new BallAnimation(ball, environment, this.shootVelocity);
                }
            }

            if (animation.Finished)
                UpdateButtons(gameTime);
        }

        /// <summary>
        /// Updates the second animation.
        /// </summary>
        /// <remarks>
        /// AUTHOR : Bishoy Bassem.
        /// </remarks>
        public void UpdateSecondAnimation()
        {
            if (firstAnimation)
                return;
            
            if (!ball.InsideTerrain(environment.TerrainWidth, environment.TerrainHeight))
                animation.Stop();
            
            if (animation.Finished && simulation == null)
                simulation = new Simulation(ball, environment, user.shootingPosition, shootVelocity,
                    ScreenManager.Game.Content, ScreenManager.GraphicsDevice, ScreenManager.SpriteBatch);
        }

        /// <summary>
        /// Draws the experiment's screen.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// <remarks>
        /// AUTHOR : Bishoy Bassem.
        /// </remarks>
        public override void Draw(GameTime gameTime)
        {
            Camera camera = targetCamera;
            if (simulation != null)
                camera = simulation.Camera;
            
            environment.Draw(camera, gameTime);
            ball.Draw(camera);
            if (firstAnimation)
            {
                float distance = animation.Displacement.Length();
                float totalDistance = (user.shootingPosition - animation.StartPosition).Length();
                if (distance / totalDistance > 1)
                {
                    //DrawStatus();
                    DrawButtons();
                }
                else
                    bar.Draw();
               
            }
            if (simulation != null)
            {
                simulation.Draw();
                DrawButtons();
                //DrawStatus();
            }
            base.Draw(gameTime);
        }

        /// <summary>
        /// Initializes the experiment's buttons.
        /// </summary>
        /// <remarks>
        /// AUTHOR : Bishoy Bassem.
        /// </remarks>
        private void InitializeButtons()
        {
            int screenWidth = this.ScreenManager.GraphicsDevice.Viewport.Width;
            int screenHeight = this.ScreenManager.GraphicsDevice.Viewport.Height;

            mainMenu = Tools3.MainMenuButton(ScreenManager.Game.Content, new Vector2(screenWidth - 105,
                screenHeight - 105), screenWidth, screenHeight, user);

            newGame = Tools3.NewGameButton(ScreenManager.Game.Content, new Vector2(screenWidth / 2 - 70,
                screenHeight - 150), screenWidth, screenHeight, user);
        }

        /// <summary>
        /// Updates the experiment's buttons.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// <remarks>
        /// AUTHOR : Bishoy Bassem.
        /// </remarks>
        private void UpdateButtons(GameTime gameTime)
        {
            mainMenu.Update(gameTime);
            newGame.Update(gameTime);
            if (mainMenu.IsClicked())
            {
                Remove();
                ScreenManager.AddScreen(new AllExperiments((User3)Game1.user3));
            }
            if (newGame.IsClicked())
            {
                Remove();
                ScreenManager.AddScreen(new Experiment3((User3)Game1.user3));
            }
        }

        /// <summary>
        /// Draws the experiment's buttons.
        /// </summary>
        /// <remarks>
        /// AUTHOR : Bishoy Bassem.
        /// </remarks>
        private void DrawButtons()
        {
            ScreenManager.SpriteBatch.Begin();
            newGame.Draw(ScreenManager.SpriteBatch, 0.6f);
            mainMenu.Draw(ScreenManager.SpriteBatch, 0.4f);
            mainMenu.DrawHand(ScreenManager.SpriteBatch);
            ScreenManager.SpriteBatch.End();
        }

    }

}
