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
    public class LinearBehavior : Behavior
    {
        public LinearBehavior(EnemyPlaneObj parent)
            : base(parent)
        {
            behaviorState = BehaviorState.LinearBehavior;
        }

        public override Behavior clone(EnemyPlaneObj newParent)
        {
            return new LinearBehavior(newParent);
        }

        internal override void Update(Map map, GameTime gameTime)
        {
            Vector2 difference;

            if (parent.Path.Count == 0)
                return;

            /* Moves linearly toward a destination */
            if (parent.Path.Peek().withinRange(parent.position,
                (parent.sprite.Height + parent.sprite.Width) / 4)) // reach the point
                parent.Path.Dequeue();
            else
            {
                difference = parent.Path.Peek().Point - parent.position;
                difference.Normalize();
                parent.move(difference.X, difference.Y, parent.velocity);
            }
        }
    }
}