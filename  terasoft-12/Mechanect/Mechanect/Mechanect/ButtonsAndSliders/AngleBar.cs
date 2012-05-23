using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mechanect.Common;
using Microsoft.Xna.Framework.Content;
using Microsoft.Kinect;

namespace Mechanect.ButtonsAndSliders {
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

        public AngleBar(User[] users, int minAngle, int maxAngle, int radius, Color accept, Color reject, Color[] playerColors)
        {
            this.user = users;
            this.minAngle = minAngle;
            this.maxAngle = maxAngle;
            this.curveRadius = radius;
            this.curveWidth = radius / 4;
            this.acceptColor = accept;
            this.rejectColor = reject;
            this.playerColor = playerColors;
        }

        public void LoadContent(GraphicsDevice graphicsDevice, ContentManager contentManager)
        {
            curve = CreateCurve(graphicsDevice);
            playerIndicator = contentManager.Load<Texture2D>("ball");
        }

        #region Create Semicircle
        

        /// <summary>
        /// gets the suitable color that fits in the gradient in the semicircle
        /// </summary>
        ///<remarks>
        ///<para>
        ///Author: Mohamed AbdelAzim
        ///</para>
        ///</remarks>
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
        ///<remarks>
        ///<para>
        ///Author: Mohamed AbdelAzim
        ///</para>
        ///</remarks>
        ///<returns>returns the semicircle with gradient indicating the accepted ranges for user's angle</returns>
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
        /// measures the orientation of the user with respect to the kinect sensor
        /// <example>a player standing facing the kinect sensor will have zero angle, </example>
        /// <example>a player turned to his right with respect to the kinect sensor will a positive angle, </example>
        /// <example>a player turned to his left with respect to the kinect sensor will a negative angle. </example>
        /// </summary>
        ///<remarks>
        ///<para>
        ///Author: Mohamed AbdelAzim
        ///</para>
        ///</remarks>
        /// <param name="ID"> the index of the User in the users array</param>
        /// <returns>returns the angle users[ID] makes with the kinect sensor. </returns>
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
