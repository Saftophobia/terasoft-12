using System;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Mechanect.Common;
using Mechanect.Classes;

namespace Mechanect
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    class Instruction
    {
        SpriteBatch spriteBatch;
        SpriteFont spriteFont;
        public SpriteFont SpriteFont
        {
            get
            {
                return spriteFont;
            }
            set
            {
                spriteFont = value;
            }
        }
        Rectangle rectangle;
        Rectangle Rectangle
        {
            get
            {
                return rectangle;
            }
            set
            {
                rectangle = value;
            }
        }
       
        Vector2 origin;
        Vector2 positionInScreen;
        String instructions;
        Texture2D mytexture;
        public Texture2D MyTexture
        {
            get
            {
                return mytexture;
            }
            set
            {
                mytexture = value;
            }
        }
        Vector2 startPosition = Vector2.Zero;
        Vector2 ButtonPosition = new Vector2(300, 300);
        Button button;
        public Button Button
        {
           get { return button; }
            set { button = value; }
        }
        ContentManager contentManager;
        public ContentManager ContentManager
        {
            get
            {
                return contentManager;
            }
            set
            {
                contentManager = value;
            }
        }
        int screenWidth = 800;
        int screenHeight = 400;
        GraphicsDevice device;
        public GraphicsDevice Device
        {
            get
            {
                return device;
            }
            set
            {
                device = value;
            }
        }
        public Vector2 PositionInScreen
        {
            get
            {
                return positionInScreen;
            }
            set
            {
                positionInScreen = value;

            }
        }

        public String Instructions
        {
            get
            {
                return instructions;
            }
            set
            {
                instructions = value;
            }
        }

        User user;
     
  
        public Instruction()
        {
           // game.Window.AllowUserResizing = true;
            instructions = "";
            origin = new Vector2(0f, 0f);
        //    game.IsMouseVisible = true;
        }
       
        /// <summary>
        /// Set the origin Vector to be the left top corner of the screen.
        /// Allow the User to resize the screen, and make the mouse visible in the screen
        /// <remarks>
        /// <para>Author: Mohamed Raafat</para>
        /// </remarks>
        /// </summary>
        public Instruction(String instructions, ContentManager contentManager, SpriteBatch spritebatch, GraphicsDevice device, User user)
        {
            this.instructions = instructions;
            this.origin = new Vector2(0f, 0f);
            this.contentManager = contentManager;
            this.spriteBatch = spritebatch;
            this.device = device;
            this.user = user;
            this.button =  Tools3.OKButton(contentManager, ButtonPosition, screenWidth, screenHeight,user);
        }



        /// <summary>
        /// LoadContent will be called only once before drawing and its the place to load
        /// all of your content.
        /// </summary>
        /// <remarks>
        ///<para>AUTHOR: Khaled Salah </para>
        ///</remarks>
        
        public  void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            screenWidth = device.Viewport.Width;
            screenHeight = device.Viewport.Height;
            button =  Tools3.OKButton(contentManager,new Vector2(device.Viewport.Width / 2, device.Viewport.Height-400),
           screenWidth , screenHeight,user );
           
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// This is called when the game screen should draw itself.
        /// The method draws the instruction screen and the given text along with an ok button down the screen.
        /// </summary>
        /// <remarks>
        ///<para>AUTHOR: Khaled Salah </para>
        ///</remarks>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>    
        public void Draw(GameTime gameTime)
        {
           // device.Clear(Color.YellowGreen);
            string output = WrapText(this.instructions);
            button.Draw(spriteBatch);
            spriteBatch.Begin();
            spriteBatch.DrawString(spriteFont, output , positionInScreen, Color.Black, 0, origin, 1f, SpriteEffects.None, 0.0f);
            spriteBatch.End();
        }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <para>Author: Mohamed Raafat</para>
        

        String getInsructions()
        {
            String output = WrapText(this.instructions);
            return output;
        }

        /// <summary>
        /// Makes sure that text displayed will not exceeds screen boundries
        /// </summary>
        /// <para>AUTHOR: Mohamed Raafat </para>
        /// <param name="text">text to be displayed on screen</param>
        /// <returns>String</returns>
        private String WrapText(String text)
        {
            String line = String.Empty;
            String returnString = String.Empty;
            String[] wordArray = text.Split(' ');

            foreach (String word in wordArray)
            {
                if (spriteFont.MeasureString(line + word).Length() > rectangle.Width)
                {
                    returnString = returnString + line + '\n';
                    line = String.Empty;
                }

                line = line + word + ' ';
            }

            return returnString + line;
        }
    
        
        

    }

}
