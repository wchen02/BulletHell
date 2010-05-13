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
    public enum BehaviorState
    {
        Shoot,
        ShootCircle,
        ShootStraightAngle,
        ShootStraight,
        ShootTarget,
        Chase,
        Evade,
        SelfDestruct,
        BehaviorController,
        LinearBehavior
    }

    public abstract class Behavior
    {
        protected EnemyPlaneObj parent;
        internal BehaviorState behaviorState;
        internal Random rand;
        public Behavior(EnemyPlaneObj parent)
        {
            this.parent = parent;
            rand = new Random((int)DateTime.Now.Ticks);
        }

        // returns a clone of this object with newParent reference
        public abstract Behavior clone(EnemyPlaneObj newParent);

        /* this method takes in an array of behavior states and checks it against a list of behaviors, the first match behavoir is return */
        internal Behavior firstBehaviorfirstGet(List<Behavior> behaviors, params BehaviorState[] behaviorOrder)
        {
            foreach (BehaviorState bs in behaviorOrder)
                foreach (Behavior b in behaviors)
                    if (b.behaviorState == bs)
                    {
                        // Applies some percentages here to elimate predictability or strike with surprise
                        //if(rand.NextDouble() <= 0.50)
                            return b;
                    }
            return null;
        }

        internal abstract void Update(Map map, GameTime gameTime);
    }
}
