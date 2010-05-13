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
    public class ChasingBehavior : Behavior
    {
        private float xDistance = 0; // positive = hero is left of parent
        private float yDistance = 0; // negative = hero is in front of parent

        public ChasingBehavior(EnemyPlaneObj parent)
            : base(parent)
        {
            behaviorState = BehaviorState.Chase;
        }

        public override Behavior clone(EnemyPlaneObj newParent)
        {
            return new ChasingBehavior(newParent);
        }

        internal override void Update(Map map, GameTime gameTime)
        {
            xDistance = (parent.X - map.hero.X);
            yDistance = (parent.Y - map.hero.Y);

            /* Chases after the target */
            if (Math.Abs(xDistance) <= 2)
                parent.move(0, (yDistance >= 0) ? -1 : 1, parent.velocity);
            else if (Math.Abs(yDistance) <= 2)
                parent.move((xDistance >= 0) ? -1 : 1, 0, parent.velocity);
            else
                parent.move((xDistance >= 0) ? -1 : 1, (yDistance >= 0) ? -1 : 1, parent.velocity);
        }
    }
}