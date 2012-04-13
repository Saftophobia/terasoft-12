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

namespace XNA_Task
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Sprite player;
        int score = 0;
        SpriteFont font;
        List<Sprite> enemies;
        List<Sprite> projectiles;
        Vector2 movePlayer;
        Texture2D enemyTexture, projectileTexture;
        Random random;

        TimeSpan enemySpawnTime;
        TimeSpan previousSpawnTime;

        TimeSpan fireTime;
        TimeSpan previousFireTime;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            player = new Sprite();

            enemies = new List<Sprite>();
            projectiles = new List<Sprite>();

            enemySpawnTime = TimeSpan.FromSeconds(1.0f);
            fireTime = TimeSpan.FromSeconds(.15f);

            random = new Random();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            Texture2D playerTexture = Content.Load<Texture2D>("player");
            player.Initialize(playerTexture, new Vector2(0, GraphicsDevice.Viewport.TitleSafeArea.Y / 2));

            font = Content.Load<SpriteFont>("gameFont");
            enemyTexture = Content.Load<Texture2D>("mine");
            projectileTexture = Content.Load<Texture2D>("laser");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            KeyboardState k = Keyboard.GetState();
            // TODO: Add your update logic here
            updateEnemies(gameTime);

            if ((k.IsKeyDown(Keys.Down) && movePlayer.Y < 0) || (k.IsKeyDown(Keys.Up) && movePlayer.Y > 0))
                movePlayer.Y = -movePlayer.Y;
            else
            {
                if (k.IsKeyDown(Keys.Down))
                {
                    movePlayer = new Vector2(0, 4);
                }
                else
                {
                    if (k.IsKeyDown(Keys.Up))
                        movePlayer = new Vector2(0, -4);
                    else
                        movePlayer = Vector2.Zero;
                }
            }

            player.Update(movePlayer);
            updateProjectiles(gameTime);
            checkCollision();

            base.Update(gameTime);
        }

        private void updateEnemies(GameTime gameTime)
        {
            if (gameTime.TotalGameTime - previousSpawnTime > enemySpawnTime)
            {
                previousSpawnTime = gameTime.TotalGameTime;

                Sprite e = new Sprite();
                Vector2 position = new Vector2(GraphicsDevice.Viewport.Width + enemyTexture.Width / 2, random.Next(100, GraphicsDevice.Viewport.Height - 100));
                e.Initialize(enemyTexture, position);

                enemies.Add(e);
            }

            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                enemies[i].Update(gameTime);

                if (enemies[i].active == false)
                {
                    enemies.RemoveAt(i);
                }
            }
        }

        private void updateProjectiles(GameTime gameTime)
        {
            if (gameTime.TotalGameTime - previousFireTime > fireTime)
            {
                // Reset our current time
                previousFireTime = gameTime.TotalGameTime;

                // Add the projectile, but add it to the front and center of the player
                Vector2 projPos = player.position + new Vector2(player.texture.Width / 2, player.texture.Height/2 - 10);
                Sprite projectile = new Sprite();
                projectile.Initialize(projectileTexture, projPos);
                projectiles.Add(projectile);

            }

            for (int i = projectiles.Count - 1; i >= 0; i--)
            {
                projectiles[i].Update(GraphicsDevice.Viewport);

                if (projectiles[i].active == false)
                {
                    projectiles.RemoveAt(i);
                }
            }
        }

        private void checkCollision()
        {
            Rectangle r1;
            Rectangle r2;

            r1 = new Rectangle((int)player.position.X,
            (int)player.position.Y,
            player.texture.Width,
            player.texture.Height);

            for (int i = 0; i < enemies.Count; i++)
            {
                r2 = new Rectangle((int)enemies[i].position.X,
                (int)enemies[i].position.Y,
                enemies[i].texture.Width,
                enemies[i].texture.Height);

                if (r1.Intersects(r2))
                {
                    
                        player.active = false;
                }

            }

            for (int i = 0; i < projectiles.Count; i++)
            {
                for (int j = 0; j < enemies.Count; j++)
                {
                    r1 = new Rectangle((int)projectiles[i].position.X -
                    projectiles[i].texture.Width / 2, (int)projectiles[i].position.Y -
                    projectiles[i].texture.Height / 2, projectiles[i].texture.Width, projectiles[i].texture.Height);

                    r2 = new Rectangle((int)enemies[j].position.X - enemies[j].texture.Width / 2,
                    (int)enemies[j].position.Y - enemies[j].texture.Height / 2,
                    enemies[j].texture.Width, enemies[j].texture.Height);

                    if (r1.Intersects(r2))
                    {
                        score++;
                        enemies[j].active = false;
                        projectiles[i].active = false;
                    }
                }
            }
        }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkSlateGray);
            spriteBatch.Begin();
            // TODO: Add your drawing code here
            foreach (Sprite e in enemies)
                e.draw(spriteBatch);
            foreach (Sprite p in projectiles)
                p.draw(spriteBatch);
            player.draw(spriteBatch);
            spriteBatch.DrawString(font, "score: " + score, new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X, GraphicsDevice.Viewport.TitleSafeArea.Y), Color.White);
            base.Draw(gameTime);

            spriteBatch.End();
        }
    }
}
