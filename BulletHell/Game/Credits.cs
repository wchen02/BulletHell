using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using BulletHell.System;
using BulletHell.Game;
using BulletHell.Game.Object;
using BulletHell.Game.Interface;

namespace BulletHell.Game
{
    public class Credits : GameState
    {
        #region Fields
        private List<string> credits = new List<string>(); // Text to be shown
        private List<Vector2> creditPos = new List<Vector2>(); // Position of each text string
        private Texture2D creditTexture, background;
        private bool transition;
        private float aplhaValue;
        #endregion

        #region Initialization
        public Credits()
        {
        }

        protected override void Initialize()
        {
            credits.Add("This game is created by");
            credits.Add("Wensheng Chen");
            credits.Add("for Game Programming Class");
            credits.Add("Prof.John Sterling");
            credits.Add("NYU:POLY 2010 Spring");
            init();
        }
        #endregion

        #region Contents
        internal override void LoadContent()
        {
            creditTexture = _GLOBAL.ContentManager.Load<Texture2D>(@"sprites\credit");
            background = _GLOBAL.ContentManager.Load<Texture2D>(@"sprites\background");
        }

        protected override void UnloadContent() { }
        #endregion

        #region Update and Draw
        internal override void Update(GameTime gameTime)
        {
            _GLOBAL.InputHandler.keyboardState = Keyboard.GetState();

            NeedDraw = true;
            NeedUpdate = true;

            /* Return to menu if Esc is pressed */
            if (_GLOBAL.InputHandler.isKeyEscape())
            {
                NeedUpdate = false;
                NeedDraw = false;
                _GLOBAL.inGameState = false;
                init();
            }

            for (int i = 0; i < 5; ++i)
            {
                creditPos[i] = new Vector2(creditPos[i].X, creditPos[i].Y - 2);
            }


            if (creditPos[0].Y < -250)
            {
                transition = true;
                aplhaValue += 0.005f;
            }

            if (creditPos[0].Y < -900)
            {
                NeedUpdate = false;
                NeedDraw = false;
                _GLOBAL.inGameState = false;
                _GLOBAL.GameStateManager.GameStates.Remove(this);
                init();
            }
            _GLOBAL.InputHandler.previousKeyboardState = _GLOBAL.InputHandler.keyboardState;

        }

        internal override void Draw(GameTime gameTime)
        {
            _GLOBAL.SpriteBatch.Begin(SpriteBlendMode.AlphaBlend);
            if (transition)
                _GLOBAL.SpriteBatch.Draw(background, _GLOBAL.ViewportRect, Color.White);

            _GLOBAL.SpriteBatch.Draw(creditTexture, _GLOBAL.ViewportRect, new Color(255.0f, 255.0f, 255.0f, (float)Math.Cos(aplhaValue)));
            for (int i = 0; i < credits.Count; ++i)
            {
                _GLOBAL.SpriteBatch.DrawString(_GLOBAL.SpriteFont, credits[i], creditPos[i], Color.White);
            }
            _GLOBAL.SpriteBatch.End();
        }
        #endregion

        #region Methods
        private void init()
        {
            creditPos.Clear();
            for (int i = 0; i < credits.Count; ++i)
            {
                /* Center aligns all text */
                creditPos.Add(new Vector2(_GLOBAL.Viewport.Width / 2 - _GLOBAL.SpriteFont.MeasureString(credits[i]).X / 2,
                _GLOBAL.Viewport.Height + i * (_GLOBAL.HuakanFont.MeasureString(credits[i]).Y + 5)));
            }

            transition = false;
            aplhaValue = 0;
        }
        #endregion
    }
}