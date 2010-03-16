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
        internal SoundEffect hitEffect;
        internal bool killable, bounceable;
        internal Vector2 direction;

        public BulletObj(Texture2D sprite, float x, float y, float vx, float vy, SoundEffect hitEffect, 
            bool killable, bool bounceable, float xDirection, float yDirection)
            : base(sprite, x, y, vx, vy)
        {
            this.hitEffect = hitEffect;
            this.killable = killable;
            this.bounceable = bounceable;
            direction = new Vector2(xDirection, yDirection);
        }


        internal override void Update(GameTime gameTime)
        {
            move(direction.X, direction.Y, velocity.X, velocity.Y);

            if (bounceable &&
                (position.X > _GLOBAL.Viewport.Width - Width || position.X < _GLOBAL.Viewport.X))
                direction.X = -direction.X;
            if ((position.Y > _GLOBAL.Viewport.Height + Height || position.Y < _GLOBAL.Viewport.Y - Height))
                visibility = false;
            // if killable, collision detection
            base.Update(gameTime);
        }

        internal virtual void Draw(GameTime gameTime)
        {
            _GLOBAL.SpriteBatch.Draw(sprite, position, Color.White);
        }
    }
}
