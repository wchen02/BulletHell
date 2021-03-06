﻿using System;
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
    public class Sample1 : GameState
    {
        private KeyboardState currentKey, prevKey;

        protected override void Initialize() { }

        internal override void LoadContent() { }

        protected override void UnloadContent() { }

        internal override void Update(GameTime gameTime)
        {
            NeedDraw = true;
            NeedUpdate = true;
            currentKey = Keyboard.GetState();
            if (currentKey.IsKeyDown(Keys.D2) && prevKey.IsKeyUp(Keys.D2))
            {
                _GLOBAL.GameStateManager.activate(new Sample2());
                NeedDraw = false;
                NeedUpdate = false;
            }
            else if (currentKey.IsKeyDown(Keys.D3) && prevKey.IsKeyUp(Keys.D3))
            {
                _GLOBAL.GameStateManager.activate(new Sample());
                NeedDraw = false;
                NeedUpdate = false;
            }

            prevKey = currentKey;
        }

        internal override void Draw(GameTime gameTime)
        {
            _GLOBAL.SpriteBatch.Begin(SpriteBlendMode.AlphaBlend);
            _GLOBAL.SpriteBatch.DrawString(_GLOBAL.SpriteFont, " In Sample1 ", new Vector2(400, 200), Color.White);
            _GLOBAL.SpriteBatch.End();
        }
    }
}