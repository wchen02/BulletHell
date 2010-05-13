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
        public HeroPlaneObj(Texture2D sprite, int x, int y, int vx, int vy, int srcRectWidth, int srcRectHeight, double moveRate, int hp)
            : base(sprite, x, y, vx, vy, srcRectWidth, srcRectHeight, moveRate, hp){}

        internal override void Update(Map map, GameTime gameTime)
        {
            _GLOBAL.InputHandler.keyboardState = Keyboard.GetState();

            if (_GLOBAL.InputHandler.keyboardState.IsKeyDown(Keys.D1) && _GLOBAL.InputHandler.previousKeyboardState.IsKeyUp(Keys.D1))
                bulletSelection = 0;

            else if (_GLOBAL.InputHandler.keyboardState.IsKeyDown(Keys.D2) && _GLOBAL.InputHandler.previousKeyboardState.IsKeyUp(Keys.D2))
                bulletSelection = 1;

            if (_GLOBAL.InputHandler.isKeyUpPressed())
                move(0, -1, velocity);

            if (_GLOBAL.InputHandler.isKeyDownPressed())
                move(0, 1, velocity);

            if (_GLOBAL.InputHandler.isKeyLeftPressed())
                move(-1, 0, velocity);

            if (_GLOBAL.InputHandler.isKeyRightPressed())
                move(1, 0, velocity);

            /* The hero plane object does not use a behavior model, so this has to be hand-coded */
            if (_GLOBAL.InputHandler.isKeyEnterPressed())
            {
                if (Timer.elapsedMilliseconds() > BulletType[bulletSelection].BulletInterval)
                {
                    shootStraight(bulletSelection, BulletType[bulletSelection].BulletAmount, BulletType[bulletSelection].BulletDensity, 3, MathHelper.Pi / 2);
                    Timer.reset();
                }
            }

            if (position.X < _GLOBAL.Viewport.X)
                position.X = 0;

            else if (position.X > _GLOBAL.Viewport.Width - Width)
                position.X = _GLOBAL.Viewport.Width - Width;

            if (position.Y < _GLOBAL.Viewport.Y)
                position.Y = 0;

            else if (position.Y > _GLOBAL.Viewport.Height - Height)
                position.Y = _GLOBAL.Viewport.Height - Height; ;

            _GLOBAL.InputHandler.previousKeyboardState = _GLOBAL.InputHandler.keyboardState;

            base.Update(map, gameTime);
        }

        internal override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        /* Adds a hero bullet object to the pool given the args */
        public override void addBulletPool(int index, float x, float y, double xDirection, double yDirection)
        {
            HeroBulletObj bullet = new HeroBulletObj(BulletType[index].sprite, x, y,
                -BulletType[index].velocity.X, -BulletType[index].velocity.Y, BulletType[index].hitEffect,
                BulletType[index].killable, BulletType[index].bounceable, (float)xDirection, (float)yDirection);

            BulletPool.Add(bullet);
        }
    }
}
