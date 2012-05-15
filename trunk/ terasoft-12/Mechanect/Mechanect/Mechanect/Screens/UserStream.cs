using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Mechanect.Common;
using Microsoft.Kinect;
using Mechanect.Classes;

namespace Mechanect.Screens
{
    public class UserStream : GameScreen
    {
        GraphicsDevice graphics;
        SpriteBatch spriteBatch;
        int screenWidth;
        int screenHeight;
        ContentManager content;
        Texture2D avatar;
        Vector2 avatarPosition;
        SpriteFont font;
        User[] users;
        Boolean[] accepted;
        String[] command;
        int minDepth;
        int maxDepth;
        int depth;
        public UserStream(User user, int minDepth, int maxDepth)
        {
            this.users = new User[1];
            this.accepted = new Boolean[1];
            this.command = new String[1];
            this.users[0] = user;
            this.minDepth = minDepth;
            this.maxDepth = maxDepth;
        }
        public UserStream(User user, User user2, int minDepth, int maxDepth)
        {
            this.users = new User[2];
            this.accepted = new Boolean[2];
            this.command = new String[2];
            this.users[0] = user;
            this.users[1] = user2;
            this.minDepth = minDepth;
            this.maxDepth = maxDepth;
        }

        public override void Initialize()
        {
            graphics = ScreenManager.GraphicsDevice;
            screenWidth = graphics.Viewport.Width;
            screenHeight = graphics.Viewport.Height;
            spriteBatch = ScreenManager.SpriteBatch;
            content = ScreenManager.Game.Content;
            avatarPosition = new Vector2(screenWidth / 2, screenHeight / 4);
            font = content.Load<SpriteFont>("spriteFont1");
            avatar = content.Load<Texture2D>("Textures/screen");
        }
        public void UpdateUser(int ID)
        {
            if (users[ID].USER == null || users[ID].USER.Position.Z == 0)
            {
                command[ID] = "No player detected";
                accepted[ID] = false;
                return;
            }
            depth = GenerateDepth(ID);
        }
        public void UpdateAvatar(Texture2D texture)
        {
            if (users[0].USER != null)
            {
                if (users[0].USER.Position.Z > minDepth && users[0].USER.Position.Z < maxDepth / 4)
                    ChangeTextureColor(texture, Color.Yellow);
                else if (users[0].USER.Position.Z > maxDepth / 4 && users[0].USER.Position.Z < maxDepth / 2)
                    ChangeTextureColor(texture, Color.Green);
                else if (users[0].USER.Position.Z > maxDepth / 2 && users[0].USER.Position.Z < maxDepth)
                    ChangeTextureColor(texture, Color.Blue);
                else if (users[0].USER.Position.Z > maxDepth)
                    ChangeTextureColor(texture, Color.MediumVioletRed);
            }
            else ChangeTextureColor(texture, Color.MediumTurquoise);
            }
        public int GenerateDepth(int i)
        {
            return (int)(100 * users[i].USER.Joints[JointType.HipCenter].Position.Z);
        }

        public override void LoadContent()
        {

        }

        public override void UnloadContent()
        {

        }

        public override void Update(GameTime gameTime, bool covered)
        {
            for (int i = 0; i < users.Length; i++)
                UpdateUser(i);
                UpdateAvatar(avatar);
            base.Update(gameTime, covered);
        }

        public override void Draw(GameTime gameTime)
        {          
                graphics.Clear(Color.Transparent);
                spriteBatch.Begin();
                spriteBatch.Draw(avatar, avatarPosition, null, Color.White, 0, new Vector2(avatar.Width / 2, avatar.Height / 2), 1f, SpriteEffects.None, 0);
                for (int i = 0; i < users.Length; i++)
                {
                    ScreenManager.SpriteBatch.DrawString(font, "Player " + i + " : " + command[i], new Vector2(100, 320 + 100 * i), Color.OrangeRed);
                }
                spriteBatch.End();
        }
        public void ChangeTextureColor(Texture2D texture, Color color)
        {
            Color[] data = new Color[texture.Width * texture.Height];
            texture.GetData(data);

            for (int i = 0; i < data.Length; i++)
                    data[i] = color;

            texture.SetData(data);
        }

        public override void Remove()
        {
            base.Remove();
        }
    }
}
