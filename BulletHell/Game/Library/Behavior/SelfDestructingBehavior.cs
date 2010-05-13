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
    /* Self Destruction is essentially chase behavior with a different condition */
    public class SelfDestructingBehavior : Behavior
    {
        private ChasingBehavior behavior;
        public SelfDestructingBehavior(EnemyPlaneObj parent)
            : base(parent)
        {
            behavior = new ChasingBehavior(parent);
            behaviorState = BehaviorState.SelfDestruct;
        }

        public override Behavior clone(EnemyPlaneObj newParent)
        {
            return new SelfDestructingBehavior(newParent);
        }

        internal override void Update(Map map, GameTime gameTime)
        {
            behavior.Update(map, gameTime);
        }
    }
}