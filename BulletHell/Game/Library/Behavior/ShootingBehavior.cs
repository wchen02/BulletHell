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
    public class ShootingBehavior : Behavior
    {
        private float xDistance = 0; // positive = hero is left of parent
        private float yDistance = 0; // negative = hero is in front of parent

        private BehaviorState lastShooting;

        public ShootingBehavior(EnemyPlaneObj parent)
            : base(parent)
        {
            behaviorState = BehaviorState.Shoot;
        }

        public override Behavior clone(EnemyPlaneObj newParent)
        {
            return new ShootingBehavior(newParent);
        }

        private void shoot(Map map, GameTime gameTime)
        {
            PlaneObj target = map.hero;
            Behavior behavior = null;

            /* h = hero; 1 = parent is at this location and uses first the if statement, 2  the second
             * -------------
             * | h | h | h |
             * +---+---+---+
             * | 1 | 2 | 1 |
             * +---+---+---+
             * | 1 | 2 | 1 |
             * -------------
             */
            if (yDistance > _GLOBAL.Viewport.Height / 6) // hero is in the back
            {
                if (xDistance > _GLOBAL.Viewport.Width / 6 || xDistance < -_GLOBAL.Viewport.Width / 3) // hero is left or right
                {
                    behavior = firstBehaviorfirstGet(parent.Behaviors,
                        BehaviorState.ShootCircle,
                        BehaviorState.ShootTarget,
                        BehaviorState.ShootStraightAngle,
                        BehaviorState.ShootStraight);
                    if (_GLOBAL.Debug) Console.WriteLine("top left/right");
                }
                else // hero is in range
                {
                    behavior = firstBehaviorfirstGet(parent.Behaviors,
                        BehaviorState.ShootStraightAngle,
                        BehaviorState.ShootCircle,
                        BehaviorState.ShootTarget,
                        BehaviorState.ShootStraight);
                    if (_GLOBAL.Debug) Console.WriteLine("top mid");
                }
            }

            /* h = hero; 1 = parent is at this location and uses first the if statement, 2  the second
             * -------------
             * |   |   |   |
             * +---+---+---+
             * |h1 |h2 |h1 |
             * +---+---+---+
             * |   |   |   |
             * -------------
             */
            else if (yDistance <= _GLOBAL.Viewport.Height / 3 && yDistance >= -_GLOBAL.Viewport.Height / 6) // hero is within range
            {
                if (xDistance > _GLOBAL.Viewport.Width / 6 || xDistance < -_GLOBAL.Viewport.Width / 6) // hero is left or right
                {
                    behavior = firstBehaviorfirstGet(parent.Behaviors,
                        BehaviorState.ShootTarget,
                        BehaviorState.ShootStraightAngle,
                        BehaviorState.ShootCircle,
                        BehaviorState.ShootStraight);
                    if (_GLOBAL.Debug) Console.WriteLine("mid left/right");
                }
                else
                {
                    behavior = firstBehaviorfirstGet(parent.Behaviors,
                        BehaviorState.ShootCircle,
                        BehaviorState.ShootStraight,
                        BehaviorState.ShootStraightAngle,
                        BehaviorState.ShootTarget);
                    if (_GLOBAL.Debug) Console.WriteLine("mid mid");
                }
            }

            /* h = hero; 1 = parent is at this location and uses first the if statement, 2  the second
             * -------------
             * | 1 | 2 | 1 |
             * +---+---+---+
             * | 1 | 2 | 1 |
             * +---+---+---+
             * | h | h | h |
             * -------------
             */

            else if (yDistance < -_GLOBAL.Viewport.Height / 6) // hero is in the front
            {
                if (xDistance > _GLOBAL.Viewport.Width / 6 || xDistance < -_GLOBAL.Viewport.Width / 6) // hero is left or right
                {
                    behavior = firstBehaviorfirstGet(parent.Behaviors,
                        BehaviorState.ShootTarget,
                        BehaviorState.ShootStraightAngle,
                        BehaviorState.ShootCircle,
                        BehaviorState.ShootStraight);
                    if (_GLOBAL.Debug) Console.WriteLine("bot left/right");
                }
                else
                {
                    behavior = firstBehaviorfirstGet(parent.Behaviors,
                        BehaviorState.ShootStraight,
                        BehaviorState.ShootCircle,
                        BehaviorState.ShootStraightAngle,
                        BehaviorState.ShootTarget);
                    if (_GLOBAL.Debug) Console.WriteLine("bot mid");
                }
            }

            if (behavior != null)
            {
                behavior.Update(map, gameTime);
                lastShooting = behavior.behaviorState;
            }
        }

        internal override void Update(Map map, GameTime gameTime)
        {
            xDistance = (parent.X - map.hero.X);
            yDistance = (parent.Y - map.hero.Y);

            shoot(map, gameTime);
        }
    }
}