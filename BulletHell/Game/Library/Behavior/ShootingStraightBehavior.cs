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
    public class ShootingStraightBehavior : Behavior
    {
        public ShootingStraightBehavior(EnemyPlaneObj parent)
            : base(parent)
        {
            behaviorState = BehaviorState.ShootStraight;
        }

        public override Behavior clone(EnemyPlaneObj newParent)
        {
            return new ShootingStraightBehavior(newParent);
        }

        internal override void Update(Map map, GameTime gameTime)
        {
            parent.shootStraight(parent.bulletSelection, parent.BulletType[parent.bulletSelection].BulletAmount,
                //MathHelper.Lerp(parent.BulletType[parent.bulletSelection].BulletDensity * 0.75f, 
                //parent.BulletType[parent.bulletSelection].BulletDensity * 1.25f,
                //(float)rand.NextDouble()) * 5,
                parent.BulletType[parent.bulletSelection].BulletDensity * 5,
                //MathHelper.Lerp(parent.BulletType[parent.bulletSelection].BulletDensity * 0.9f,
                //parent.BulletType[parent.bulletSelection].BulletDensity * 1.1f,
                //(float)rand.NextDouble() * 3),
                parent.BulletType[parent.bulletSelection].BulletDensity * 3,

                MathHelper.Pi / 2 * 3);
        }
    }
}