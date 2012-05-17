using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Mechanect.Classes;

namespace Mechanect
{
    public class ThemeAndLevel
    {

        public Vector2 Position { get; set; }
        private readonly User _user;
        private Texture2D _textureStrip, _theme1, _texture;
        //private Texture2D _theme2;
        private Texture2D _easy, _medium, _hard, _selected;
        private int _frame, _width, _height, _frame2;
        public int Themeno, Levelno;
        readonly SpriteBatch _spriteBatch;
        readonly ContentManager _content;
        Button _rightArrow, _leftArrow;
        List<Button> _buttons;
        Button _rightArrow2, _leftArrow2;
        private readonly Vector2 position2;
        public ThemeAndLevel(Microsoft.Xna.Framework.Game game, Vector2 position, SpriteBatch spriteBatch, User u)
        {
            _user = u;
            _spriteBatch = spriteBatch;
            Position = position;
            _frame = 0;
            _content = game.Content;
            _frame2 = 0;
            position2 = new Vector2(position.X, Position.Y + 200);
        }

        /// <summary>
        /// loading contents and initilizing buttons
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Tamer Nabil </para>
        /// </remarks>
        /// <param name="screenw">screen width</param>
        /// <param name="screenh">screen height</param>
        public void Initialize(int screenw, int screenh)
        {

            //List that Contains All Buttons
            _buttons = new List<Button>();
            _width = 200;
            _height = 140;
            var screenW = screenw;
            var screenH = screenh;
            Themeno = 1;
            Levelno = 1;
            var buttonWidth = _content.Load<GifAnimation.GifAnimation>("Textures/dummy").GetTexture().Width;
            //Create and Initialize all Buttons.

            var leftArrowPos = new Vector2(Position.X + 100, Position.Y + 15);
            var leftArrowPos2 = new Vector2(Position.X + 100, Position.Y + 60 + leftArrowPos.Y);

            _rightArrow = new Button(_content.Load<GifAnimation.GifAnimation>("Textures/rightArrow"), _content.Load<GifAnimation.GifAnimation>("Textures/rightArrow"),
               new Vector2(Position.X + buttonWidth + 200 + _width, Position.Y + 15), screenW, screenH, _content.Load<Texture2D>("Textures/Buttons/Hand"), _user);
            _leftArrow = new Button(_content.Load<GifAnimation.GifAnimation>("Textures/leftArrow"), _content.Load<GifAnimation.GifAnimation>("Textures/leftArrow"),
                leftArrowPos, screenW, screenH, _content.Load<Texture2D>("Textures/Buttons/Hand"), _user);

            _rightArrow2 = new Button(_content.Load<GifAnimation.GifAnimation>("Textures/rightArrow"), _content.Load<GifAnimation.GifAnimation>("Textures/rightArrow"),
               new Vector2(Position.X + buttonWidth + 200 + _width, Position.Y + 60 + leftArrowPos.Y), screenW, screenH, _content.Load<Texture2D>("Textures/Buttons/Hand"), _user);
            _leftArrow2 = new Button(_content.Load<GifAnimation.GifAnimation>("Textures/leftArrow"), _content.Load<GifAnimation.GifAnimation>("Textures/leftArrow"),
                leftArrowPos2, screenW, screenH, _content.Load<Texture2D>("Textures/Buttons/Hand"), _user);
            //Load Textures
            _texture = _content.Load<Texture2D>("Textures/texture");
            _theme1 = _content.Load<Texture2D>("Textures/Experiment2/Sliders Images/Theme1");
            //_theme2 = _content.Load<Texture2D>("Textures/Experiment2/Sliders Images/ball");
            _textureStrip = _theme1;

            _easy = _content.Load<Texture2D>("Textures/Experiment2/Sliders Images/easy");
            _medium = _content.Load<Texture2D>("Textures/Experiment2/Sliders Images/medium");
            _hard = _content.Load<Texture2D>("Textures/Experiment2/Sliders Images/hard");
            _selected = _easy;
            //Add Buttons to the list.
            _buttons.Add(_rightArrow);
            _buttons.Add(_rightArrow2);
            _buttons.Add(_leftArrow);
            _buttons.Add(_leftArrow2);
        }

        /// <summary>
        /// The update method that update the engine work always
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Tamer Nabil </para>
        /// </remarks>
        /// <param name="gameTime">gameTime</param>
        public void Update(GameTime gameTime)
        {
            if (_rightArrow.IsClicked() && _frame != 1)
            {
                _frame = 1;
                Themeno++;
                _rightArrow.Reset();
            }

            if (_leftArrow.IsClicked() && _frame != 0)
            {
                _frame = 0;
                Themeno--;
                _leftArrow.Reset();
            }
            if (_rightArrow2.IsClicked() && _frame2 != 3)
            {
                _frame2++;
                Levelno++;
                _rightArrow2.Reset();
            }

            if (_leftArrow2.IsClicked() && _frame2 != 0)
            {
                _frame2--;
                Levelno--;
                _leftArrow2.Reset();
            }

            switch (Themeno)
            {
                case 1:
                    _textureStrip = _theme1;
                    break;
                /* case 2:
                     _textureStrip = _theme2;
                     break;*/
            }

            switch (Levelno)
            {
                case 1:
                    _selected = _easy;
                    break;
                case 2:
                    _selected = _medium;
                    break;
                case 3:
                    _selected = _hard;
                    break;
            }


            foreach (var b in _buttons)
                b.Update(gameTime);

        }
        /// <summary>
        /// Draw method update the UI frequently
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Tamer Nabil </para>
        /// </remarks>
        /// <param name="gameTime">gameTime</param>
        public void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            //Draw theme part according to the frame
            _spriteBatch.Draw(_textureStrip, new Vector2(Position.X + _content.Load<GifAnimation.GifAnimation>("Textures/leftArrow").GetTexture().Width + 170, Position.Y + 10), new Rectangle(_frame, 0, _width, _height), Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
            _spriteBatch.Draw(_texture, Position, Color.White);
            _spriteBatch.End();


            _spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            //Draw level part according to the frame
            _spriteBatch.Draw(_selected, new Vector2(Position.X + _content.Load<GifAnimation.GifAnimation>("Textures/leftArrow").GetTexture().Width + 200, Position.Y + 210), new Rectangle(_frame2, 0, _width, _height), Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
            _spriteBatch.Draw(_texture, position2, Color.White);

            _spriteBatch.End();

            //Draw each Button in the list
            foreach (var b in _buttons)
                b.Draw(_spriteBatch);

        }
    }
}
