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
    public class HeroBulletObj : BulletObj
    {
        public HeroBulletObj(Texture2D sprite, float x, float y, float vx, float vy, SoundEffect hitEffect,
            bool killable, bool bounceable, float xDirection, float yDirection)
            : base(sprite, x, y, vx, vy, hitEffect, killable, bounceable, xDirection, yDirection) { }


        internal override void Update(GameTime gameTime)
        {
            foreach (PlaneObj enemy in SharedData.enemies)
            {
                if (collision(enemy))
                {
                    visibility = false;
                    enemy.hp--;
                }
            }
            // if killable, collision detection
            base.Update(gameTime);
        }
    }
}
