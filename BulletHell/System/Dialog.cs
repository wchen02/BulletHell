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
    /* This class's job is to provide a dynamic size diaglogbox which contains the title, message, and yes/no option */
    public class Dialog : GameState
    {
        #region Fields

        /* b => bottom; c => center; t => top; l => left; r => right */
        private Texture2D bc, bl, br, cc, cl, cr, tc, tl, tr;
        private Vector2 blPos, brPos, tlPos, trPos;
        private Rectangle bcRect, ccRect, tcRect, clRect, crRect;
        private string title, message;
        private Vector2 position, titlePos, msgPos, optionPos;
        private int index, selected, height, width;
        #endregion

        #region Initialization
        public Dialog(string title, string message, Vector2 position2)
        {
            this.title = title;
            this.message = message;
            position = position2;
            setUp();
        }

        protected override void Initialize()
        {
            this.LoadContent();
        }

        private void setUp(){
            int x = (int)position.X;
            int y = (int)position.Y;
            int paddingX = 10, paddingY = 10;
            height = (int)_GLOBAL.SpriteFont.MeasureString(message).Y;
            width = (int)_GLOBAL.SpriteFont.MeasureString(message).X;

            /* Calculations for position of each texture */
            tlPos = new Vector2(x - width / 2 - tl.Width - paddingX, y - height / 2 - tl.Height - paddingY);
            trPos = new Vector2(x + width / 2 + paddingX, y - height / 2 - tr.Height - paddingY);
            tcRect = new Rectangle(x - width / 2 - paddingX, y - height / 2 - tl.Height - paddingY,
                width + 2 * paddingX, height + 2 * paddingY);

            clRect = new Rectangle(x - width / 2 - cl.Width - paddingX, y - height / 2 - paddingY,
                cl.Width + paddingX, 2 * height + 2 * paddingY);
            crRect = new Rectangle(x + width / 2, y - height / 2 - paddingY,
                cr.Width + paddingX, 2 * height + 2 * paddingY);
            ccRect = new Rectangle(x - width / 2 - paddingX, y - height / 2 - paddingY, width + 2 * paddingX, 2 * height + 2 * paddingY);

            blPos = new Vector2(x - width / 2 - bl.Width - paddingX, y + height + height/2 + paddingY);
            brPos = new Vector2(x + width / 2 + paddingX, y + height + height / 2 + paddingY);
            bcRect = new Rectangle(x - width / 2 - paddingX, y + height + height / 2 + paddingY,
                width + 2 * paddingX, br.Height);

            //this.position = new Vector2((position.X - dialogBox.Width) / 2, (position.Y - dialogBox.Height) / 2);
            titlePos = new Vector2(tcRect.X + paddingX, tcRect.Y + paddingY);
            msgPos = new Vector2(ccRect.X + paddingX, paddingY / 2 + titlePos.Y + _GLOBAL.SpriteFont.MeasureString(title).Y);
            optionPos = new Vector2(x, y + height);
            index = 0;
            selected = -1;
        }
        #endregion

        #region Contents
        internal override void LoadContent()
        {
            bc = _GLOBAL.ContentManager.Load<Texture2D>(@"sprites/DialogBox/DialogBox_bc");
            bl = _GLOBAL.ContentManager.Load<Texture2D>(@"sprites/DialogBox/DialogBox_bl");
            br = _GLOBAL.ContentManager.Load<Texture2D>(@"sprites/DialogBox/DialogBox_br");
            tc = _GLOBAL.ContentManager.Load<Texture2D>(@"sprites/DialogBox/DialogBox_tc");
            tl = _GLOBAL.ContentManager.Load<Texture2D>(@"sprites/DialogBox/DialogBox_tl");
            tr = _GLOBAL.ContentManager.Load<Texture2D>(@"sprites/DialogBox/DialogBox_tr");
            cc = _GLOBAL.ContentManager.Load<Texture2D>(@"sprites/DialogBox/DialogBox_cc");
            cl = _GLOBAL.ContentManager.Load<Texture2D>(@"sprites/DialogBox/DialogBox_cl");
            cr = _GLOBAL.ContentManager.Load<Texture2D>(@"sprites/DialogBox/DialogBox_cr");
        }

        protected override void UnloadContent() { }
        #endregion

        #region Update and Draw
        internal override void Update(GameTime gameTime)
        {
            _GLOBAL.InputHandler.keyboardState = Keyboard.GetState();
            if (selected == -1)
            {
                if (_GLOBAL.InputHandler.isKeyRight())
                {
                    index = (index > 0) ? index - 1 : 1;
                    //_GLOBAL. .play("cursorMove");
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
                _GLOBAL.inGameState = false;
                NeedDraw = false;
                NeedUpdate = false;
            }
            _GLOBAL.InputHandler.previousKeyboardState = _GLOBAL.InputHandler.keyboardState;
        }

        internal override void Draw(GameTime gameTime)
        {
            _GLOBAL.SpriteBatch.Begin(SpriteBlendMode.AlphaBlend);
            _GLOBAL.SpriteBatch.Draw(tl, tlPos, Color.White);
            _GLOBAL.SpriteBatch.Draw(tc, tcRect, Color.White);
            _GLOBAL.SpriteBatch.Draw(tr, trPos, Color.White);

            _GLOBAL.SpriteBatch.Draw(cl, clRect, Color.White);
            _GLOBAL.SpriteBatch.Draw(cc, ccRect, Color.White);
            _GLOBAL.SpriteBatch.Draw(cr, crRect, Color.White);

            _GLOBAL.SpriteBatch.Draw(bl, blPos, Color.White);
            _GLOBAL.SpriteBatch.Draw(bc, bcRect, Color.White);
            _GLOBAL.SpriteBatch.Draw(br, brPos, Color.White);

            _GLOBAL.SpriteBatch.DrawString(_GLOBAL.SpriteFont, title, titlePos, Color.White);
            _GLOBAL.SpriteBatch.DrawString(_GLOBAL.SpriteFont, message, msgPos, Color.White);
            optionPos.X = position.X - width / 3;
            _GLOBAL.SpriteBatch.DrawString(_GLOBAL.SpriteFont, "No", optionPos, Color.White, 0.0f, Vector2.Zero, (index == 0) ? 1.25f : 1.0f, SpriteEffects.None, 0.0f);
            optionPos.X = position.X + width / 3 - (int)(_GLOBAL.SpriteFont.MeasureString("Yes").X);
            _GLOBAL.SpriteBatch.DrawString(_GLOBAL.SpriteFont, "Yes", optionPos, Color.White, 0.0f, Vector2.Zero, (index == 1) ? 1.25f : 1.0f, SpriteEffects.None, 0.0f);
            _GLOBAL.SpriteBatch.End();
        }
        #endregion
    }
}