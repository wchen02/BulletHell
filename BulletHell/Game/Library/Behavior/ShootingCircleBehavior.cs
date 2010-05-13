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
    public class ShootingCircleBehavior : Behavior
    {
        public ShootingCircleBehavior(EnemyPlaneObj parent)
            : base(parent)
        {
            behaviorState = BehaviorState.ShootCircle;
        }

        public override Behavior clone(EnemyPlaneObj newParent)
        {
            return new ShootingCircleBehavior(newParent);
        }

        internal override void Update(Map map, GameTime gameTime)
        {
            parent.shootCircle(parent.bulletSelection, MathHelper.Pi / 180 * parent.BulletType[parent.bulletSelection].BulletDensity);
        }
    }
}