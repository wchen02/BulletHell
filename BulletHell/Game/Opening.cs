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
    public class Opening : GameState
    {
        private Texture2D background;
        private GameState menu;
        private bool isStartPressed;
        private Vector2 startPos;
        private float startScale;
        private String startMsg = "Press Start";

        public Opening() { }

        protected override void Initialize()
        {
            menu = new Menu();
        }

        internal override void LoadContent()
        {
            background = _GLOBAL.ContentManager.Load<Texture2D>(@"sprites\background");
            _GLOBAL.BGM = _GLOBAL.SoundBank.GetCue("OpeningBGM"); 
            _GLOBAL.BGM.Play();
        }

        protected override void UnloadContent() { }

        internal override void Update(GameTime gameTime)
        {
            NeedUpdate = true;
            NeedDraw = true;

            _GLOBAL.InputHandler.keyboardState = Keyboard.GetState();

            Vector2 startVect = _GLOBAL.HuakanFont.MeasureString(startMsg);

            /* Put the string in the center of x axis and at the bottom 200 of y axis */
            startPos = new Vector2(_GLOBAL.Viewport.Width / 2 - startVect.X / 2, _GLOBAL.Viewport.Height - 200);

            /* Activate menu if enter is pressed */
            if (!isStartPressed)
            {
                if (_GLOBAL.InputHandler.isKeyEnter())
                {
                    _GLOBAL.GameStateManager.activate(menu);
                    _GLOBAL.SoundBank.PlayCue(_GLOBAL.onConfirm);
                    isStartPressed = true;
                }

                startScale = 1.0f + (float)Math.Cos(gameTime.TotalGameTime.TotalSeconds * 1.2) / 20;
            }
            _GLOBAL.InputHandler.previousKeyboardState = _GLOBAL.InputHandler.keyboardState;
        }

        internal override void Draw(GameTime gameTime)
        {
            _GLOBAL.SpriteBatch.Begin(SpriteBlendMode.AlphaBlend);
            _GLOBAL.SpriteBatch.Draw(background, _GLOBAL.ViewportRect, Color.White);

            if (!isStartPressed)
            {
                /* Draws the string with growing and shrinking effect */
                _GLOBAL.SpriteBatch.DrawString(_GLOBAL.HuakanFont, startMsg, startPos,
                    new Color(1.0f, 1.0f, 1.0f, 0.6f + (float)Math.Cos(gameTime.TotalGameTime.TotalSeconds * 1.2) / 3),
                    0.0f, Vector2.Zero, startScale, SpriteEffects.None, 0);
            }
            _GLOBAL.SpriteBatch.End();
        }
    }
}