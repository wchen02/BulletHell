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
//    public class Enemy1 : EnemyPlaneObj
//    {
//        float separation = 50.0f;
//        int amount = 10;
//        TimeEventHandler timer = new TimeEventHandler();

//        public Enemy1(int x, int y)
//            : base(Stage1.sprites[1], x, y, 3, 3, 32, 32, 20, 100)
//        { }

//        internal override void Update(GameTime gameTime)
//        {
//            timer.Update(gameTime);
//            if (timer.elapsedSeconds() >= 2)
//            {
//                shoot(0, amount, separation);
//                timer.reset();
//            }
//            moveCircle(20, 100);
//            //x = a + r*cos(theta), y = b + r*sin(theta);
//            base.Update(gameTime);
//        }
        
//        public void moveCircle(int x, int y) {
//            float xCoor = 0.0f, yCoor = 0.0f;

//            if (xCoor == 0.0f && yCoor == 0.0f) {
//                xCoor = position.X - x;
//                yCoor = position.Y - y;
//            }

//            //angle = (float)Math.Atan2(y, x);
//            //angle = MathHelper.WrapAngle(angle);
////            if (timer.elapsedMilliseconds() > 100) {
//                //angle = MathHelper.WrapAngle(angle);

//                move(5*(float)Math.Cos(timer.elapsedMilliseconds()), 5*(float)Math.Sin(timer.elapsedMilliseconds()), velocity);
////                timer.reset();
////            }
//        }
//    }
//}
