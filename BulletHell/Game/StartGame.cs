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
    public class StartGame : GameState
    {
        #region Fields
        private SpriteFont huakanFont;
        private Texture2D maskTexture;
        private GameState myGame;
        private SharedData data;
        private bool isPause = false;
        private Vector2 pausePos;
        private float fontEffect = 0.0f;
        #endregion

        #region Initialization
        public StartGame()
        {}

        protected override void Initialize()
        {
            data = new SharedData();
            pausePos = new Vector2(_GLOBAL.Viewport.Width / 2 - 100, _GLOBAL.Viewport.Height / 2 - 50);
            myGame = new Stage1(_GLOBAL.GameStateManager);
            _GLOBAL.GameStateManager.activate(myGame);
        }
        #endregion

        #region Contents
        internal override void LoadContent()
        {
            huakanFont = _GLOBAL.ContentManager.Load<SpriteFont>("fonts//DFShaoNvW5-GB");
            maskTexture = _GLOBAL.ContentManager.Load<Texture2D>("sprites//dot");
        }

        protected override void UnloadContent() { }
        #endregion

        #region Update and Draw
        internal override void Update(GameTime gameTime)
        {
            if (!isPause)
                data.Update(gameTime);

            NeedUpdate = true;
            //NeedDraw = true;
            myGame.NeedUpdate = !isPause;
            myGame.NeedDraw = true;
            if (_GLOBAL.InputHandler.isKeyPause())
                pause(myGame);
            if (_GLOBAL.InputHandler.isKeyEscape())
                return2Menu(myGame);
        }

        internal override void Draw(GameTime gameTime)
        {
            data.Draw(gameTime);
            _GLOBAL.SpriteBatch.Begin(SpriteBlendMode.AlphaBlend);
            if (isPause)
            {
                _GLOBAL.SpriteBatch.Draw(maskTexture, _GLOBAL.ViewportRect, Color.White);
                _GLOBAL.SpriteBatch.DrawString(huakanFont, "Paused", pausePos, Color.Red, 0.0f,
                    Vector2.Zero, 2.0f + (float)Math.Cos(fontEffect += 0.075f) / 12, SpriteEffects.None, 0.0f);
            }
            _GLOBAL.SpriteBatch.End();
        }
        #endregion

        #region Methods
        private void pause(GameState srcGame)
        {
            //srcGame.NeedUpdate = !srcGame.NeedUpdate;
            //srcGame.NeedDraw = !srcGame.NeedDraw;
            isPause = !isPause;
        }

        private void return2Menu(GameState theGame)
        {
            //Initialized = false;
            NeedDraw = false;
            NeedUpdate = false;
            _GLOBAL.inGameState = false;
            //gameStateManager.gameStates[0].NeedUpdate = true;

            isPause = false;
            theGame.NeedDraw = false;
            theGame.NeedUpdate = false;
            //theGame.UnloadContent();
        }
        #endregion
    }
}