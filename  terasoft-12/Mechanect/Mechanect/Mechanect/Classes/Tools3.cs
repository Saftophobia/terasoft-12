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
    }
}
