using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Mechanect.Common;
using Microsoft.Kinect;

namespace Mechanect.Screens
{
    public class UserAvatarScreen : GameScreen
    {
        GraphicsDevice graphics;
        SpriteBatch spriteBatch;
        int screenWidth;
        int screenHeight;
        ContentManager content;
        Texture2D[] avatar;
        Vector2[] avatarPosition;
        SpriteFont font;
        User[] users;
        MKinect kinect;
        String[] command;
        const int minDepth = 120;
        const int maxDepth = 350;
        int[] depth;
        public UserAvatarScreen(User user, MKinect kinect)
        {
            this.users = new User[1];
            this.kinect = kinect;
            this.users[0] = user;
            Initialize();
        }
        public UserAvatarScreen(User user, User user2, MKinect kinect)
        {
            this.users = new User[2];
            this.users[0] = user;
            this.users[1] = user2;
            this.kinect = kinect;
            Initialize();
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
            font = content.Load<SpriteFont>("spriteFont1");
            for (int i = 0; i < avatar.Length; i++)
            {
                avatar[i] = content.Load<Texture2D>(@"Textures/avatar-white");
                avatarPosition[i] = new Vector2(screenWidth / (i + 1), screenHeight);
            }
        }

        public void Initialize()
        {
            this.depth = new int[users.Length];
            this.command = new String[users.Length];
            avatar = new Texture2D[users.Length];
            avatarPosition = new Vector2[users.Length];
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
            depth[ID] = GenerateDepth(ID);
            if (depth[ID] == 0)
            {
                avatar[ID] = content.Load<Texture2D>(@"Textures/avatar-dead");
                command[ID] = "No player detected" + depth[ID];
            }
            else
            {
                if (depth[ID] < minDepth)
                    avatar[ID] = content.Load<Texture2D>(@"Textures/avatar-red");
                else if (depth[ID] > maxDepth)
                    avatar[ID] = content.Load<Texture2D>(@"Textures/avatar-white");
                else if (depth[ID] < maxDepth)
                    avatar[ID] = content.Load<Texture2D>(@"Textures/avatar-green");
            }
            command[ID] = "You're standing at" + depth[ID];
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
        public void UpdateAvatar(Texture2D texture, User user)
        {
            int userindex = getUserindex(user);
            if (user.USER != null)
            {
                if (depth[userindex] < maxDepth / 4)
                    ChangeTextureColor(texture, "Yellow");
                else if (depth[userindex] < maxDepth / 2)
                    ChangeTextureColor(texture, "Green");
                else if (depth[userindex] < maxDepth)
                    ChangeTextureColor(texture, "Blue");
                else if (depth[userindex] > maxDepth)
                    ChangeTextureColor(texture, "Red");
            }
            else ChangeTextureColor(texture, "Green");
        }

        public int getUserindex(User user)
        {
            int userindex = 0;
            for (int i = 0; i < users.Length; i++)
            {
                if (users[i].Equals(user))
                    userindex = i;
            }
            return userindex;
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
            try
            {
                return (int)(100 * users[index].USER.Joints[JointType.HipCenter].Position.Z);
            }
            catch (NullReferenceException)
            {
                return 0;
            }
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

            if (users.Length == 1)
            {
                users[0].USER = kinect.requestSkeleton();

            }
            else if (users.Length == 2)
            {
                users[0].USER = kinect.requestSkeleton();
                users[1].USER = kinect.request2ndSkeleton();
            }
            for (int i = 0; i < users.Length; i++)
            {
                UpdateUser(i);
                // UpdateAvatar(avatar[i], users[i]);
            }
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
            for (int i = 0; i < avatar.Length; i++)
            {
                spriteBatch.Draw(avatar[i], avatarPosition[i], null, Color.White, 0,
                    new Vector2(avatar[i].Width, avatar[i].Height), 1f, SpriteEffects.None, 0);
            }
            for (int i = 0; i < users.Length; i++)
            {
                ScreenManager.SpriteBatch.DrawString(font, "Player " + i + " : " + command[i],
                    new Vector2(100, 320 + 100 * i), Color.OrangeRed);
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
        public void ChangeTextureColor(Texture2D texture, string color)
        {
            Color[] data = new Color[texture.Width * texture.Height];
            texture.GetData(data);
            switch (color)
            {
                case "Red":
                    for (int i = 0; i < data.Length; i++)
                        data[i].R = 255; break;
                case "Yellow":
                    for (int i = 0; i < data.Length; i++)
                    {
                        data[i].G = 255;
                        data[i].R = 255;
                    } break;
                case "Green":
                    for (int i = 0; i < data.Length; i++)
                        data[i].G = 255; break;
                case "Blue":
                    for (int i = 0; i < data.Length; i++)
                        data[i].B = 255; break;
                case "Transparent":
                    for (int i = 0; i < data.Length; i++)
                        data[i].A = 0; break;
                default: break;
            }
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
s