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

namespace BulletHell.Game.Library.Behavior
{
    public class ShootingStraightAngleBehavior : Behavior
    {
        private double x;
        private double y;

        private float targetAngle;

        public ShootingStraightAngleBehavior(EnemyPlaneObj parent)
            : base(parent)
        {
            behaviorState = BehaviorState.ShootStraightAngle;
        }

        public override Behavior clone(EnemyPlaneObj newParent)
        {
            return new ShootingStraightAngleBehavior(newParent);
        }

        internal override void Update(Map map, GameTime gameTime)
        {
            x = map.hero.position.X - parent.position.X;
            y = map.hero.position.Y - parent.position.Y;

            targetAngle = (float)(Math.PI * 2 - Math.Atan2(y, x));
            parent.shootStraight(parent.bulletSelection, parent.BulletType[parent.bulletSelection].BulletAmount,

            /* Lerp adds unpredictability to the bullet */
            MathHelper.Lerp(parent.BulletType[parent.bulletSelection].BulletDensity * 0.9f,
            parent.BulletType[parent.bulletSelection].BulletDensity * 1.1f,
            (float)rand.NextDouble()) * 5,

            /* Lerp adds unpredictability to the bullet */
            MathHelper.Lerp(parent.BulletType[parent.bulletSelection].BulletDensity * 0.9f,
            parent.BulletType[parent.bulletSelection].BulletDensity * 1.1f,
            (float)rand.NextDouble()) * 3, targetAngle);
        }
    }
}