using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Kinect;

namespace Mechanect.Common
{

    /// <summary>
    /// This class represents an avatar which tracks a player's distance from the kinect device and changes its color according to this distance, its implemented to work for two players aswell since the kinect device can only track two players maximum at the same time
    /// </summary>
    /// <remarks>
    /// <para>AUTHOR: Khaled Salah </para>
    /// </remarks>
    public class UserAvatar
    {
        private GraphicsDevice graphics;
        private SpriteBatch spriteBatch;
        private int screenWidth;
        private int screenHeight;
        private ContentManager content;
        private Texture2D[] avatar;
        private Vector2[] avatarPosition;
        private SpriteFont font;
        private User[] users;
        private MKinect kinect;
        private String[] command;
        const int minDepth = 120;
        const int maxDepth = 350;
        private int[] depth;
        private Texture2D[] allAvatars;

        /// <summary>
        /// Class constructor for 1 player mode.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Khaled Salah </para>
        /// </remarks>
        public UserAvatar(User user, ContentManager content, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
            this.users = new User[1];
            this.users[0] = user;
            this.graphics = graphicsDevice;
            screenWidth = graphics.Viewport.Width;
            screenHeight = graphics.Viewport.Height;
            this.spriteBatch = spriteBatch;
            this.content = content;
            Initialize(); 
        }

        /// <summary>
        /// Class constructor for 2 player mode.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Khaled Salah </para>
        /// </remarks>
        public UserAvatar(User user, User user2, ContentManager content,GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
            this.users = new User[2];
            this.users[0] = user;
            this.users[1] = user2;
            this.graphics = graphicsDevice;
            screenWidth = graphics.Viewport.Width;
            screenHeight = graphics.Viewport.Height;
            this.spriteBatch = spriteBatch;
            this.content = content;
            Initialize(); 
        }
        /// <summary>
        /// Initializes the kinect sensor and the arrays that keep track of user's information like avatars, avatar positions, depth and notification messages.
        /// all of your content.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Khaled Salah </para>
        /// </remarks>
        public void Initialize()
        {
            this.depth = new int[users.Length];
            this.command = new String[users.Length];
            avatar = new Texture2D[users.Length];
            avatarPosition = new Vector2[users.Length];
            kinect = users[0].Kinect;
            allAvatars = new Texture2D[4];
        }
        /// <summary>
        /// LoadContent will be called only once before drawing and it's the place to load
        /// all of your content.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Khaled Salah </para>
        /// </remarks>
        public void LoadContent()
        {
            font = content.Load<SpriteFont>("spriteFont1");
            allAvatars[0] = content.Load<Texture2D>(@"Textures/avatar-dead");
            allAvatars[1] = content.Load<Texture2D>(@"Textures/avatar-white");
            allAvatars[2] = content.Load<Texture2D>(@"Textures/avatar-green");
            allAvatars[3] = content.Load<Texture2D>(@"Textures/avatar-red");
            for (int i = 0; i < avatar.Length; i++)
            {
                avatar[i] = allAvatars[0];
                command[i] = "";
            }
            avatarPosition[0] = new Vector2(screenWidth+25, screenHeight / 2.6f);
            if (avatarPosition.Length == 2)
                avatarPosition[1] = new Vector2(screenWidth / 8, screenHeight / 2.6f);
        }

        /// <summary>
        /// This is called when the game screen should draw itself.
        /// </summary>
        /// <remarks>
        ///<para>AUTHOR: Khaled Salah </para>
        ///</remarks>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>    
        public void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            for (int i = 0; i < users.Length; i++)
            {
                spriteBatch.Draw(avatar[i], avatarPosition[i], null, Color.White, 0,
                    new Vector2(avatar[i].Width, avatar[i].Height), 1f, SpriteEffects.None, 0);
                //spriteBatch.DrawString(font, command[i],new Vector2(100, 520 + 100 * i), Color.OrangeRed);
            }
            spriteBatch.End();
        }


        /// <summary>
        /// Allows the game screen to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <remarks>
        ///<para>AUTHOR: Khaled Salah </para>
        ///</remarks>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(GameTime gameTime)
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
                if (users[i].USER!=null)
                UpdateUser(i);
            }
        }
        
        /// <summary>
        /// Takes the user's index in the users array and calculates the player's distance from the kinect device, and updates the notification message that should be printed if the user is not detected or too far away.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Khaled Salah </para>
        /// </remarks>
        /// <param name="ID">
        /// The user's index in users array.
        /// </param>
        public void UpdateUser(int ID)
        {
            depth[ID] = GenerateDepth(ID);
            if (depth[ID] == 0)
            {
                avatar[ID] = allAvatars[0];
                command[ID] = "Player " + (ID+1) + " : No player detected";
            }
            else
            {
                if (depth[ID] < minDepth)
                    avatar[ID] = allAvatars[3];
                else if (depth[ID] > maxDepth)
                    avatar[ID] = allAvatars[1];
                else if (depth[ID] < maxDepth)
                    avatar[ID] = allAvatars[2];
                command[ID] = "";
            }
        }

        /// <summary>
        /// Takes the user's index in the array and gets his distance from the kinect device.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Khaled Salah </para>
        /// </remarks>
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
        /// Takes a user as a parameter and returns his index in the array of users.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Khaled Salah </para>
        /// </remarks>
        /// <param name="user">
        /// The user.
        /// </param>
        /// <returns>
        /// Int number which is the index of this user in the array.
        /// </returns>
        public static int getUserindex(User user, User[] users)
        {
            int userindex = 0;
            for (int i = 0; i < users.Length; i++)
            {
                if (users[i].Equals(user))
                    userindex = i;
            }
            return userindex;
        }

        
        #region unused methods used in old design
        /// <summary>
        /// Takes a 2D texture as a parameter and colors it according to user's distance from the kinect device.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Khaled Salah </para>
        /// </remarks>
        /// <param name="texture">
        /// The 2D texture that should be colored.
        /// </param>
        public void UpdateAvatar(Texture2D texture, User user)
        {
            int userindex = getUserindex(user,users);
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
        /// <summary>
        /// Takes as parameters a 2D texture and a color and changes the texture's color to the specified color.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Khaled Salah </para>
        /// </remarks>
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
        #endregion
    }
}
