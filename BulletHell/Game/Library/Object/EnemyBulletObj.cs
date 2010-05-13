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
    public class EnemyBulletObj : BulletObj
    {
        int hp = 2;
        public EnemyBulletObj(Texture2D sprite, float x, float y, float vx, float vy, SoundEffect hitEffect,
            bool killable, bool bounceable, float xDirection, float yDirection)
            : base(sprite, x, y, vx, vy, hitEffect, killable, bounceable, xDirection, yDirection) { }

        internal override void Update(List<BulletObj> pool, Map map, GameTime gameTime)
        {
            if (collision(map.hero) && map.hero.visibility)
            {
                visibility = false;
                map.hero.hp--;
            }

            /* if the bullet can be killed check to see if the bullet has collide with any ofthe hero's bullet */
            if (killable){
                foreach(BulletObj bullet in map.hero.BulletPool)
                    if (bullet.collision(this) && visibility && bullet.visibility)
                    {
                        hp--;
                        bullet.visibility = false;
                    }
            }
            if (hp <= 0)
                visibility = false;

            base.Update(pool, map, gameTime);
        }

        internal override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
