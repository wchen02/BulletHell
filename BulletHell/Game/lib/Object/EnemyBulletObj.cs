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
        int hp = 5;
        public EnemyBulletObj(Texture2D sprite, float x, float y, float vx, float vy, SoundEffect hitEffect,
            bool killable, bool bounceable, float xDirection, float yDirection)
            : base(sprite, x, y, vx, vy, hitEffect, killable, bounceable, xDirection, yDirection) { }

        internal override void Update(GameTime gameTime)
        {
            if (collision(SharedData.hero) && SharedData.hero.visibility)
            {
                visibility = false;
                SharedData.hero.hp--;
            }

            if (killable){
                foreach(BulletObj bullet in SharedData.heroBullets)
                    if (bullet.collision(this) && visibility && bullet.visibility)
                    {
                        hp--;
                        bullet.visibility = false;
                    }
            }
            if (hp <= 0)
                visibility = false;

            base.Update(gameTime);
        }
    }
}
