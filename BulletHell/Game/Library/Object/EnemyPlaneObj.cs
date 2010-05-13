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
using BulletHell.Game.Library.Behavior;
using BulletHell.Game.Library;
using BulletHell.System;
using BulletHell.Game;
using BulletHell.Game.Object;
using BulletHell.Game.Interface;

namespace BulletHell.Game.Object
{
    public class EnemyPlaneObj : PlaneObj
    {
        internal Behavior currentBehavior = null;
        internal List<Behavior> Behaviors { get; private set; }
        internal Path Path { get; private set; }

        public EnemyPlaneObj(Texture2D sprite, int x, int y, int vx, int vy, int srcRectWidth, int srcRectHeight, double moveRate, int hp, Path enemyPath)
            : base(sprite, x, y, vx, vy, srcRectWidth, srcRectHeight, moveRate, hp)
        {
            Behaviors = new List<Behavior>();
            Path = enemyPath;
        }

        /* Doesn't look like it, but this is a Copy Constructor */
        public EnemyPlaneObj(EnemyPlaneObj rhs)
            : this(rhs.sprite, (int)rhs.position.X, (int)rhs.position.Y, (int)rhs.velocity.X, (int)rhs.velocity.Y, 
            rhs.srcRect.Width, rhs.srcRect.Height, rhs.MoveRate, rhs.hp, 
            (rhs.Path != null) ? rhs.Path.createMirror(false, false) : null)
        {
            foreach(Behavior behavior in rhs.Behaviors)
                Behaviors.Add(behavior.clone(this));

            currentBehavior = rhs.currentBehavior.clone(this);

            foreach(BulletObj bulletObj in rhs.BulletType)
                BulletType.Add(bulletObj);
        }

        internal override void Update(Map map, GameTime gameTime)
        {
            if (collision(map.hero) && map.hero.visibility)
            {
                map.hero.hp -= 5;
                hp -= 5;
            }

            /* If the object is out of visible or allowed viewport, it is removed */
            if ((position.Y > _GLOBAL.Viewport.Height + 200 || position.Y < _GLOBAL.Viewport.Y - 200
                || position.X > _GLOBAL.Viewport.Width + 200 || position.X < _GLOBAL.Viewport.X - 200))
                visibility = false;

            currentBehavior.Update(map, gameTime);

            base.Update(map, gameTime);
        }

        internal override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        /* Adds an enemy bullet object to the pool given the args */
        public override void addBulletPool(int index, float x, float y, double xDirection, double yDirection)
        {
            EnemyBulletObj bullet = new EnemyBulletObj(BulletType[index].sprite, x, y,
                -BulletType[index].velocity.X, -BulletType[index].velocity.Y, BulletType[index].hitEffect,
                BulletType[index].killable, BulletType[index].bounceable, (float)xDirection, (float)yDirection);

            BulletPool.Add(bullet);
        }
    }
}
