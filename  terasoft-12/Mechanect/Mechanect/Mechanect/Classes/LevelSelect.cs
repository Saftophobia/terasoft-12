using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Mechanect
{
    public class levelSelect 
    {
        //Position of the level select menu on the screen.
        public Vector2 position { get; set; }
        
        public Texture2D textureStrip, texture;
        public int frame, width, height, level; 
        SpriteBatch spriteBatch;
        ContentManager Content;
        Button rightArrow, leftArrow, firstButton, secondButton, thirdButton;
        List<Button> Buttons;
        int[] values;

        public levelSelect(Game game, Vector2 position, SpriteBatch spriteBatch)
        {
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


            int ButtonWidth = Content.Load<GifAnimation.GifAnimation>("Textures/dummy").GetTexture().Width;
            //Create and Initialize all Buttons.
            rightArrow = new Button(Content.Load<GifAnimation.GifAnimation>("Textures/rightArrow"), Content.Load<GifAnimation.GifAnimation>("Textures/rightArrow"),
                new Vector2(position.X + ButtonWidth + 65 + width, position.Y + 15), screenW, screenH, Content.Load<Texture2D>("Textures/Buttons/Hand"));
            leftArrow = new Button(Content.Load<GifAnimation.GifAnimation>("Textures/leftArrow"), Content.Load<GifAnimation.GifAnimation>("Textures/leftArrow"),
                new Vector2(position.X, position.Y + 15), screenW, screenH, Content.Load<Texture2D>("Textures/Buttons/Hand"));
            firstButton = new Button(Content.Load<GifAnimation.GifAnimation>("Textures/dummy"), Content.Load<GifAnimation.GifAnimation>("Textures/dummySelected"),
                new Vector2(leftArrow.Position.X + Content.Load<GifAnimation.GifAnimation>("Textures/leftArrow").GetTexture().Width + 73, position.Y + 33),
                screenW, screenH, Content.Load<Texture2D>("Textures/Buttons/Hand"));
            secondButton = new Button(Content.Load<GifAnimation.GifAnimation>("Textures/dummy"), Content.Load<GifAnimation.GifAnimation>("Textures/dummySelected"),
                new Vector2(firstButton.Position.X + ButtonWidth + 8, position.Y + 33), screenW, screenH, Content.Load<Texture2D>("Textures/Buttons/Hand"));
            thirdButton = new Button(Content.Load<GifAnimation.GifAnimation>("Textures/dummy"), Content.Load<GifAnimation.GifAnimation>("Textures/dummySelected"),
                new Vector2(secondButton.Position.X + ButtonWidth + 8, position.Y + 33), screenW, screenH, Content.Load<Texture2D>("Textures/Buttons/Hand"));


            texture = Content.Load<Texture2D>("Textures/texture");
            textureStrip = Content.Load<Texture2D>("Textures/textureStrip");

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
            if (rightArrow.isClicked() && frame != 2)
            {
                frame++;
                values[0]++;
                values[1]++;
                values[2]++;
                rightArrow.reset();
            }
            //Same as above but move the strip to the left by updating which frame to draw 
            //and decrease value of the Buttons.
            if (leftArrow.isClicked() && frame != 0)
            {
                 frame--;
                 values[0]--;
                 values[1]--;
                 values[2]--;
                 leftArrow.reset();
            }
            //If any of the level Buttons is pressed.. set the level to the value of that Button.
            if (firstButton.isClicked())
                level = values[0];
            if (secondButton.isClicked())
                level = values[1];
            if (thirdButton.isClicked())
                level = values[2];


            foreach (Button b in Buttons)
                b.update(gameTime);

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
            spriteBatch.Draw(textureStrip, new Vector2(position.X + Content.Load<GifAnimation.GifAnimation>("leftArrow").GetTexture().Width + 70, position.Y + 30), new Rectangle(142 * frame, 0, width, height), Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
            spriteBatch.Draw(texture, position, Color.White);
            spriteBatch.End();
            //Draw each Button in the list
            foreach (Button b in Buttons)
                b.draw(spriteBatch);

        }
    }
}
