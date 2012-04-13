using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XNA_Task
{
    class Sprite
    {
        public Vector2 position;
        public bool active;
        public Texture2D texture;
        float projectileMoveSpeed;
        float enemyMoveSpeed;

        public void Initialize(Texture2D tex, Vector2 pos)
        {
            texture = tex;
            position = pos;
            active = true;
            projectileMoveSpeed = 8.0f;
            enemyMoveSpeed = 6f;
        }

        public void Update(Vector2 velocity)
        {
            position += velocity;
        }

        public void Update(Viewport viewport)
        {
            // Projectiles always move to the right
            position.X += projectileMoveSpeed;

            // Deactivate the bullet if it goes out of screen
            if (position.X + texture.Width / 2 > viewport.Width)
                active = false;
        }

        public void Update(GameTime gameTime)
        {
            position.X -= enemyMoveSpeed;


            if (position.X < -texture.Width)
            {
                active = false;
            }
        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
            
    }
}
