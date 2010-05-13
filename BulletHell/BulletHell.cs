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
using BulletHell.Game.Library.xml;

namespace BulletHell
{
    public class BulletHell : Microsoft.Xna.Framework.Game
    {
        private MouseState mouseState;
        private Obj cursor;
        private Texture2D cursorTexture;

        #region Class
        public BulletHell()
        {
            _GLOBAL.Graphics = new GraphicsDeviceManager(this);
            _GLOBAL.ContentManager = new ContentManager(Services, "Content");
            _GLOBAL.GameStateManager = new GameStateManager(this);
            Components.Add(_GLOBAL.GameStateManager);
        }

        /* Global Variables initialization */
        protected override void Initialize()
        {
            //graphicMode(1440, 900, true);

            _GLOBAL.InputHandler = new InputHandler();
            _GLOBAL.Viewport = _GLOBAL.Graphics.GraphicsDevice.Viewport;
            _GLOBAL.ViewportRect = new Rectangle(_GLOBAL.Viewport.X, _GLOBAL.Viewport.Y, _GLOBAL.Viewport.Width, _GLOBAL.Viewport.Height);
            _GLOBAL.SpriteBatch = new SpriteBatch(GraphicsDevice);
            _GLOBAL.AudioEngine = new AudioEngine(@"Content\audio\audioEngine.xgs");
            _GLOBAL.WaveBank = new WaveBank(_GLOBAL.AudioEngine, @"Content\audio\waveBank.xwb");
            _GLOBAL.SoundBank = new SoundBank(_GLOBAL.AudioEngine, @"Content\audio\soundBank.xsb");

            base.Initialize();
            cursor = new Obj(cursorTexture, 0, 0);

            _GLOBAL.GameStateManager.activate(new Opening());
        }

        protected override void LoadContent()
        {
            cursorTexture = _GLOBAL.ContentManager.Load<Texture2D>(@"cursor\normal");
            _GLOBAL.SpriteFont = _GLOBAL.ContentManager.Load<SpriteFont>(@"fonts\gamefont");
            _GLOBAL.HuakanFont = _GLOBAL.ContentManager.Load<SpriteFont>(@"fonts\DFShaoNvW5-GB");
        }

        protected override void UnloadContent()
        {
        }

        /* updates mouse, Audio Engine and inputs */
        protected override void Update(GameTime gameTime)
        {
            if(_GLOBAL.Debug)
                mouse();
            
            _GLOBAL.AudioEngine.Update();
            
            if (_GLOBAL.Quit)
                Exit();
            
            _GLOBAL.InputHandler.Update(gameTime);
            
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            base.Draw(gameTime);
            if (_GLOBAL.Debug)
                drawCoor();
        }

        private void mouse()
        {
            mouseState = Mouse.GetState();
            cursor.position.X = mouseState.X;
            cursor.position.Y = mouseState.Y;
        }

        /* draw the mouse cursor and (x, y) coordinate of it */
        private void drawCoor()
        {
            _GLOBAL.SpriteBatch.Begin();
            _GLOBAL.SpriteBatch.DrawString(_GLOBAL.SpriteFont, "(" + mouseState.X + ", " + mouseState.Y + ")", new Vector2(10, 10), Color.Blue);
            _GLOBAL.SpriteBatch.Draw(cursor.sprite, cursor.position, Color.White);
            _GLOBAL.SpriteBatch.End();
        }

        /* this method sets the screen to the resolution given. If the resolution given is in the display mode, accept the changes.
         * Return false otherwise. */
        internal bool graphicMode(int width, int height, bool fullScreen)
        {
            if (fullScreen == false)
            {
                if ((width <= GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width)
                    && (height <= GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height))
                {
                    _GLOBAL.Graphics.PreferredBackBufferWidth = width;
                    _GLOBAL.Graphics.PreferredBackBufferHeight = height;
                    _GLOBAL.Graphics.IsFullScreen = fullScreen;
                    _GLOBAL.Graphics.ApplyChanges();
                    return true;
                }
            }
            else
            {
                foreach (DisplayMode resolution in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
                {
                    if ((resolution.Width == width) && (resolution.Height == height))
                    {
                        _GLOBAL.Graphics.PreferredBackBufferWidth = width;
                        _GLOBAL.Graphics.PreferredBackBufferHeight = height;
                        _GLOBAL.Graphics.IsFullScreen = fullScreen;
                        _GLOBAL.Graphics.ApplyChanges();
                        return true;
                    }
                }
            }
            return false;
        }

        #endregion
    }

    #region Entry Point
    static class test
    {
        static void Main()
        {
            using (BulletHell game = new BulletHell())
            {
                game.Run();
            }
        }
    }
    #endregion
}
