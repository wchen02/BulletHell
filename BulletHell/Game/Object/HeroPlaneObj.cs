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

namespace BulletHell.Game.Object
{
    public class HeroPlaneObj : PlaneObj
    {
        TimeEventHandler timer;
        public HeroPlaneObj(Texture2D sprite, int x, int y, int vx, int vy, int srcRectWidth, int srcRectHeight, double moveRate, int hp)
            : base(sprite, x, y, vx, vy, srcRectWidth, srcRectHeight, moveRate, hp) 
        {
                timer = new TimeEventHandler();
        }

        internal override void Update(GameTime gameTime)
        {
            timer.Update(gameTime);
            if (_GLOBAL.InputHandler.keyboardState.IsKeyDown(Keys.D1))
                bulletSelection = 0;

            else if (_GLOBAL.InputHandler.keyboardState.IsKeyDown(Keys.D2))
                bulletSelection = 1;

            if (_GLOBAL.InputHandler.isKeyUpPressed())
                move(0, -1, velocity);

            if (_GLOBAL.InputHandler.isKeyDownPressed())
                move(0, 1, velocity);

            if (_GLOBAL.InputHandler.isKeyLeftPressed())
                move(-1, 0, velocity);

            if (_GLOBAL.InputHandler.isKeyRightPressed())
                move(1, 0, velocity);

            if (_GLOBAL.InputHandler.isKeyEnterPressed())
            {
                if (timer.elapsedMilliseconds() > 25)
                {
                    shoot(bulletSelection, 10, 10.0f);
                    timer.reset();
                }
            }

            if (_GLOBAL.InputHandler.keyboardState.IsKeyDown(Keys.C))
                SharedData.enemyBullets.Clear();

            if (position.X < _GLOBAL.Viewport.X)
                position.X = 0;

            else if (position.X > _GLOBAL.Viewport.Width - Width)
                position.X = _GLOBAL.Viewport.Width - Width;

            if (position.Y < _GLOBAL.Viewport.Y)
                position.Y = 0;

            else if (position.Y > _GLOBAL.Viewport.Height - Height)
                position.Y = _GLOBAL.Viewport.Height - Height; ;

            base.Update(gameTime);
        }

        public override void shoot(int index, int amount, float separation)
        {
            float x, y, angle;
            HeroBulletObj tmp;
            int half = amount / 2;
            for (int i = 0; i < amount; ++i)
            {

                if (i < half)
                    x = separation * i;
                else
                    x = -separation * (i - amount / 2);
                y = 0;
                angle = i * (200 / amount) / 0.02f;
                tmp = new HeroBulletObj(bulletType[index].sprite,
                    position.X + Width / 2 - bulletType[index].Width / 2 + x,
                    position.Y - bulletType[index].Height + y,
                    bulletType[index].velocity.X,
                    bulletType[index].velocity.Y,
                    null,
                    bulletType[index].killable,
                    bulletType[index].bounceable,
                    0,
                    -1.0f);
                SharedData.heroBullets.Add(tmp);
            }
        }
    }
}
