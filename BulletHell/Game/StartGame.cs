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
        private Texture2D maskTexture;
        private GameState myGame;
        private bool isPause = false;
        private Vector2 pausePos;
        private float fontEffect = 0.0f;
        #endregion

        #region Initialization
        public StartGame()
        { }

        protected override void Initialize()
        {
            //data = new SharedData();
            pausePos = new Vector2(_GLOBAL.Viewport.Width / 2 - 100, _GLOBAL.Viewport.Height / 2 - 50);
            myGame = new MapLoader(new MapXml()).loadMap("stage1.xml");

            _GLOBAL.GameStateManager.activate(myGame);
        }
        #endregion

        #region Contents
        internal override void LoadContent()
        {
            maskTexture = _GLOBAL.ContentManager.Load<Texture2D>("sprites//dot");
        }

        protected override void UnloadContent() 
        {
            _GLOBAL.GameStateManager.GameStates.Remove(myGame);
        }
        #endregion

        #region Update and Draw
        internal override void Update(GameTime gameTime)
        {
            _GLOBAL.InputHandler.keyboardState = Keyboard.GetState();

            NeedUpdate = true;
            NeedDraw = true;
            if (_GLOBAL.InputHandler.isKeyPause())
                pause(myGame);

            if (_GLOBAL.InputHandler.isKeyEscape())
                return2Menu(myGame);
            
            if (_GLOBAL.InputHandler.keyboardState.IsKeyDown(Keys.R))
            {
                _GLOBAL.GameStateManager.GameStates.Remove(myGame);
                myGame = new MapLoader(new MapXml()).loadMap("stage1.xml");
                _GLOBAL.GameStateManager.activate(myGame);
            }

            _GLOBAL.InputHandler.previousKeyboardState = _GLOBAL.InputHandler.keyboardState;
        }

        internal override void Draw(GameTime gameTime)
        {
            //data.Draw(gameTime);
            _GLOBAL.SpriteBatch.Begin(SpriteBlendMode.AlphaBlend);
            if (isPause)
            {
                _GLOBAL.SpriteBatch.Draw(maskTexture, _GLOBAL.ViewportRect, Color.White);
                _GLOBAL.SpriteBatch.DrawString(_GLOBAL.HuakanFont, "Paused", pausePos, Color.Red, 0.0f,
                    Vector2.Zero, 2.0f + (float)Math.Cos(fontEffect += 0.075f) / 12, SpriteEffects.None, 0.0f);
            }
            _GLOBAL.SpriteBatch.End();
        }
        #endregion

        #region Methods
        private void pause(GameState srcGame)
        {
            isPause = !isPause;
            _GLOBAL.GameStateManager.setUpdate(myGame, !isPause);
        }

        private void return2Menu(GameState theGame)
        {
            _GLOBAL.inGameState = false;

            UnloadContent();
            _GLOBAL.GameStateManager.GameStates.Remove(this);
        }
        #endregion
    }
}