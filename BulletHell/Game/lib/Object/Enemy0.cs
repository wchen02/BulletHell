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
    public class Enemy0 : EnemyPlaneObj
    {
        float separation = 20.0f;
        int amount = 2;
        TimeEventHandler timer = new TimeEventHandler();

        public Enemy0(int x, int y)
            : base(Stage1.sprites[0], x, y, 2, 2, 32, 32, 15, 50) 
        { }

        internal override void Update(GameTime gameTime)
        {
            timer.Update(gameTime);
            if (timer.elapsedSeconds() >= 1) {
                shoot(0, amount, separation);
                timer.reset();
            }
            move(0, 1, velocity);

            base.Update(gameTime);
        }
    }
}
