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
    public class MassShootingPlaneObj : EnemyPlaneObj
    {
        public MassShootingPlaneObj(Texture2D sprite, int x, int y, int vx, int vy, int srcRectWidth, int srcRectHeight, double moveRate, int hp)
            : base(sprite, x, y, vx, vy, srcRectWidth, srcRectHeight, moveRate, hp) { }
       
        public override void shoot(int index, int amount, float separation)
        {
            float x, y, angle;
            EnemyBulletObj tmp;

            for (int i = 0; i < amount; ++i)
            {
                if (i < amount / 2)
                    x = separation * i;
                else
                    x = -separation * (i - amount / 2);
                y = _GLOBAL.Viewport.Height;
                angle = i*(200/amount)/0.02f;
                tmp = new EnemyBulletObj(bulletType[index].sprite,
                    position.X + Width / 2 - bulletType[index].Width / 2,
                    position.Y - bulletType[index].Height,
                    bulletType[index].velocity.X,
                    bulletType[index].velocity.Y,
                    null,
                    bulletType[index].killable,
                    bulletType[index].bounceable,
                    -(float)Math.Cos(angle),
                    -(float)Math.Sin(angle));
                SharedData.enemyBullets.Add(tmp);
                tmp = new EnemyBulletObj(bulletType[index].sprite,
                    position.X + Width / 2 - bulletType[index].Width / 2,
                    position.Y - bulletType[index].Height,
                    bulletType[index].velocity.X,
                    bulletType[index].velocity.Y,
                    null,
                    bulletType[index].killable,
                    bulletType[index].bounceable,
                    (float)Math.Cos(angle),
                    (float)Math.Sin(angle));
                SharedData.enemyBullets.Add(tmp);
            }
        }
    }
}
