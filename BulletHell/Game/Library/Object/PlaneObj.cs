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
    /* This class was mean't to be created as an abstract class, however, due to the need of base classes for maps, this is changed to a normal class */
    public class PlaneObj : MoveableSpriteObj
    {
        /* The types of bullet this plane is allowed to shoot */
        public List<BulletObj> BulletType { get; private set; }
        public TimeEventHandler Timer { get; private set; }

        /* The bullets that were projected */
        public List<BulletObj> BulletPool { get; private set; }
        internal int hp;

        /* The current selection of which bullet in the BulletType to shoot; BulletType[bulletSelection] */
        internal int bulletSelection;

        #region Initialization
        public PlaneObj(Texture2D sprite, int x, int y, int vx, int vy, int srcRectWidth, int srcRectHeight, double moveRate, int hp)
            : base(sprite, x, y, vx, vy, srcRectHeight, srcRectHeight, moveRate)
        {
            BulletType = new List<BulletObj>();
            BulletPool = new List<BulletObj>();
            Timer = new TimeEventHandler();

            this.hp = hp;
            bulletSelection = 0;
        }

        internal virtual void Update(Map map, GameTime gameTime)
        {
            Timer.Update(gameTime);
            if (!visibility)
            {
                //map.toBeRemoved.Add(ID); // remove the plane if it's invisible
                map.enemiessToBeUpdated.Remove(this);
                map.bulletPool.AddRange(BulletPool); // this adds all the BulletObjs in BulletPool to the map.bulletPool
            }

            if (hp <= 0)
                visibility = false;

            /* Calling update on all of it's bullets, foreach will cause a problem if the item is removed */
            for (int i = BulletPool.Count - 1; i >= 0; --i)
                BulletPool[i].Update(BulletPool, map, gameTime);

            map.MSG2 += "\n" + ID + ".Bulelts: " + BulletPool.Count;

            base.Update(gameTime);
        }

        internal override void Draw(GameTime gameTime)
        {
            foreach (BulletObj bullet in BulletPool)
                bullet.Draw(gameTime);
            base.Draw(gameTime);
        }

        public void addBulletType(BulletObj bullet)
        {
            BulletType.Add(bullet);
            bullet.visibility = false;
        }

        public virtual void addBulletPool(int index, float x, float y, double xDirection, double yDirection) { }
        
        public void shoot(int index, float angle)
        {
            shoot(index, position.X + Width / 2 - BulletType[index].Width / 2, position.Y + Height / 2 - BulletType[index].Height / 2, angle);
        }

        public void shoot(int index, float x, float y, float angle)
        {
            if (index >= BulletType.Count) return;
            addBulletPool(index, x, y, -Math.Cos(angle), Math.Sin(angle));
        }

        public void shootCircle(int index, float separationAngle)
        {
            for (int i = 0; i < MathHelper.Pi * 2 / separationAngle; ++i)
                shoot(index, i * separationAngle);
        }

        public void shootMulti(int index, int amount, float separationAngle, float angle)
        {
            if (amount % 2 == 0)
            { // even 
                for (float i = 0.5f; i < amount / 2; ++i)
                {
                    shoot(index, angle + i * separationAngle);
                    shoot(index, angle - i * separationAngle);
                }
            }
            else
            { // odd
                shoot(index, angle);
                for (float i = 1; i <= amount / 2; ++i)
                {
                    shoot(index, angle + i * separationAngle);
                    shoot(index, angle - i * separationAngle);
                }
            }
        }

        public void shootStraight(int index, int amount, float separationX, float separationY, float angle)
        {
            float x = position.X + Width / 2 - BulletType[index].Width / 2;
            float y = position.Y + Height / 2 - BulletType[index].Height / 2;

            if (amount % 2 == 0)
            { // even 
                for (float i = 0.5f; i < amount / 2; ++i)
                {
                    shoot(index, x + i * separationX, y + i * separationY, angle);
                    shoot(index, x - i * separationX, y + i * separationY, angle);
                }
            }
            else
            { // odd
                shoot(index, angle);
                for (float i = 1; i <= amount / 2; ++i)
                {
                    shoot(index, x + i * separationX, y + i * separationY, angle);
                    shoot(index, x - i * separationX, y + i * separationY, angle);
                }
            }
        }
        #endregion
    }
}
