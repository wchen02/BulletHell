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
        private string credits;
        private Vector2 creditPos;
        private Texture2D creditTexture, background;
        private bool transition = false;
        private float aplhaValue = 45.0f;
        #endregion

        #region Initialization
        public Credits(GameStateManager gameStateManager)
        {
        }

        protected override void Initialize()
        {
            credits = "   This game is created by\n\n";
            credits += "         Wensheng Chen    \n\n";
            credits += "for Game Programming Class\n\n";
            credits += "    Prof.John Sterling    \n\n";
            credits += "   NYU:POLY 2010 Spring   \n\n";
            creditPos = new Vector2(_GLOBAL.Viewport.Width / 2 - 150,
                _GLOBAL.Viewport.Height);
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
            NeedDraw= true;
            NeedUpdate = true;
            creditPos.Y -= 2;

            if (_GLOBAL.InputHandler.isKeyEscape())
            {
                reset();
            }

            if (creditPos.Y < -250)
            {
                transition = true;
                aplhaValue += 0.005f;
            }
            if (creditPos.Y < -500)
            {
                reset();
            }
        }

        internal override void Draw(GameTime gameTime)
        {
            _GLOBAL.SpriteBatch.Begin(SpriteBlendMode.AlphaBlend);
            if (transition)
                _GLOBAL.SpriteBatch.Draw(background, _GLOBAL.ViewportRect, Color.White);

            _GLOBAL.SpriteBatch.Draw(creditTexture, _GLOBAL.ViewportRect, new Color(255.0f, 255.0f, 255.0f, (float)Math.Cos(aplhaValue)));
            _GLOBAL.SpriteBatch.DrawString(_GLOBAL.SpriteFont, credits, creditPos, Color.White);
            _GLOBAL.SpriteBatch.End();
        }
        #endregion

        #region Methods
        private void reset()
        {
            _GLOBAL.GameStateSelect = 0;
            NeedUpdate = false;
            NeedDraw = false;
            transition = false;
            creditPos.Y = _GLOBAL.Viewport.Height;
            aplhaValue = 45.0f;
        }
        #endregion
    }
}