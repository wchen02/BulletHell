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

namespace BulletHell
{
    public class MoveableObj : Obj, Moveable
    {
        private const int MOVE_RATE = 15;
        private Vector2 velocity;
        private uint vertical;
        private uint horizontal;
        private Vector2 movingSprite;
        private int counter = 0;

        public MoveableObj(Texture2D sprite, Vector2 position, Vector2 velocity, bool visibility, Vector2 srcRectSize)
            : base(sprite, position, visibility, srcRectSize)
        {
                this.velocity = velocity;
                vertical = (uint)(sprite.Height / srcRectSize.Y);
                horizontal = (uint)(sprite.Width / srcRectSize.X);
                movingSprite = Vector2.Zero;
        }

        public MoveableObj(Texture2D sprite, Vector2 position, Vector2 velocity, bool visibility)
            : this(sprite, position, velocity, visibility, new Vector2(sprite.Width, sprite.Height)) { }

        public MoveableObj(Texture2D sprite, Vector2 position, Vector2 velocity)
            : this(sprite, position, velocity, true) { }

        public Vector2 Velocity{
            get { return velocity; }
            protected set { velocity = value; }
        }

        public void move(Vector2 relatedPosition, Vector2 v){
            updateMove(relatedPosition, v);
        }

        public void move(int x, int y, Vector2 v){
            move(new Vector2(x, y), v);
        }

        public void move(int x, int y) {
            move(new Vector2(x, y), Velocity);
        }

        public void updateMove(Vector2 relatedPosition, Vector2 v) {
            if (relatedPosition.X > 0) {
                XPosition += relatedPosition.X * v.X;
                movingSprite.Y = 2;
            }
            else if (relatedPosition.X < 0) {
                XPosition += relatedPosition.X * v.X;
                movingSprite.Y = 1;
            }
            if (relatedPosition.Y > 0) {
                YPosition -= relatedPosition.Y * v.Y;
                movingSprite.Y = 3;
            }
            else if (relatedPosition.Y < 0) {
                YPosition -= relatedPosition.Y * v.Y;
                movingSprite.Y = 0;
            }
        }

        public void updateMoveSprite() {
            counter = (counter + 1) % MOVE_RATE;
            movingSprite.X = (movingSprite.X + counter / (MOVE_RATE-1)) % 3;
            SrcRect = new Rectangle((int)(movingSprite.X * SrcRect.Width), (int)(movingSprite.Y * SrcRect.Height), SrcRect.Width, SrcRect.Height);         
        }
    }
}
