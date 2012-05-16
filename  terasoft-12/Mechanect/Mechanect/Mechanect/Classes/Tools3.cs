using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using Microsoft.Xna.Framework;
using Mechanect.Common;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Mechanect.Classes
{
    class Tools3
    {
      

        /// <summary>
        /// calculates the friction vector that will cause the velocity to stop without changing the velocity direction
        /// </summary>
        /// <param name="velocity">the velocity vector</param>
        /// <param name="frictionValue">the value of the friction</param>
        /// <remarks>Auther : Bishoy Bassem</remarks>
        public static Vector3 calculateFriction(Vector3 velocity, float frictionValue)
        {
            return velocity * frictionValue / velocity.Length() * -1;
        }




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
        /// <param name="c">content managaer to load pictures</param>
        /// <param name="p">position of the button</param>
        /// <param name="sw">screen width</param>
        /// <param name="sh">screen height</param>
        /// <param name="u">instance of user<param>
        /// <returns>returns OK button</returns>
        public static Button OKButton(ContentManager c, Vector2 p, int sw, int sh, User u)
        {
            return new Button(c.Load<GifAnimation.GifAnimation>("Textures/Buttons/ok-s"),
           c.Load<GifAnimation.GifAnimation>("Textures/Buttons/ok-m"), p, sw, sh,
           c.Load<Texture2D>("Textures/Buttons/hand"), u);
        }


        ///<remarks>
        ///<para>
        ///Author: HegazY
        ///</para>
        ///</remarks>
        /// <summary>
        /// Used to get the customized button with MainMenu
        /// </summary>
        /// <param name="c">content managaer to load pictures</param>
        /// <param name="p">position of the button</param>
        /// <param name="sw">screen width</param>
        /// <param name="sh">screen height</param>
        /// <param name="u">instance of user<param>
        /// <returns>returns MainMenu button</returns>
        public static Button MainMenuButton(ContentManager c, Vector2 p, int sw, int sh, User u)
        {
            return new Button(c.Load<GifAnimation.GifAnimation>("Textures/Buttons/menu-s"),
           c.Load<GifAnimation.GifAnimation>("Textures/Buttons/menu-m"), p, sw, sh,
           c.Load<Texture2D>("Textures/Buttons/hand"), u);
        }


        ///<remarks>
        ///<para>
        ///Author: HegazY
        ///</para>
        ///</remarks>
        /// <summary>
        /// Used to get the customized button with NewGame
        /// </summary>
        /// <param name="c">content managaer to load pictures</param>
        /// <param name="p">position of the button</param>
        /// <param name="sw">screen width</param>
        /// <param name="sh">screen height</param>
        /// <param name="u">instance of user<param>
        /// <returns>returns NewGame button</returns>
        public static Button NewGameButton(ContentManager c, Vector2 p, int sw, int sh, User u)
        {
            return new Button(c.Load<GifAnimation.GifAnimation>("Textures/Buttons/newgame-s"),
           c.Load<GifAnimation.GifAnimation>("Textures/Buttons/newgame-m"), p, sw, sh,
           c.Load<Texture2D>("Textures/Buttons/hand"), u);
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
        public static void DislayIsWin(SpriteBatch spriteBatch, ContentManager content, Vector2 position, bool status)
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
