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

namespace BulletHell.System
{
    public class Dialog : GameState
    {
        #region Fields
        private Texture2D dialogBox;
        private string title, message;
        private Vector2 position, titlePos, msgPos, optionPos;
        private int index, selected;
        #endregion

        #region Initialization
        public Dialog(GameStateManager gameStateManager, Texture2D dialogBox, string title, string message, Vector2 position2)
        {
            this.dialogBox = dialogBox;
            this.title = title;
            this.message = message;
            position = position2;
            //this.title = "Exit?";
            this.position = new Vector2((position.X - dialogBox.Width) / 2, (position.Y - dialogBox.Height) / 2);
            titlePos = new Vector2( position.X+dialogBox.Width/2-5*(title.Length), position.Y + 15);
            msgPos = new Vector2(position.X + 20, position.Y + 55);
            optionPos = new Vector2(position.X + dialogBox.Width/2, msgPos.Y + 50);
            index = 0;
            selected = -1;
        }
        protected override void Initialize()
        {
        }
        #endregion

        #region Contents
        internal override void LoadContent()
        {
        }

        protected override void UnloadContent() { }
        #endregion

        #region Update and Draw
        internal override void Update(GameTime gameTime)
        {
            if (selected == -1)
            {
                if (_GLOBAL.InputHandler.isKeyRight())
                {
                    index = (index > 0) ? index - 1 : 1;
                    //BulletHell.audioManager.play("cursorMove");
                }
                else if (_GLOBAL.InputHandler.isKeyLeft())
                {
                    index = (index < 1) ? index + 1 : 0;
                    //BulletHell.audioManager.play("cursorMove");
                }
                else if (_GLOBAL.InputHandler.isKeyEnter())
                {
                    selected = index;
                    //BulletHell.audioManager.play("confrim");
                }
                else if (_GLOBAL.InputHandler.isKeyEscape())
                {
                    selected = 0;
                }
                NeedUpdate = true;
            }
            else
            {
                if (selected == 1)
                    _GLOBAL.Quit = true;
                selected = -1;
                _GLOBAL.GameStateSelect = 0;
                NeedDraw = false;
                NeedUpdate = false;
            }
        }

        internal override void Draw(GameTime gameTime)
        {
            _GLOBAL.SpriteBatch.Begin(SpriteBlendMode.AlphaBlend);
            _GLOBAL.SpriteBatch.Draw(dialogBox, position, Color.White);
            _GLOBAL.SpriteBatch.DrawString(_GLOBAL.SpriteFont, title, titlePos, Color.White);
            _GLOBAL.SpriteBatch.DrawString(_GLOBAL.SpriteFont, message, msgPos, Color.White);
            optionPos.X = titlePos.X - 80;
            _GLOBAL.SpriteBatch.DrawString(_GLOBAL.SpriteFont, "No", optionPos, Color.White, 0.0f, Vector2.Zero, (index == 0) ? 1.25f : 1.0f, SpriteEffects.None, 0.0f);
            optionPos.X = titlePos.X + 80;
            _GLOBAL.SpriteBatch.DrawString(_GLOBAL.SpriteFont, "Yes", optionPos, Color.White, 0.0f, Vector2.Zero, (index == 1) ? 1.25f : 1.0f, SpriteEffects.None, 0.0f);
            _GLOBAL.SpriteBatch.End();
        }
        #endregion
    }
}