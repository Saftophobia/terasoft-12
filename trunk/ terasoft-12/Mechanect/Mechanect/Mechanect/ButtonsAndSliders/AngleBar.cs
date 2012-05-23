using System;
using Mechanect.Common;
using Microsoft.Kinect;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Mechanect.ButtonsAndSliders
{
    /// <summary>
    /// A bar that displays Users' current position and the correct range to stand in
    /// </summary>
    /// <remarks>
    /// <para>AUTHOR: Mohamed AbdelAzim</para>
    /// </remarks>
    class AngleBar
    {

        #region Variables And Fields

        User[] user;
        int minAngle;
        int maxAngle;


        Texture2D curve;
        int curveRadius;
        int curveWidth;

        Texture2D playerIndicator;
        Color[] playerColor;
        Color acceptColor;
        Color rejectColor;

        /// <summary>
        /// Getter for the Users' State
        /// </summary>
        /// <remarks>
        /// <para>Author: Mohamed AbdelAzim</para>
        /// </remarks>
        /// <returns>bool, Returns true if all users are standing with the correct orientation</returns>
        public bool Accepted
        {
            get
            {
                for (int i = 0; i < user.Length; i++)
                    if (Angle(i) >= maxAngle || Angle(i) <= minAngle)
                        return false;
                return true;
            }
        }

        /// <summary>
        /// Getter for the Rule that should be visibile to the user
        /// </summary>
        /// <remarks>
        /// <para>Author: Mohamed AbdelAzim</para>
        /// </remarks>
        /// <returns>string, returns the Rule that should be followed by the user</returns>
        public string Rule
        {
            get
            {
                int avgAngle = (minAngle + maxAngle) / 2;
                if (avgAngle == 0)
                {
                    return "Stand facing the kinect sensor";
                }
                else if (avgAngle > 0)
                {
                    return "Turn to your right at an angle " + avgAngle + "degrees with the kinect sensor.";
                }
                else
                {
                    return "Turn to your left at an angle " + (-1 * avgAngle) + "degrees with the kinect sensor.";
                }
            }
        }

        #endregion

        #region Construct & Load

        /// <summary>
        /// Generates an angle Bar that displays users state regarding his orientation with respect to the correct orientation region
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed AbdelAzim</para>
        /// </remarks>
        /// <param name ="user">Array of users that be tracked by the depth bar</param>
        /// <param name ="minAngle">the minimum angle (to the left) the users can stand with</param>
        /// <param name ="maxAngle">the maximum angle (to the right) the users can stand with</param>
        /// <param name ="curveRadius">the radius of the semicircle represention various angles</param>
        /// <param name ="acceptColor">the color representing the accepted range of standing orientation</param>
        /// <param name ="rejectColor">the color representing the rejected range of standing orientation</param>
        /// <param name ="playerColor">Array of colors representing each user</param>
        public AngleBar(User[] user, int minAngle, int maxAngle, int curveRadius, Color acceptColor, Color rejectColor, Color[] playerColor)
        {
            this.user = user;
            this.minAngle = minAngle;
            this.maxAngle = maxAngle;
            this.curveRadius = curveRadius;
            this.curveWidth = curveRadius / 4;
            this.acceptColor = acceptColor;
            this.rejectColor = rejectColor;
            this.playerColor = playerColor;
        }

        /// <summary>
        /// Loads the textures of the angle bar
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed AbdelAzim</para>
        /// </remarks>
        /// <param name ="graphicsDevice">The graphics device of the screen manager</param>
        public void LoadContent(GraphicsDevice graphicsDevice, ContentManager contentManager)
        {
            curve = CreateCurve(graphicsDevice);
            playerIndicator = contentManager.Load<Texture2D>("ball");
        }

        #region Create Semicircle


        /// <summary>
        /// gets the suitable color that fits in the gradient in the semicircle
        /// </summary>
        /// <remarks>
        /// <para>Author: Mohamed AbdelAzim</para>
        /// </remarks>
        /// <param name="leftAngle"> the start angle of the gradient</param>
        /// <param name="rightAngle"> the end angle of the gradient</param>
        /// <param name="currentAngle"> the pixel's angle</param>
        /// <param name="leftColor"> the color at the start (left side) of the gradient</param>
        /// <param name="rightColor"> the color at the end (right side) of the gradient</param>
        /// <returns>returns the color corresponding to the gradient respect to pixel's position within the angle ranges</returns>
        public Color CurveColor(int leftAngle, int rightAngle, int currentAngle, Color leftColor, Color rightColor)
        {
            int R = (rightColor.R * (currentAngle - leftAngle) + leftColor.R * (rightAngle - currentAngle)) / (rightAngle - leftAngle);
            int G = (rightColor.G * (currentAngle - leftAngle) + leftColor.G * (rightAngle - currentAngle)) / (rightAngle - leftAngle);
            int B = (rightColor.B * (currentAngle - leftAngle) + leftColor.B * (rightAngle - currentAngle)) / (rightAngle - leftAngle);
            return new Color(R, G, B);
        }

        /// <summary>
        /// creates the texture2D representing the angle bar
        /// </summary>
        /// <remarks>
        /// <para>Author: Mohamed AbdelAzim</para>
        /// </remarks>
        /// <returns>returns the semicircle with gradient indicating the accepted ranges for user's angle</returns>
        public Texture2D CreateCurve(GraphicsDevice graphicsDevice)
        {
            int textureWidth = 2 * curveRadius;
            int textureHeight = curveRadius;
            int avgAngle = (minAngle + maxAngle) / 2;
            Texture2D curve = new Texture2D(graphicsDevice, textureWidth, textureHeight);
            Color[] texturePixels = new Color[textureHeight * textureWidth];
            Vector2 pixelLocation = new Vector2();
            double radius, theta;
            for (int pixelIndex = 0; pixelIndex < texturePixels.Length; pixelIndex++)
            {
                pixelLocation.X = (int)(pixelIndex % textureWidth - textureWidth / 2);
                pixelLocation.Y = textureHeight - pixelIndex / textureWidth;
                radius = pixelLocation.Length();
                if (radius <= curveRadius && radius >= curveRadius - curveWidth)
                {
                    if (pixelLocation.X == 0)
                        theta = 0;
                    else
                    {
                        theta = MathHelper.ToDegrees((float)Math.Atan(pixelLocation.Y / pixelLocation.X));
                        if (theta > 0)
                            theta = 90 - theta;
                        else
                            theta = -90 - theta;
                    }
                    if (theta <= minAngle || theta >= maxAngle)
                        texturePixels[pixelIndex] = rejectColor;
                    else if (theta >= (minAngle + avgAngle) / 2 && theta <= (maxAngle + avgAngle) / 2)
                        texturePixels[pixelIndex] = acceptColor;
                    else if (theta < avgAngle)
                        texturePixels[pixelIndex] = CurveColor(minAngle, (minAngle + avgAngle) / 2, (int)theta, rejectColor, acceptColor);
                    else if (theta > avgAngle)
                        texturePixels[pixelIndex] = CurveColor((maxAngle + avgAngle) / 2, maxAngle, (int)theta, acceptColor, rejectColor);
                }
            }
            curve.SetData(texturePixels);
            return curve;
        }


        #endregion

        #endregion

        #region Functions

        /// <summary>
        /// A getter to the command that should be visible to the user
        /// </summary>
        /// <remarks>
        /// <para>Author: Mohamed AbdelAzim</para>
        /// </remarks>
        /// <param name="id">an int representing the ID of the user</param>
        /// <returns>string, returns the command that should be applied by the user to reach the correct orientation.</returns>
        public string Command(int ID)
        {
            if (Angle(ID) == 0)
                return "No player detected";
            if (Angle(ID) < minAngle)
                return "Turn a little to your right";
            if (Angle(ID) > maxAngle)
                return "Turn a little to your left";
            return "OK!";
        }

        /// <summary>
        /// Calculates the Angle of the user specified by the ID
        /// </summary>
        /// <remarks>
        /// <para>Author: Mohamed AbdelAzim</para>
        /// </remarks>
        /// <param name="ID">the index of the User in the users array</param>
        /// <returns>returns the angle the user with the kinect sensor. 0 when he faces the sensor, +ve when he looks right, -ve when he looks left</returns>
        public int Angle(int ID)
        {
            try
            {
                Vector2 rightHip = new Vector2(user[ID].USER.Joints[JointType.HipRight].Position.X, user[ID].USER.Joints[JointType.HipRight].Position.Z);
                Vector2 leftHip = new Vector2(user[ID].USER.Joints[JointType.HipLeft].Position.X, user[ID].USER.Joints[JointType.HipLeft].Position.Z);
                Vector2 fromLeftHipToRightHip = rightHip - leftHip;
                int angle = (int)MathHelper.ToDegrees((float)Math.Atan(fromLeftHipToRightHip.Y / fromLeftHipToRightHip.X));
                return angle;
            }
            catch (NullReferenceException)
            {
                return 0;
            }
            catch (IndexOutOfRangeException)
            {
                return 0;
            }
        }

        #endregion

        #region Draw

        /// <summary>
        /// Draws the angle bar
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed AbdelAzim</para>
        /// </remarks>
        /// <param name ="spriteBatch">The sprite batch of the screen manager</param>
        /// <param name ="position">The potition the bar should be drawn at</param>
        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(curve, position, Color.White);
            for (int i = 0; i < user.Length; i++)
                spriteBatch.Draw(playerIndicator, new Rectangle(curveRadius + (int)position.X, curveRadius + (int)position.Y,
                    curveRadius, curveRadius / 8), null, playerColor[i], (float)((Angle(i) - 90) * Math.PI / 180),
                    new Vector2(0, playerIndicator.Height / 2), SpriteEffects.None, 0f);

        }

        #endregion
    }
}
