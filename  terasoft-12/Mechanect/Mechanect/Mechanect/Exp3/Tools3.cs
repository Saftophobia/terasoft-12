using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using Microsoft.Xna.Framework;
using Mechanect.Common;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ButtonsAndSliders;

namespace Mechanect.Exp3
{
    public class Tools3
    {
      
        ///<remarks>
        ///<para>
        ///Author: Cena
        ///</para>
        ///</remarks>
        /// <summary>
        /// calculates the final velocity using Vf = Vi + a*t
        /// </summary>
        /// <param name="acceleration">acceleration of the body</param>
        /// <param name="velocityInitial">initial veloctiy of the body</param>
        /// <param name="totalTime">the total time the body moved</param>
        /// <returns></returns>

        public static Vector3 GetVelocity(Vector3 position, double totalTime)
        {
            return new Vector3(position.X / (float)totalTime, 0, position.Z / (float)totalTime);
        }




        ///<remarks>
        ///<para>
        ///Author: HegazY
        ///</para>
        ///</remarks>
        /// <summary>
        /// Used to get the customized button with OKButton
        /// </summary>
        /// <param name="contentManager">content managaer to load pictures</param>
        /// <param name="position">position of the button</param>
        /// <param name="screenWidth">screen width</param>
        /// <param name="screenHeight">screen height</param>
        /// <param name="user">instance of user<param>
        /// <returns>returns NewGame button</returns>
        public static Button OKButton(ContentManager contentManager, Vector2 position,
            int screenWidth, int screenHeight, User user)
        {
            return new Button(contentManager.Load<GifAnimation.GifAnimation>("Textures/Buttons/ok-s"),
                contentManager.Load<GifAnimation.GifAnimation>("Textures/Buttons/ok-m"), position, 
                screenWidth, screenHeight, contentManager.Load<Texture2D>("Textures/Buttons/hand"), user);
        }


        ///<remarks>
        ///<para>
        ///Author: HegazY
        ///</para>
        ///</remarks>
        /// <summary>
        /// Used to get the customized button with MainMenu
        /// </summary>
        /// <param name="contentManager">content managaer to load pictures</param>
        /// <param name="position">position of the button</param>
        /// <param name="screenWidth">screen width</param>
        /// <param name="screenHeight">screen height</param>
        /// <param name="user">instance of user<param>
        /// <returns>returns NewGame button</returns>
        public static Button MainMenuButton(ContentManager contentManager, Vector2 position,
            int screenWidth, int screenHeight, User user)
        {
            return new Button(contentManager.Load<GifAnimation.GifAnimation>("Textures/Buttons/menu-s"),
                contentManager.Load<GifAnimation.GifAnimation>("Textures/Buttons/menu-m"), position, 
                screenWidth, screenHeight, contentManager.Load<Texture2D>("Textures/Buttons/hand"), user);
        }


        ///<remarks>
        ///<para>
        ///Author: HegazY
        ///</para>
        ///</remarks>
        /// <summary>
        /// Used to get the customized button with NewGame
        /// </summary>
        /// <param name="contentManager">content managaer to load pictures</param>
        /// <param name="position">position of the button</param>
        /// <param name="screenWidth">screen width</param>
        /// <param name="screenHeight">screen height</param>
        /// <param name="user">instance of user<param>
        /// <returns>returns NewGame button</returns>
        public static Button NewGameButton(ContentManager contentManager, Vector2 position, 
            int screenWidth, int screenHeight, User user)
        {
            return new Button(contentManager.Load<GifAnimation.GifAnimation>("Textures/Buttons/newgame-s"),
                contentManager.Load<GifAnimation.GifAnimation>("Textures/Buttons/newgame-m"), position, 
                screenWidth, screenHeight, contentManager.Load<Texture2D>("Textures/Buttons/hand"), user);
        }


        ///<remarks>
        ///<para>
        ///Author: HegazY
        ///</para>
        ///</remarks>
        /// <summary>
        /// displaying the wining word on the screen.
        /// </summary>
        /// <param name="spriteBatch">used to draw images on the screen</param>
        /// <param name="content">used to load the images</param>
        /// <param name="position">the desired position</param>
        /// <param name="status">true: if the user won, false: if the user lost</param>
        [System.Obsolete("use method DisplayIsWin instead, typoo", false)]
        public static void DislayIsWin(SpriteBatch spriteBatch, ContentManager content, Vector2 position, bool status)
        {
            /*
            spriteBatch.Begin();
            if (status)
            {
                Texture2D winningPicture = content.Load<Texture2D>("Textures/WorL/winner");
                spriteBatch.Draw(winningPicture, position, Color.White);
            }
            else
            {
                Texture2D losingPicture = content.Load<Texture2D>("Textures/WorL/looser");
                spriteBatch.Draw(losingPicture, position, Color.White);
            }
            spriteBatch.End();
             */
        }

        public static void DisplayIsWin(SpriteBatch spriteBatch, ContentManager content, Vector2 position, bool status)
        {
            spriteBatch.Begin();
            if (status)
            {
                Texture2D winningPicture = content.Load<Texture2D>("Textures/WorL/winner");
                spriteBatch.Draw(winningPicture, position, Color.White);
            }
            else
            {
                Texture2D losingPicture = content.Load<Texture2D>("Textures/WorL/looser");
                spriteBatch.Draw(losingPicture, position, Color.White);
            }
            spriteBatch.End();
        }

        /// <summary>
        /// Generates a random float value between two float numbers.
        /// </summary>
        /// <remarks>
        ///<para>AUTHOR: Khaled Salah </para>
        ///</remarks>
        /// <param name="min">
        /// The minimum value. 
        /// </param>
        /// /// <param name="max">
        /// The maximum value.
        /// </param>
        /// <returns>
        /// Float number which is the generated random value.
        /// </returns>

        public static float GenerateRandomValue(float min, float max)
        {
            if (max > min)
            {
                var random = new Random();
                var value = ((float)(random.NextDouble() * (max - min))) + min;
                return value;
            }
            else throw new ArgumentException("max value has to be greater than min value");
        }
    }
}
