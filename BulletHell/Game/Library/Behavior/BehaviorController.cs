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

/* Behaviors such as Evade, Chase, Self Destruction, ChangingBehavior... are still not implemented */
namespace BulletHell.Game.Library.Behavior
{
    public class BehaviorController : Behavior
    {
        private BehaviorState lastBehavior = BehaviorState.BehaviorController;
        private bool shootable = false,
            shootableCheck = false;

        private float chaseDistance = (_GLOBAL.Viewport.Width + _GLOBAL.Viewport.Height) / 6,
            evadeDistance = (_GLOBAL.Viewport.Width + _GLOBAL.Viewport.Height) / 6,
            selfDestructDistance = (_GLOBAL.Viewport.Width + _GLOBAL.Viewport.Height) / 4,
            hysteresis = 50,
            lastDistance = 0, // the distance computed last update
            distance = 0,
            xDistance = 0, // positive = hero is left of parent
            yDistance = 0; // negative = hero is in front of parent

        private TimeEventHandler timer;

        public BehaviorController(EnemyPlaneObj parent)
            : base(parent)
        {
            behaviorState = BehaviorState.BehaviorController;
            timer = new TimeEventHandler();
        }

        public override Behavior clone(EnemyPlaneObj newParent)
        {
            return new BehaviorController(newParent);
        }

        /* Checks to see if the parent has the ability to shoot */
        private bool canShoot(List<Behavior> behaviors)
        {
            if (!shootableCheck)
                shootable = firstBehaviorfirstGet(behaviors, BehaviorState.ShootCircle,
                    BehaviorState.ShootStraight,
                    BehaviorState.ShootStraightAngle,
                    BehaviorState.ShootTarget) != null;

            return shootable;
        }

        /* checks the condition for a given behavior, easier to manage as these condition are used in multiple places */
        private bool checkBehavior(List<Behavior> behaviors, BehaviorState target)
        {
            switch (target)
            {
                case BehaviorState.SelfDestruct:
                    return (distance < selfDestructDistance);

                case BehaviorState.Shoot:
                    return (parent.Timer.elapsedMilliseconds() > parent.BulletType[parent.bulletSelection].BulletInterval)
                    && canShoot(parent.Behaviors);

                case BehaviorState.Evade:
                    return (distance < evadeDistance - hysteresis);

                case BehaviorState.Chase:
                    return (distance > chaseDistance + hysteresis);
            }

            return false;
        }

        private Behavior routineA()
        {
            List<BehaviorState> behaviors = new List<BehaviorState>();

            if (lastDistance - distance > 0) // hero is coming
            {
                if (checkBehavior(parent.Behaviors, BehaviorState.SelfDestruct))
                    behaviors.Add(BehaviorState.SelfDestruct);

                if (checkBehavior(parent.Behaviors, BehaviorState.Shoot))
                {
                    behaviors.Add(BehaviorState.Shoot);
                    parent.Timer.reset();
                }

                if (checkBehavior(parent.Behaviors, BehaviorState.Evade))
                    behaviors.Add(BehaviorState.Evade);

                if (checkBehavior(parent.Behaviors, BehaviorState.Chase))
                    behaviors.Add(BehaviorState.Chase);

                return firstBehaviorfirstGet(parent.Behaviors, behaviors.ToArray());
            }
            else // hero is leaving
            {
                if (checkBehavior(parent.Behaviors, BehaviorState.SelfDestruct))
                    behaviors.Add(BehaviorState.SelfDestruct);

                if (distance > chaseDistance + hysteresis)
                    behaviors.Add(BehaviorState.Chase);

                if (checkBehavior(parent.Behaviors, BehaviorState.Shoot))
                {
                    behaviors.Add(BehaviorState.Shoot);
                    parent.Timer.reset();
                }

                if (checkBehavior(parent.Behaviors, BehaviorState.Evade))
                    behaviors.Add(BehaviorState.Evade);

                return firstBehaviorfirstGet(parent.Behaviors, behaviors.ToArray());
            }
        }

        private Behavior routineB()
        {
            List<BehaviorState> behaviors = new List<BehaviorState>();

            if (lastDistance - distance > 0) // hero is coming
            {
                if (checkBehavior(parent.Behaviors, BehaviorState.Evade))
                    behaviors.Add(BehaviorState.Evade);

                if (checkBehavior(parent.Behaviors, BehaviorState.Shoot))
                {
                    behaviors.Add(BehaviorState.Shoot);
                    parent.Timer.reset();
                }

                if (checkBehavior(parent.Behaviors, BehaviorState.SelfDestruct))
                    behaviors.Add(BehaviorState.SelfDestruct);

                if (checkBehavior(parent.Behaviors, BehaviorState.Chase))
                    behaviors.Add(BehaviorState.Chase);

                return firstBehaviorfirstGet(parent.Behaviors, behaviors.ToArray());
            }
            else // hero is leaving
            {
                if (checkBehavior(parent.Behaviors, BehaviorState.Chase))
                    behaviors.Add(BehaviorState.Chase);

                if (checkBehavior(parent.Behaviors, BehaviorState.Shoot))
                {
                    behaviors.Add(BehaviorState.Shoot);
                    parent.Timer.reset();
                }

                if (checkBehavior(parent.Behaviors, BehaviorState.SelfDestruct))
                    behaviors.Add(BehaviorState.SelfDestruct);

                if (checkBehavior(parent.Behaviors, BehaviorState.Evade))
                    behaviors.Add(BehaviorState.Evade);

                return firstBehaviorfirstGet(parent.Behaviors, behaviors.ToArray());
            }
        }

        private void controlBehavior(Map map, GameTime gameTime)
        {
            PlaneObj target = map.hero;
            Behavior behavior = null;

            foreach (Behavior b in parent.Behaviors)
            {
                /* If the object has waypoints to go, ignore other behaviors; waypoints are prioritized */
                if (b.behaviorState == BehaviorState.LinearBehavior)
                {
                    if (parent.Path.Count > 0)
                        behavior = b;
                }
            }

            if (behavior == null)
            {
                /* These if-statements can be reduced, but this is more readable. At lease, for me it is. */
                if (yDistance > _GLOBAL.Viewport.Height / 6) // hero is in the back
                {
                    if (xDistance > _GLOBAL.Viewport.Width / 6 || xDistance < -_GLOBAL.Viewport.Width / 6) // hero is left or right of parent
                        behavior = routineB();
                    else // hero is nearby
                        behavior = routineA();
                }
                else if (yDistance <= _GLOBAL.Viewport.Height / 6 && yDistance >= -_GLOBAL.Viewport.Height / 6) // hero is nearby
                    behavior = routineA();
                else if (yDistance < -_GLOBAL.Viewport.Height / 6) // hero is in the front
                    behavior = routineB();
            }
            else 
            {
                /* if the hero has waypoints to move, but it can shoot as well. Shooting is prioritized */
                if (parent.Timer.elapsedMilliseconds() > parent.BulletType[parent.bulletSelection].BulletInterval)
                {
                    foreach (Behavior b in parent.Behaviors)
                        if (b.behaviorState == BehaviorState.Shoot && canShoot(parent.Behaviors))
                            behavior = b;

                    parent.Timer.reset();
                }
            }

            if (behavior != null)
            {
                if (_GLOBAL.Debug)
                    Console.WriteLine(behavior.ToString());

                behavior.Update(map, gameTime);
                lastBehavior = behavior.behaviorState;
            }
        }

        internal override void Update(Map map, GameTime gameTime)
        {
            timer.Update(gameTime);
            if (timer.elapsedMilliseconds() > 50)
            {
                distance = Vector2.Distance(parent.position, map.hero.position);
                xDistance = parent.X - map.hero.X;
                yDistance = parent.Y - map.hero.Y;

                controlBehavior(map, gameTime);

                timer.reset();
                lastDistance = distance;
            }
        }
    }
}