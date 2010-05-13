//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Audio;
//using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.Media;
//using Microsoft.Xna.Framework.Net;
//using Microsoft.Xna.Framework.Storage;
//using BulletHell.System;
//using BulletHell.Game;
//using BulletHell.Game.Object;
//using BulletHell.Game.Interface;

//namespace BulletHell.Game.Object
//{
//    public class Enemy2 : MassShootingPlaneObj
//    {
//        float separation = 15.0f;
//        int amount = 75;
//        int direction = 1;
//        TimeEventHandler timer = new TimeEventHandler();

//        public Enemy2(int x, int y)
//            : base(Stage1.sprites[2], x, y, 1, 1, 32, 32, 15, 200)
//        { }

//        internal override void Update(GameTime gameTime)
//        {
//            timer.Update(gameTime);
//            if (timer.elapsedSeconds() >= 3)
//            {
//                shoot(0, amount, separation);
//                timer.reset();
//            }

//            move(1.0f * direction, 0, velocity);
//            if (position.X > _GLOBAL.Viewport.Width - Width)
//                direction = -direction;
//            else if (position.X < _GLOBAL.Viewport.X)
//                direction = -direction;


//            base.Update(gameTime);
//        }
//    }
//}
