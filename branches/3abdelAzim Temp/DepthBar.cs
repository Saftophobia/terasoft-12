 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Mechanect.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Kinect;

namespace Mechanect.Screens
{
    class DepthBar
    {
        User user;
        int minDepth;
        int maxDepth;
        String rule;
        Texture2D depthBar;
        int width;
        int height;
        Color accept;
        Color reject;
        Color userColor;
        int depth;

        public string Rule
        {
            get
            {
                return rule;
            }
        }

        public DepthBar(User user, int minDepth, int maxDepth, int width, int height, Color accept, Color reject, Color userColor, GraphicsDevice graphicsDevice)
        {
            this.user = user;
            this.minDepth = minDepth;
            this.maxDepth = maxDepth;
            this.width = width;
            this.height = height;
            this.accept = accept;
            this.reject = reject;
            this.userColor = userColor;
            depthBar = new Texture2D(graphicsDevice, width, height);
            rule = "Stand at a distance of " + ((float)(minDepth + maxDepth) / 200) + " meters from the kinect sensor.";
        }


        public bool Accepted()
        {
            return depth <= maxDepth && depth >= minDepth;
        }

        public string Command()
        {

            if (depth < minDepth)
            {
                return "Move backwards away from the kinect sensor";
            }
            if (depth > maxDepth)
            {
                return "Move forward towards the kinect sensor";
            }
            return "You Are Standing at a correct distance from the kinect sensor";
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
        public int Depth()
        {
            try
            {
                return (int)(100 * user.USER.Joints[JointType.HipCenter].Position.Z);
            }
            catch (NullReferenceException)
            {
                return 0;
            }
        }

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
        public void Update()
        {
            Color[] data = new Color[height];
            depth = Depth();
            int avgDepth = (minDepth + maxDepth) / 2;
            for (int i = 0; i < height; i++)
            {
                if (2 * i + 50 <= minDepth || 2 * i + 50 >= maxDepth)
                    data[i] = reject;
                else if (2 * i + 50 > (avgDepth + minDepth) / 2 && 2 * i + 50 < (avgDepth + maxDepth) / 2)
                    data[i] = accept;
                else if (2 * i + 50 < avgDepth)
                    data[i] = GradientColor(minDepth, (avgDepth + minDepth) / 2, 2 * i + 50, reject, accept);
                else if (2 * i + 50 > avgDepth)
                    data[i] = GradientColor((avgDepth + maxDepth) / 2, maxDepth, 2 * i + 50, accept, reject);
                if (2 * i + 50 <= depth + 5 && 2 * i + 50 >= depth - 5)
                    data[i] = userColor;
            }
            Color[] finalData = new Color[height * width];
            for (int j = 0; j < finalData.Length; j++)
            {
                finalData[j] = data[j / width];
            }
            depthBar.SetData(finalData);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(depthBar, position, Color.White);
        }


        internal void Draw(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }
    }
}
