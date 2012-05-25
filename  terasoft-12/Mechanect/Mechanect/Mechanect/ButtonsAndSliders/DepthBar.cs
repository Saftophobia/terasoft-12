using System;
using Mechanect.Common;
using Microsoft.Kinect;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mechanect.ButtonsAndSliders
{
    /// <summary>
    /// A bar that displays Users' current position and the correct range to stand in
    /// </summary>
    /// <remarks>
    /// <para>AUTHOR: Mohamed AbdelAzim</para>
    /// </remarks>
    class DepthBar
    {

        #region Variables And Fields

        private User[] user;
        private int minDepth;
        private int maxDepth;
        private Color acceptColor;
        private Color rejectColor;
        private Color[] playerColor;

        private Texture2D bar;
        private int barWidth;
        private int barHeight;

        private Texture2D playerIndicator;

        /// <summary>
        /// Getter for the Users' State
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed AbdelAzim</para>
        /// </remarks>
        /// <returns>bool, Returns true if all users are standing in the correct region</returns>
        public bool Accepted
        {
            get
            {
                for (int i = 0; i < user.Length; i++)
                    if (Depth(i) >= maxDepth || Depth(i) <= minDepth)
                        return false;
                return true;
            }
        }

        /// <summary>
        /// Getter for the Rule that should be visibile to the user
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed AbdelAzim</para>
        /// </remarks>
        /// <returns>string, returns the Rule that should be followed by the user</returns>
        public string Rule
        {
            get
            {
                return "Stand at a distance of " + ((float)(minDepth + maxDepth) / 200) + " meters from the kinect sensor.";
            }
        }

        #endregion

        #region Construct & Load

        /// <summary>
        /// Generates a Depth Bar that displays users state regarding his position with respect to the correct standing region
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed AbdelAzim</para>
        /// </remarks>
        /// <param name ="user">Array of users that be tracked by the depth bar</param>
        /// <param name ="minDepth">the minimum depth the users should stand at</param>
        /// <param name ="maxDepth">the maximum depth the users should stand at</param>
        /// <param name ="barWidth">the width of the depth bar</param>
        /// <param name ="barHeight">the height of the depth bar</param>
        /// <param name ="acceptColor">the color representing the accepted standing region</param>
        /// <param name ="rejectColor">the color represention the rejected standing region</param>
        /// <param name ="playerColor">Array of colors representing each user</param>
        public DepthBar(User[] user, int minDepth, int maxDepth, int barWidth, int barHeight, Color acceptColor, Color rejectColor, Color[] playerColor)
        {
            this.user = user;
            this.minDepth = minDepth;
            this.maxDepth = maxDepth;
            this.barWidth = barWidth;
            this.barHeight = barHeight;
            this.acceptColor = acceptColor;
            this.rejectColor = rejectColor;
            this.playerColor = playerColor;
        }

        /// <summary>
        /// Loads the textures of the depth bar
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed AbdelAzim</para>
        /// </remarks>
        /// <param name ="graphicsDevice">The graphics device of the screen manager</param>
        public void LoadContent(GraphicsDevice graphicsDevice)
        {
            bar = CreateBar(graphicsDevice);
            playerIndicator = new Texture2D(graphicsDevice, barWidth, 5);
            Color[] fillColor = new Color[barWidth * 5];
            for (int j = 0; j < fillColor.Length; j++)
                fillColor[j] = Color.White;
            playerIndicator.SetData<Color>(fillColor);
        }

        #region CreateGradient

        /// <summary>
        /// The method gets the suitable color to fit in the gradient.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed AbdelAzim</para>
        /// </remarks>
        /// <param name="start">the start position of the gradient</param>
        /// <param name="end">the end position of the gradient</param>
        /// <param name="currentPosition">the pixel's position</param>
        /// <param name="startColor">the color at the top of the gradient</param>
        /// <param name="endColor">the color at the bottom of the gradient</param>
        /// <returns>returns a color according to the location with respect to the start and end points of the gradient.</returns>
        private Color GradientColor(int start, int end, int currentPosition, Color startColor, Color endColor)
        {
            int R = (endColor.R * (currentPosition - start) + startColor.R * (end - currentPosition)) / (end - start);
            int G = (endColor.G * (currentPosition - start) + startColor.G * (end - currentPosition)) / (end - start);
            int B = (endColor.B * (currentPosition - start) + startColor.B * (end - currentPosition)) / (end - start);
            return new Color(R, G, B);
        }

        /// <summary>
        /// Creates the bar texture, with a gradient representing the accepted and rejected regions
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed AbdelAzim</para>
        /// </remarks>
        /// <param name ="graphicsDevice">The graphics device of the screen manager</param>
        private Texture2D CreateBar(GraphicsDevice graphicsDevice)
        {
            Texture2D gradient = new Texture2D(graphicsDevice, barWidth, barHeight);
            Color[] data = new Color[barHeight];
            int avgDepth = (minDepth + maxDepth) / 2;
            for (int i = 0; i < barHeight; i++)
            {
                if ((400 / barHeight) * i + 50 <= minDepth || (400 / barHeight) * i + 50 >= maxDepth)
                    data[i] = rejectColor;
                else if ((400 / barHeight) * i + 50 > (avgDepth + minDepth) / 2 && (400 / barHeight) * i + 50 < (avgDepth + maxDepth) / 2)
                    data[i] = acceptColor;
                else if ((400 / barHeight) * i + 50 < avgDepth)
                    data[i] = GradientColor(minDepth, (avgDepth + minDepth) / 2, (400 / barHeight) * i + 50, rejectColor, acceptColor);
                else if ((400 / barHeight) * i + 50 > avgDepth)
                    data[i] = GradientColor((avgDepth + maxDepth) / 2, maxDepth, (400 / barHeight) * i + 50, acceptColor, rejectColor);
            }
            Color[] finalData = new Color[barHeight * barWidth];
            for (int j = 0; j < finalData.Length; j++)
            {
                finalData[j] = data[j / barWidth];
            }
            gradient.SetData(finalData);
            return gradient;
        }
        #endregion
        
        #endregion

        #region Functions

        /// <summary>
        /// Getter for the User's State
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed AbdelAzim</para>
        /// </remarks>
        /// <param name="id">an int representing the ID of the user</param>
        /// <returns>bool, Returns true if the user is standing in the correct region</returns>
        public bool UserAccepted(int ID)
        {
            return Depth(ID) < maxDepth && Depth(ID) > minDepth;
        }

        /// <summary>
        /// A getter to the command that should be visible to the user
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed AbdelAzim</para>
        /// </remarks>
        /// <param name="id">an int representing the ID of the user</param>
        /// <returns>string, returns the command that should be applied by the user to reach the correct position.</returns>
        public string Command(int ID)
        {
            if (Depth(ID) == 0)
                return "Not detected";
            if (Depth(ID) < minDepth)
                return "Move backwards";
            if (Depth(ID) > maxDepth)
                return "Move forward";
            return "OK!";
        }

        /// <summary>
        /// Calculates the Depth of the user specified by the ID
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed AbdelAzim</para>
        /// </remarks>
        /// <param name="ID">the index of the User in the users array</param>
        /// <returns>returns the distance of user from the kinect sensor</returns>
        public int Depth(int ID)
        {
            try
            {
                return (int)(100 * user[ID].USER.Joints[JointType.HipCenter].Position.Z);
            }
            catch (NullReferenceException)
            {
                return 0;
            }
        }

        #endregion

        #region Draw

        /// <summary>
        /// Draws the depth bar
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed AbdelAzim</para>
        /// </remarks>
        /// <param name ="spriteBatch">The sprite batch of the screen manager</param>
        /// <param name ="position">The potition the bar should be drawn at</param>
        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(bar, position, Color.White);
            for (int i = 0; i < user.Length; i++)
                if (Depth(i) > 50)
                    spriteBatch.Draw(playerIndicator, position + new Vector2(0, (Depth(i) - 50) * barHeight / 400), playerColor[i]);
        }

        #endregion
    }
}
