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
        SpriteFont font1;
        public SpriteFont Font1
        {
            get
            {
                return font1;
            }
            set
            {
                font1 = value;
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
        Vector2 sPos = Vector2.Zero;
        Vector2 ButtonPosition = new Vector2(300, 300);
        Button b;
        public Button Button
        {
           get { return b; }
            set { b = value; }
        }
        ContentManager cmanager;
        public ContentManager Cmanager
        {
            get
            {
                return cmanager;
            }
            set
            {
                cmanager = value;
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
        public Instruction(String instructions, ContentManager content, SpriteBatch batch, GraphicsDevice device, User u)
        {
            this.instructions = instructions;
            origin = new Vector2(0f, 0f);
            cmanager = content;
            spriteBatch = batch;
            this.device = device;
            this.user = u;
            b =  Tools3.OKButton(cmanager, ButtonPosition, screenWidth, screenHeight,u);
        }




        /// <remarks>
        ///<para>AUTHOR: Khaled Salah </para>
        ///</remarks>
        /// <summary>
        /// LoadContent will be called only once before drawing and its the place to load
        /// all of your content.
        /// </summary>
        public  void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            screenWidth = device.Viewport.Width;
            screenHeight = device.Viewport.Height;
            b =  Tools3.OKButton(cmanager,new Vector2(device.Viewport.Width / 2, device.Viewport.Height-400),
           screenWidth , screenHeight,user );
           
            // TODO: use this.Content to load your game content here
        }

       
        /// <remarks>
        ///<para>AUTHOR: Khaled Salah </para>
        ///</remarks>
        /// <summary>
        /// This is called when the game screen should draw itself.
        /// The method draws the instruction screen and the given text along with an ok button down the screen.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>    
        public void Draw(GameTime gameTime)
        {
           // device.Clear(Color.YellowGreen);
            string output = WrapText(Font1, this.instructions, screenWidth);
            b.Draw(spriteBatch);
            spriteBatch.Begin();
            spriteBatch.DrawString(Font1, output , positionInScreen, Color.Black, 0, origin, 1f, SpriteEffects.None, 0.0f);
            spriteBatch.End();
        }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <para>Author: Mohamed Raafat</para>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>

        String getInsructions()
        {
            String output = WrapText(Font1, this.instructions, screenWidth);
            return output;
        }

        /// <summary>
        /// Makes sure that text displayed will not exceeds screen boundries
        /// </summary>
        /// <para>AUTHOR: Mohamed Raafat </para>
        /// <param name="spriteFont">contains properties of the font</param>
        /// <param name="text">text to be displayed on screen</param>
        /// <param name="maxLineWidth">max line width to be displayed</param>
        /// <returns>builder</returns>
        public String WrapText(SpriteFont spriteFont, String text, float maxLineWidth)
        {
            string[] words = text.Split(' ');
            StringBuilder builder = new StringBuilder();
            float lineWidth = PositionInScreen.X;
            float spaceWidth = spriteFont.MeasureString(" ").X;

            foreach (String word in words)
            {
                Vector2 width = spriteFont.MeasureString(word);


                if (lineWidth + width.X < maxLineWidth || lineWidth + width.X < device.PresentationParameters.BackBufferWidth - 5)
                {
                    builder.Append(word + " ");
                    lineWidth += width.X + spaceWidth;

                }
                else
                {

                    builder.Append("\n" + word + " ");
                    lineWidth = 0;

                }
            }
            return builder.ToString();
        }
    
        
        

    }

}
