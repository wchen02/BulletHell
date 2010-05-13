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
    /* No Settings as of yet */
    public class Setting : GameState
    {
        #region Fields
        #endregion

        #region Initialization
        public Setting(GameStateManager gameStateManager)
        {
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
        internal override void Update(GameTime gameTime) { }

        internal override void Draw(GameTime gameTime)
        {
            _GLOBAL.SpriteBatch.Begin(SpriteBlendMode.AlphaBlend);
            _GLOBAL.SpriteBatch.DrawString(_GLOBAL.SpriteFont, "Settings",
                new Vector2(_GLOBAL.Viewport.Width / 2 - 50,
                100), Color.Black);
            _GLOBAL.SpriteBatch.End();
        }
        #endregion

        #region Methods
        //internal Keys captureKeys()
        //{
        //    return Keys.A;
        //}
        #endregion
    }
}