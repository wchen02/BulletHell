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

namespace BulletHell.Game.Object
{
    public class BulletObj : MoveableObj
    {
        internal SoundEffect hitEffect; // The soundeffect to play when the bullet collide with a planeObj, is not implemented yet.
        internal bool killable, // Can the bullet be killed off
            bounceable; // Does the bullet bounce back when it hits the boundaries of the viewport
        internal Vector2 direction; // The direction the bullet is advancing

        /* These two properties were added to support Map's multi-bullet as a base */
        internal float BulletDensity { get; set; } // How dense, or in another word, how close are the bullets to another 
        internal int BulletAmount { get; set; } // How many bullets are projected at once.
        internal int BulletInterval { get; set; } // How often bullets are projected

        public BulletObj(Texture2D sprite, float x, float y, float vx, float vy, SoundEffect hitEffect, 
            bool killable, bool bounceable, float xDirection, float yDirection)
            : base(sprite, x, y, vx, vy)
        {
            this.hitEffect = hitEffect;
            this.killable = killable;
            this.bounceable = bounceable;
            direction = new Vector2(xDirection, yDirection);
        }


        internal virtual void Update(List<BulletObj> pool, Map map, GameTime gameTime)
        {
            if (!visibility)
                pool.Remove(this);

            // Advances the bullet. */
            move(direction.X, direction.Y, velocity.X, velocity.Y);

            /* Check to see if the bullet has collided with the  left and right boundaries. */
            if (bounceable &&
                (position.X > _GLOBAL.Viewport.Width - Width || position.X < _GLOBAL.Viewport.X))
                direction.X = -direction.X;
            if ((position.Y > _GLOBAL.Viewport.Height + Height || position.Y < _GLOBAL.Viewport.Y - Height
                || position.X > _GLOBAL.Viewport.Width + Width || position.X < _GLOBAL.Viewport.X - Width))
                visibility = false;
            // if killable, collision detection

            base.Update(gameTime);
        }

        internal override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
