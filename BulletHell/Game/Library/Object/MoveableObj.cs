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

namespace BulletHell.Game.Object
{
    public abstract class MoveableObj : Obj, Interface.IMoveable
    {
        internal Vector2 velocity;

        internal MoveableObj(Texture2D sprite, float x, float y, float vx, float vy, int width, int height)
            :base(sprite, x, y, width, height)
        {
            velocity = new Vector2(vx, vy);
        }

        internal MoveableObj(Texture2D sprite, float x, float y, float vx, float vy)
            : this(sprite, x, y, vx, vy, sprite.Width, sprite.Height) { }

        // IMovable
        public virtual void move(float x, float y)
        {
            position.X += x;
            position.Y += y;
        }

        // IMovable
        public void move(float x, float y, float vx, float vy)
        {
            position.X += x * vx;
            position.Y += y * vy;
        }

        internal void move(Vector2 speed)
        {
            move(velocity.X, velocity.Y, speed.X, speed.Y);
        }

        internal override void Update(GameTime gameTime)
        {
            posRect.X = (int)position.X;
            posRect.Y = (int)position.Y;
            base.Update(gameTime);
        }

        internal override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
