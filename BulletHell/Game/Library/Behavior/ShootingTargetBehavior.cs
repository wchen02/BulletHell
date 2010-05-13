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
    public class ShootingTargetBehavior : Behavior
    {
        private double x;
        private double y;

        private float targetAngle;

        public ShootingTargetBehavior(EnemyPlaneObj parent)
            : base(parent)
        {
            behaviorState = BehaviorState.ShootTarget;
        }

        public override Behavior clone(EnemyPlaneObj newParent)
        {
            return new ShootingTargetBehavior(newParent);
        }

        internal override void Update(Map map, GameTime gameTime)
        {
            x = map.hero.position.X - parent.position.X;
            y = map.hero.position.Y - parent.position.Y;

            targetAngle = (float)(Math.PI * 2 - Math.Atan2(y, x));

            if (_GLOBAL.Debug)
            {
                double degree = (((targetAngle) / (Math.PI * 2)) * 360 + 360) % 360;
                map.TargetShooting = "Hero: (" + map.hero.position.X + ", " + map.hero.position.Y +
                    ")\nEnemy: (" + parent.position.X + ", " + parent.position.Y +
                    ")\nDegree: " + degree +
                    "\ntargetAngle: " + targetAngle;
            }

            /* Lerp adds unpredictability to the bullet */
            parent.shootMulti(parent.bulletSelection, parent.BulletType[parent.bulletSelection].BulletAmount,
            MathHelper.Pi / 180 * MathHelper.Lerp(parent.BulletType[parent.bulletSelection].BulletDensity * 0.9f, 
                parent.BulletType[parent.bulletSelection].BulletDensity * 1.1f,
                (float)rand.NextDouble()), targetAngle);
        }
    }
}