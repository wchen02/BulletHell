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

namespace BulletHell.Game.Object
{
    public class EnemyPlaneObj : PlaneObj
    {
        public EnemyPlaneObj(Texture2D sprite, int x, int y, int vx, int vy, int srcRectWidth, int srcRectHeight, double moveRate, int hp)
            : base(sprite, x, y, vx, vy, srcRectWidth, srcRectHeight, moveRate, hp) { }

        internal override void Update(GameTime gameTime)
        {
            if (collision(SharedData.hero) && SharedData.hero.visibility)
            {
                SharedData.hero.hp -= 5;
                hp -= 5;
            }

            if (
                //position.X < _GLOBAL.Viewport.X - Width ||
                //position.X > _GLOBAL.Viewport.Width - 2 * Width ||
                //position.Y < _GLOBAL.Viewport.Y - Height ||
                position.Y > _GLOBAL.Viewport.Height + 2 * Height
                )
                visibility = false;

            base.Update(gameTime);
        }

        public override void shoot(int index, int amount, float separation)
        {
            float x, y, angle;
            EnemyBulletObj tmp;

            for (int i = 0; i < amount; ++i)
            {
                if (i < amount / 2)
                    x = position.X + separation * i - SharedData.hero.position.X;
                else
                    x = position.X - separation * (i - amount / 2) - SharedData.hero.position.X;
                y = position.Y - SharedData.hero.position.Y;
                angle = (float)Math.Atan2(y, x);

                tmp = new EnemyBulletObj(bulletType[index].sprite,
                    position.X + Width / 2 - bulletType[index].Width / 2,
                    position.Y + Height / 2 - bulletType[index].Height / 2,
                    bulletType[index].velocity.X,
                    bulletType[index].velocity.Y,
                    null,
                    bulletType[index].killable,
                    bulletType[index].bounceable,
                    -(float)Math.Cos(angle),
                    -(float)Math.Sin(angle));
                SharedData.enemyBullets.Add(tmp);
            }
        }
    }
}
