using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Kinect;

namespace Mechanect.Common
{
    class DepthBar
    {

        #region Variables And Fields

        User[] users;
        int minDepth;
        int maxDepth;
        int width;
        int height;
        Color accept;
        Color reject;
        Color[] playerColors;

        Texture2D gradient;
        Texture2D playerIndicator; 
        
        public bool Accepted
        {
            get
            {
                for (int i = 0; i < users.Length; i++)
                    if (Depth(i) >= maxDepth || Depth(i) <= minDepth)
                        return false;
                return true;
            }
        }

        public string Rule
        {
            get
            {
                return "Stand at a distance of " + ((float)(minDepth + maxDepth) / 200) + " meters from the kinect sensor.";
            }
        }

        #endregion

        #region LoadTextures

        public DepthBar(User[] users, int minDepth, int maxDepth, int width, int height, Color accept, Color reject, Color[] playerColors)
        {
            this.users = users;
            this.minDepth = minDepth;
            this.maxDepth = maxDepth;
            this.width = width;
            this.height = height;
            this.accept = accept;
            this.reject = reject;
            this.playerColors = playerColors;
        }


        public void LoadContent(GraphicsDevice graphicsDevice)
        {
            gradient = CreateGradient(graphicsDevice);
            playerIndicator = new Texture2D(graphicsDevice, width, 5);
            Color[] fillColor = new Color[width * 5];
            for (int j = 0; j < fillColor.Length; j++)
                fillColor[j] = Color.White;
            playerIndicator.SetData<Color>(fillColor);
        }

        #region CreateGradient
        /// <summary>
        /// The method gets the suitable color to fit in the gradient.
        /// </summary>
        ///<remarks>
        ///<para>
        ///Author: Mohamed AbdelAzim
        ///</para>
        ///</remarks>
        /// <param name="start"> the start position of the gradient</param>
        /// <param name="end"> the end position of the gradient</param>
        /// <param name="index"> the pixel's position</param>
        /// <param name="top"> the color at the top of the gradient</param>
        /// <param name="bot"> the color at the bottom of the gradient</param>
        /// <returns>returns a color according to the location with respect to the start and end points of the gradient. </returns>
        public Color GradientColor(int start, int end, int index, Color top, Color bot)
        {
            int R = (bot.R * (index - start) + top.R * (end - index)) / (end - start);
            int G = (bot.G * (index - start) + top.G * (end - index)) / (end - start);
            int B = (bot.B * (index - start) + top.B * (end - index)) / (end - start);
            return new Color(R, G, B);
        }

        /// <summary>
        /// updates the gradient in the Depth bar to represent the players
        /// </summary>
        ///<remarks>
        ///<para>
        ///Author: Mohamed AbdelAzim
        ///</para>
        ///</remarks>
        public Texture2D CreateGradient(GraphicsDevice graphicsDevice)
        {
            Texture2D gradient = new Texture2D(graphicsDevice, width, height);
            Color[] data = new Color[height];
            int avgDepth = (minDepth + maxDepth) / 2;
            for (int i = 0; i < height; i++)
            {
                if ((400 / height) * i + 50 <= minDepth || (400 / height) * i + 50 >= maxDepth)
                    data[i] = reject;
                else if ((400 / height) * i + 50 > (avgDepth + minDepth) / 2 && (400 / height) * i + 50 < (avgDepth + maxDepth) / 2)
                    data[i] = accept;
                else if ((400 / height) * i + 50 < avgDepth)
                    data[i] = GradientColor(minDepth, (avgDepth + minDepth) / 2, (400 / height) * i + 50, reject, accept);
                else if ((400 / height) * i + 50 > avgDepth)
                    data[i] = GradientColor((avgDepth + maxDepth) / 2, maxDepth, (400 / height) * i + 50, accept, reject);
            }
            Color[] finalData = new Color[height * width];
            for (int j = 0; j < finalData.Length; j++)
            {
                finalData[j] = data[j / width];
            }
            gradient.SetData(finalData);
            return gradient;
        }
        #endregion
        
        #endregion

        #region Functions

        public string Command(int ID)
        {
            if (Depth(ID) == 0)
                return "No player detected";
            if (Depth(ID) < minDepth)
                return "Move backwards away from the kinect sensor";
            if (Depth(ID) > maxDepth)
                return "Move forward towards the kinect sensor";
            return "OK!";
        }

        ///<remarks>
        ///<para>
        ///Author: Mohamed AbdelAzim
        ///</para>
        ///</remarks>
        /// <summary>
        /// <returns>returns the distance of users[ID] from the kinect sensor</returns>
        /// </summary>
        /// <param name="ID"> the index of the User in the users array</param>
        public int Depth(int ID)
        {
            try
            {
                return (int)(100 * users[ID].USER.Joints[JointType.HipCenter].Position.Z);
            }
            catch (NullReferenceException)
            {
                return 0;
            }
        }

        #endregion

        #region Draw

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(gradient, position, Color.White);
            for (int i = 0; i < users.Length; i++)
            {
                spriteBatch.Draw(playerIndicator, position + new Vector2(0, Depth(i)), playerColors[i]);
            }

        }

        #endregion
    }
}
