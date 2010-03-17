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
    public abstract class PlaneObj:MoveableSpriteObj, Interface.IShootable
    {
        protected List<BulletObj> bulletType;
        internal int hp;
        internal int bulletSelection;

        #region Initialization
        public PlaneObj(Texture2D sprite, int x, int y, int vx, int vy, int srcRectWidth, int srcRectHeight, double moveRate, int hp)
            : base(sprite, x, y, vx, vy, srcRectHeight, srcRectHeight, moveRate)
        {
            bulletType = new List<BulletObj>();
            this.hp = hp;
            bulletSelection = 0;
        }

        internal override void Update(GameTime gameTime) {
            if (hp <= 0)
                visibility = false;
            base.Update(gameTime);
        }

        internal override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        public void addBullet(BulletObj bullet)
        {
            bulletType.Add(bullet);
            bullet.visibility = false;
        }

        public abstract void shoot(int index, int amount, float separation);
        #endregion
    }
}
