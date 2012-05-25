using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Mechanect.Classes;
using Mechanect.Common;
using ButtonsAndSliders;

namespace Mechanect.Exp2
{
    /// <summary>
    /// ThemeAndLevel class is the one that allows you to change theme and level for the game.
    /// </summary>
    /// <remarks>
    /// <para>AUTHOR: Tamer Nabil </para>
    /// </remarks>
    public class ThemeAndLevel
    {

        private readonly User user;
        private Texture2D selectedTheme, theme1, outlineFrame;
        //private Texture2D _theme2;
        private Texture2D easy, medium, hard, selectedLevel;
        private int frameTheme, pictureWidth, pictureHeight, frameLevel;
        ContentManager content;
        Button themeRightArrow, themeLeftArrow;
        List<Button> buttons;
        Button levelRightArrow2, levelLeftArrow;
        private readonly Vector2 levelRectanglePosition;
        /// <summary>
        /// getter and setter for level number
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Tamer Nabil </para>
        /// </remarks>
        public int levelNo { get; set; }
        /// <summary>
        /// getter and setter for theme number
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Tamer Nabil </para>
        /// </remarks>
        public int themeNo { get; set; }
        /// <summary>
        /// getters and setters for Rectangle Position of theme
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Tamer Nabil </para>
        /// </remarks>
        public Vector2 themeRectanglePosition { get; set; }

        /// <summary>
        /// Constructor that takes Theme Rectangle Position and User as input
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Tamer Nabil </para>
        /// </remarks>
        /// <param name="themeRectanglePosition">Theme Rectangle position that will be drawn at</param>
        /// <param name="user">User</param>

        public ThemeAndLevel(Vector2 themeRectanglePosition, User user)
        {
            this.user = user;
            this.themeRectanglePosition = themeRectanglePosition;
            frameTheme = 0;
            frameLevel = 0;
            levelRectanglePosition = new Vector2(themeRectanglePosition.X, themeRectanglePosition.Y + 170);
        }

        /// <summary>
        /// Loading contents and initializing buttons
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Tamer Nabil </para>
        /// </remarks>
        /// <param name="screenWidth">Screen width</param>
        /// <param name="screenHeight">Screen height</param>
        ///  <param name="gameContent">Game content</param>
        public void LoadContent(int screenWidth, int screenHeight, ContentManager gameContent)
        {
            content = gameContent;
            //List that Contains All Buttons
            buttons = new List<Button>();
            pictureWidth = 200;
            pictureHeight = 140;
            themeNo = 1;
            levelNo = 1;
            var buttonWidth = content.Load<GifAnimation.GifAnimation>("Textures/dummy").GetTexture().Width;
            //Create and Initialize all Buttons.

            var leftArrowPos = new Vector2(themeRectanglePosition.X + 100, themeRectanglePosition.Y + 15);
            var leftArrowPos2 = new Vector2(levelRectanglePosition.X + 100, levelRectanglePosition.Y + 20);

            themeRightArrow = new Button(content.Load<GifAnimation.GifAnimation>("Textures/rightArrow"),
                content.Load<GifAnimation.GifAnimation>("Textures/rightArrow"),
                new Vector2(themeRectanglePosition.X + buttonWidth + 200 + pictureWidth, themeRectanglePosition.Y + 15),
                screenWidth, screenHeight, content.Load<Texture2D>("Textures/Buttons/Hand"), user);

            themeLeftArrow = new Button(content.Load<GifAnimation.GifAnimation>("Textures/leftArrow"),
                content.Load<GifAnimation.GifAnimation>("Textures/leftArrow"),
                leftArrowPos, screenWidth, screenHeight, content.Load<Texture2D>("Textures/Buttons/Hand"), user);

            levelRightArrow2 = new Button(content.Load<GifAnimation.GifAnimation>("Textures/rightArrow"),
                content.Load<GifAnimation.GifAnimation>("Textures/rightArrow"),
                new Vector2(levelRectanglePosition.X + buttonWidth + 200 + pictureWidth, levelRectanglePosition.Y + 20),
                screenWidth, screenHeight, content.Load<Texture2D>("Textures/Buttons/Hand"), user);

            levelLeftArrow = new Button(content.Load<GifAnimation.GifAnimation>("Textures/leftArrow"),
                content.Load<GifAnimation.GifAnimation>("Textures/leftArrow"),
                leftArrowPos2, screenWidth, screenHeight, content.Load<Texture2D>("Textures/Buttons/Hand"), user);
            //Load Textures
            outlineFrame = content.Load<Texture2D>("Textures/texture");
            theme1 = content.Load<Texture2D>("Textures/Experiment2/Sliders Images/Theme1");
            //_theme2 = _content.Load<Texture2D>("Textures/Experiment2/Sliders Images/ball");
            selectedTheme = theme1;

            easy = content.Load<Texture2D>("Textures/Experiment2/Sliders Images/easy");
            medium = content.Load<Texture2D>("Textures/Experiment2/Sliders Images/medium");
            hard = content.Load<Texture2D>("Textures/Experiment2/Sliders Images/hard");
            selectedLevel = easy;
            //Add Buttons to the list.
            buttons.Add(themeRightArrow);
            buttons.Add(levelRightArrow2);
            buttons.Add(themeLeftArrow);
            buttons.Add(levelLeftArrow);
        }

        /// <summary>
        /// Update is method that get updated at gameTime 
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Tamer Nabil </para>
        /// </remarks>
        /// <param name="gameTime">gameTime</param>
        public void Update(GameTime gameTime)
        {
            if (themeRightArrow.IsClicked() && frameTheme != 1)
            {
                frameTheme = 1;
                themeNo++;
                themeRightArrow.Reset();
            }

            else if (themeLeftArrow.IsClicked() && frameTheme != 0)
            {
                frameTheme = 0;
                themeNo--;
                themeLeftArrow.Reset();
            }
            if (levelRightArrow2.IsClicked() && frameLevel != 3)
            {
                frameLevel++;
                levelNo++;
                levelRightArrow2.Reset();
            }

            else if (levelLeftArrow.IsClicked() && frameLevel != 0)
            {
                frameLevel--;
                levelNo--;
                levelLeftArrow.Reset();
            }

            switch (themeNo)
            {
                case 1:
                    selectedTheme = theme1;
                    break;
                /* case 2:
                        _textureStrip = _theme2;
                        break;*/
            }

            switch (levelNo)
            {
                case 1:
                    selectedLevel = easy;
                    break;
                case 2:
                    selectedLevel = medium;
                    break;
                case 3:
                    selectedLevel = hard;
                    break;
            }


            foreach (var b in buttons)
                b.Update(gameTime);

        }
        /// <summary>
        /// Draw method that updates the UI frequently
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Tamer Nabil </para>
        /// </remarks>
        /// <param name="spriteBatch">SpriteBatch</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            //Draw theme part according to the frame
            spriteBatch.Draw(selectedTheme, new Vector2(themeRectanglePosition.X +
                content.Load<GifAnimation.GifAnimation>("Textures/leftArrow").GetTexture().Width + 170, themeRectanglePosition.Y + 10),
                new Rectangle(frameTheme, 0, pictureWidth, pictureHeight), Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);

            spriteBatch.Draw(outlineFrame, themeRectanglePosition, Color.White);
            spriteBatch.End();


            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            //Draw level part according to the frame

            spriteBatch.Draw(selectedLevel, new Vector2(levelRectanglePosition.X + content.Load<GifAnimation.GifAnimation>("Textures/leftArrow")
                .GetTexture().Width + 200, levelRectanglePosition.Y + 20),
                new Rectangle(frameLevel, 0, pictureWidth, pictureHeight), Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
            spriteBatch.Draw(outlineFrame, levelRectanglePosition, Color.White);

            spriteBatch.End();

            //Draw each Button in the list
            foreach (var b in buttons)
            {
                spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
                b.Draw(spriteBatch);
                spriteBatch.End();

            }

        }
    }
}
