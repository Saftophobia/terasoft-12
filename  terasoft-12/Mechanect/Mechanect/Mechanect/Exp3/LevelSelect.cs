using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Mechanect.Classes;
using Mechanect.Common;
using ButtonsAndSliders;

namespace Mechanect.Exp3
{
    public class levelSelect
    {
        //Position of the level select menu on the screen.
        public Vector2 position { get; set; }

        private User user;
        private Texture2D textureStrip, textureStrip1, textureStrip2, textureStrip3, textureStrip4, textureStrip5, texture;
        private int frame, width, height;
        public int level;
        SpriteBatch spriteBatch;
        ContentManager Content;
        Button rightArrow, leftArrow, firstButton, secondButton, thirdButton;
        List<Button> Buttons;
        int[] values;

        public levelSelect(Microsoft.Xna.Framework.Game game, Vector2 position, SpriteBatch spriteBatch, User u)
        {
            this.user = u;
            this.spriteBatch = spriteBatch;
            this.position = position;
            this.frame = 0;
            this.Content = game.Content;
        }

        /// <remarks>
        ///<para>AUTHOR: Omar Abdulaal </para>
        ///</remarks>
        /// <summary>
        /// Initializes Buttons and Values.
        /// </summary>
        public void Initialize(int screenw, int screenh)
        {
            //Initialize level values
            values = new int[3];
            values[0] = 1;
            values[1] = 2;
            values[2] = 3;


            //List that Contains All Buttons in the level select part.
            Buttons = new List<Button>();

            //width and height of the textureStrip
            width = 425;
            height = 200;

            int screenW = screenw;
            int screenH = screenh;

            level = 1;

            int ButtonWidth = Content.Load<GifAnimation.GifAnimation>("Textures/dummy").GetTexture().Width;
            //Create and Initialize all Buttons.

            Vector2 leftArrowPos = new Vector2(position.X, position.Y + 15);
            Vector2 firstButtonPos = new Vector2(leftArrowPos.X + Content.Load<GifAnimation.GifAnimation>("Textures/leftArrow").GetTexture().Width + 73, position.Y + 33);
            Vector2 secondButtonPos = new Vector2(firstButtonPos.X + ButtonWidth + 8, position.Y + 33);
            Vector2 thirdButtonPos = new Vector2(secondButtonPos.X + ButtonWidth + 8, position.Y + 33);

            rightArrow = new Button(Content.Load<GifAnimation.GifAnimation>("Textures/rightArrow"), Content.Load<GifAnimation.GifAnimation>("Textures/rightArrow"),
                new Vector2(position.X + ButtonWidth + 65 + width, position.Y + 15), screenW, screenH, Content.Load<Texture2D>("Textures/Buttons/Hand"), user);
            leftArrow = new Button(Content.Load<GifAnimation.GifAnimation>("Textures/leftArrow"), Content.Load<GifAnimation.GifAnimation>("Textures/leftArrow"),
                leftArrowPos, screenW, screenH, Content.Load<Texture2D>("Textures/Buttons/Hand"), user);
            firstButton = new Button(Content.Load<GifAnimation.GifAnimation>("Textures/dummy"), Content.Load<GifAnimation.GifAnimation>("Textures/dummySelected"),
                firstButtonPos, screenW, screenH, Content.Load<Texture2D>("Textures/Buttons/Hand"), user);
            secondButton = new Button(Content.Load<GifAnimation.GifAnimation>("Textures/dummy"), Content.Load<GifAnimation.GifAnimation>("Textures/dummySelected"),
                secondButtonPos, screenW, screenH, Content.Load<Texture2D>("Textures/Buttons/Hand"), user);
            thirdButton = new Button(Content.Load<GifAnimation.GifAnimation>("Textures/dummy"), Content.Load<GifAnimation.GifAnimation>("Textures/dummySelected"),
                thirdButtonPos, screenW, screenH, Content.Load<Texture2D>("Textures/Buttons/Hand"), user);

            //Load Textures
            texture = Content.Load<Texture2D>("Textures/texture");
            textureStrip1 = Content.Load<Texture2D>("Textures/textureStrip1");
            textureStrip2 = Content.Load<Texture2D>("Textures/textureStrip2");
            textureStrip3 = Content.Load<Texture2D>("Textures/textureStrip3");
            textureStrip4 = Content.Load<Texture2D>("Textures/textureStrip4");
            textureStrip5 = Content.Load<Texture2D>("Textures/textureStrip5");
            textureStrip = textureStrip1;

            //Add Buttons to the list.
            Buttons.Add(rightArrow);
            Buttons.Add(leftArrow);
            Buttons.Add(firstButton);
            Buttons.Add(secondButton);
            Buttons.Add(thirdButton);

        }


        /// <remarks>
        ///<para>AUTHOR: Omar Abdulaal </para>
        ///</remarks>
        /// <summary>
        /// Update Method.
        /// </summary>
        public void Update(GameTime gameTime)
        {

            //If right arrow Button is pressed.. Move the textureStrip one frame to the right 
            //and increase the values of the Buttons to match the levels
            if (rightArrow.IsClicked() && frame != 2)
            {
                frame++;
                values[0]++;
                values[1]++;
                values[2]++;
                rightArrow.Reset();
            }
            //Same as above but move the strip to the left by updating which frame to draw 
            //and decrease value of the Buttons.
            else
            {
                if (leftArrow.IsClicked() && frame != 0)
                {
                    frame--;
                    values[0]--;
                    values[1]--;
                    values[2]--;
                    leftArrow.Reset();
                }
            }
            //If any of the level Buttons is pressed.. set the level to the value of that Button.
            if (firstButton.IsClicked())
            {
                level = values[0];
                firstButton.Reset();
            }
            else
            {
                if (secondButton.IsClicked())
                {
                    level = values[1];
                    secondButton.Reset();
                }
                else
                {
                    if (thirdButton.IsClicked())
                    {
                        level = values[2];
                        thirdButton.Reset();
                    }
                }
            }

            switch (level)
            {
                case 1:
                    textureStrip = textureStrip1;
                    break;
                case 2:
                    textureStrip = textureStrip2;
                    break;
                case 3:
                    textureStrip = textureStrip3;
                    break;
                case 4:
                    textureStrip = textureStrip4;
                    break;
                case 5:
                    textureStrip = textureStrip5;
                    break;
            }

            foreach (Button b in Buttons)
                b.Update(gameTime);

        }
        /// <remarks>
        ///<para>AUTHOR: Omar Abdulaal </para>
        ///</remarks>
        /// <summary>
        /// XNA Draw Method.
        /// </summary>
        public void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            //Draw the texture and textureStrip according to the frame
            spriteBatch.Draw(textureStrip, new Vector2(position.X + Content.Load<GifAnimation.GifAnimation>("Textures/leftArrow").GetTexture().Width + 70, position.Y + 30), new Rectangle(142 * frame, 0, width, height), Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
            spriteBatch.Draw(texture, position, Color.White);
            spriteBatch.End();
            //Draw each Button in the list
            foreach (Button b in Buttons)
                b.Draw(spriteBatch);

        }
    }
}
