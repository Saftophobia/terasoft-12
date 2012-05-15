using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Mechanect.Common;
using Microsoft.Kinect;
using Mechanect.Classes;

namespace Mechanect.Screens
{
    public class UserAvatarScreen : GameScreen
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
        String[] command;
        int minDepth;
        int maxDepth;
        int depth;
        public UserAvatarScreen(User user, int minDepth, int maxDepth)
        {
            this.users = new User[1];
            this.command = new String[1];
            this.users[0] = user;
            this.minDepth = minDepth;
            this.maxDepth = maxDepth;
        }
        public UserAvatarScreen(User user, User user2, int minDepth, int maxDepth)
        {
            this.users = new User[2];
            this.command = new String[2];
            this.users[0] = user;
            this.users[1] = user2;
            this.minDepth = minDepth;
            this.maxDepth = maxDepth;
        }
        /// <summary>
        /// LoadContent will be called only once before drawing and its the place to load
        /// all of your content.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Khaled Salah </para>
        /// </remarks>
        public override void LoadContent()
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

        /// <summary>
        /// Takes player index in the array and updates his distance from the kinect device, and adds a message to be printed if not detected or too far away.
        /// </summary>
        /// <remarks>
        ///<para>AUTHOR: Khaled Salah </para>
        ///</remarks>
        /// <param name="ID">
        /// The user's index in users array.
        /// </param>
        public void UpdateUser(int ID)
        {
            if (users[ID].USER == null || users[ID].USER.Position.Z == 0)
            {
                command[ID] = "No player detected";
                return;
            }
            depth = GenerateDepth(ID);
        }

        /// <summary>
        /// Takes a 2D texture as a parameter and colors it according to user's distance from the kinect device.
        /// </summary>
        /// <remarks>
        ///<para>AUTHOR: Khaled Salah </para>
        ///</remarks>
        /// <param name="texture">
        /// The 2D texture that should be colored.
        /// </param>
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

        /// <summary>
        /// Takes the user's index in the array and gets his distance from the kinect device.
        /// </summary>
        /// <remarks>
        ///<para>AUTHOR: Khaled Salah </para>
        ///</remarks>
        /// <param name="index">
        /// The user's index in the array.
        /// </param>
        /// <returns>
        /// Int number which is the calculated depth.
        /// </returns>
        public int GenerateDepth(int index)
        {
            return (int)(100 * users[index].USER.Joints[JointType.HipCenter].Position.Z);
        }

        /// <summary>
        /// Allows the game screen to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <remarks>
        ///<para>AUTHOR: Khaled Salah </para>
        ///</remarks>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// <param name="covered">Determines whether you want this screen to be covered by another screen or not.</param>
        public override void Update(GameTime gameTime, bool covered)
        {
            for (int i = 0; i < users.Length; i++)
                UpdateUser(i);
                UpdateAvatar(avatar);
            base.Update(gameTime, covered);
        }

        /// <summary>
        /// This is called when the game screen should draw itself.
        /// </summary>
        /// <remarks>
        ///<para>AUTHOR: Khaled Salah </para>
        ///</remarks>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>    
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

        /// <summary>
        /// Takes as parameters a 2D texture and a color and changes the texture's color to the specified color.
        /// </summary>
        /// <remarks>
        ///<para>AUTHOR: Khaled Salah </para>
        ///</remarks>
        /// <param name="texture">
        /// The texture which should be colored.
        /// </param>
        /// /// <param name="color">
        /// The color you want your texture's color to be changed to.
        /// </param>
        public void ChangeTextureColor(Texture2D texture, Color color)
        {
            Color[] data = new Color[texture.Width * texture.Height];
            texture.GetData(data);

            for (int i = 0; i < data.Length; i++)
                    data[i] = color;

            texture.SetData(data);
        }

        /// <summary>
        /// This is called when you want to exit the screen.
        /// </summary>
        /// <remarks>
        ///<para>AUTHOR: Khaled Salah </para>
        ///</remarks>  
        public override void Remove()
        {
            base.Remove();
        }
    }
}
