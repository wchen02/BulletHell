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
    public class ShootableObj : Obj
    {
        protected List<BulletObj> BulletPool;
        protected int angle;

        public ShootableObj(Texture2D sprite, int x, int y)
            : base(sprite, x, y)
        {
            BulletPool = new List<BulletObj>();
            angle = 0;
        }

        public void shoot(BulletObj bullet, int angle)
        {
        }

        /*public void updateSprite(GameTime gameTime) { 
            gameTime.
        }*/
    }
}
