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


        /// <summary>
        /// Creates a pre-customized OKButton.
        /// </summary>
        /// <remarks>
        /// <para>Author: HegazY</para>
        /// </remarks>
        /// <param name="contentManager">Content managaer to load the pictures.</param>
        /// <param name="position">The Position of the button.</param>
        /// <param name="screenWidth">The width of the screen.</param>
        /// <param name="screenHeight">The height of the screen.</param>
        /// <param name="user">The instance of the user.</param>
        /// <returns>returns OKButton button</returns>
        public static Button OKButton(ContentManager contentManager, Vector2 position,
            int screenWidth, int screenHeight, User user)
        {
            return new Button(contentManager.Load<GifAnimation.GifAnimation>("Textures/Buttons/ok-s"),
                contentManager.Load<GifAnimation.GifAnimation>("Textures/Buttons/ok-m"), position, 
                screenWidth, screenHeight, contentManager.Load<Texture2D>("Textures/Buttons/hand"), user);
        }

        /// <summary>
        /// Create a button that have retry label
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Raafat</para>
        /// </remarks>
        /// <param name="contentManager"></param>
        /// <param name="position"></param>
        /// <param name="screenWidth"></param>
        /// <param name="screenHeight"></param>
        /// <param name="user"></param>
        /// <returns>Button</returns>
        public static Button RetryButton(ContentManager contentManager, Vector2 position,
            int screenWidth, int screenHeight, User user)
        {
            return new Button(contentManager.Load<GifAnimation.GifAnimation>("Textures/Buttons/retry-s"),
                contentManager.Load<GifAnimation.GifAnimation>("Textures/Buttons/retry-m"), position,
                screenWidth, screenHeight, contentManager.Load<Texture2D>("Textures/Buttons/hand"), user);
        }

        /// <summary>
        /// Create a button that have solution label
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Raafat</para>
        /// </remarks>
        /// <param name="contentManager"></param>
        /// <param name="position"></param>
        /// <param name="screenWidth"></param>
        /// <param name="screenHeight"></param>
        /// <param name="user"></param>
        /// <returns>Button</returns>
        public static Button SolutionButton(ContentManager contentManager, Vector2 position,
            int screenWidth, int screenHeight, User user)
        {
            return new Button(contentManager.Load<GifAnimation.GifAnimation>("Textures/Buttons/solution-s"),
                contentManager.Load<GifAnimation.GifAnimation>("Textures/Buttons/solution-m"), position,
                screenWidth, screenHeight, contentManager.Load<Texture2D>("Textures/Buttons/hand"), user);
        }


        /// <summary>
        /// Creates a pre-customized MainMenuButton.
        /// </summary>
        /// <remarks>
        /// <para>Author: HegazY</para>
        /// </remarks>
        /// <param name="contentManager">Content managaer to load the pictures.</param>
        /// <param name="position">The Position of the button.</param>
        /// <param name="screenWidth">The width of the screen.</param>
        /// <param name="screenHeight">The height of the screen.</param>
        /// <param name="user">The instance of the user.</param>
        /// <returns>returns MainMenu button</returns>
        public static Button MainMenuButton(ContentManager contentManager, Vector2 position,
            int screenWidth, int screenHeight, User user)
        {
            return new Button(contentManager.Load<GifAnimation.GifAnimation>("Textures/Buttons/menu-s"),
                contentManager.Load<GifAnimation.GifAnimation>("Textures/Buttons/menu-m"), position, 
                screenWidth, screenHeight, contentManager.Load<Texture2D>("Textures/Buttons/hand"), user);
        }


        /// <summary>
        /// Creates a pre-customized NewGame.
        /// </summary>
        /// <remarks>
        /// <para>Author: HegazY</para>
        /// </remarks>
        /// <param name="contentManager">Content managaer to load the pictures.</param>
        /// <param name="position">The Position of the button.</param>
        /// <param name="screenWidth">The width of the screen.</param>
        /// <param name="screenHeight">The height of the screen.</param>
        /// <param name="user">The instance of the user.</param>
        /// <returns>returns NewGame button</returns>
        public static Button NewGameButton(ContentManager contentManager, Vector2 position, 
            int screenWidth, int screenHeight, User user)
        {
            return new Button(contentManager.Load<GifAnimation.GifAnimation>("Textures/Buttons/newgame-s"),
                contentManager.Load<GifAnimation.GifAnimation>("Textures/Buttons/newgame-m"), position, 
                screenWidth, screenHeight, contentManager.Load<Texture2D>("Textures/Buttons/hand"), user);
        }


        /// <summary>
        /// Displays the wining or losing word on the screen.
        /// </summary>
        /// <remarks>
        /// <para>Author: HegazY</para>
        /// </remarks>
        /// <param name="spriteBatch">Draws the image on the screen</param>
        /// <param name="content">Loads the images.</param>
        /// <param name="position">The position of the word.</param>
        /// <param name="status">The user has won or not.</param>
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
        /// Displays the wining or losing word on the screen.
        /// </summary>
        /// <remarks>
        /// <para>Author: HegazY</para>
        /// </remarks>
        /// <param name="spriteBatch">Draws the image on the screen</param>
        /// <param name="content">Loads the images.</param>
        /// <param name="position">The position of the word.</param>
        /// <param name="scale">Scales the size of the word.</param>
        /// <param name="status">The user has won or not.</param>
        public static void DisplayIsWin(SpriteBatch spriteBatch, ContentManager content, Vector2 position, 
            float scale, bool status)
        {

            spriteBatch.Begin();
            if (status)
            {
                Texture2D winningPicture = content.Load<Texture2D>("Textures/WorL/winner");
                Rectangle rectangle = new Rectangle((int)position.X, (int)position.Y,
                (int)(scale * winningPicture.Width), (int)(scale * winningPicture.Height));
                spriteBatch.Draw(winningPicture, rectangle, Color.White);
            }
            else
            {
                Texture2D losingPicture = content.Load<Texture2D>("Textures/WorL/looser");
                Rectangle rectangle = new Rectangle((int)position.X, (int)position.Y,
                (int)(scale * losingPicture.Width), (int)(scale * losingPicture.Height));
                spriteBatch.Draw(losingPicture, rectangle, Color.White);
            }
            spriteBatch.End();
        }
    }
}
