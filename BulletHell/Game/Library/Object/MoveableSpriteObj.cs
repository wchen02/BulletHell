﻿using System;
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
    public class MoveableSpriteObj : MoveableObj
    {
        protected int xImages, // how many images in x direction in the sprite src file
            yImages, // how many images in y direction in the sprite src file
            xCounter, // current x image of the sprite
            yCounter; // current y image of the sprite
        protected TimeSpan elapsedTime;

        public double MoveRate { get; private set; } // how fast does the sprite changes per second
        internal MoveableSpriteObj(Texture2D sprite, int x, int y, int vx, int vy, int srcRectWidth, int srcRectHeight, double moveRate)
            : base(sprite, x, y, vx, vy, srcRectWidth, srcRectHeight)
        {
            xImages = sprite.Width / srcRectWidth;
            yImages = sprite.Height / srcRectHeight;
            xCounter = 0;
            yCounter = 0;
            elapsedTime = TimeSpan.Zero;
            MoveRate = moveRate;
        }

        // IMovable
        public void move(float x, float y, Vector2 speed)
        {
            base.move(x, y, speed.X, speed.Y);

            if (x > 0) // Right
                yCounter = 2;
            else if (x < 0) // Left
                yCounter = 1;

            if (y > 0) // Down
                yCounter = 0;
            else if (y < 0) // Up
                yCounter = 3;

            // rect updated for collision detections
            srcRect.X = xCounter * Width;
            srcRect.Y = yCounter * Height;
        }

        internal override void Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime;
            if (elapsedTime > TimeSpan.FromMilliseconds(1000 / MoveRate))
            {
                elapsedTime -= TimeSpan.FromMilliseconds(1000 / MoveRate);
                xCounter++; // moves the sprite
                if (xCounter == xImages)
                    xCounter = 0;
            }
            base.Update(gameTime);
        }

        internal override void Draw(GameTime gameTime) {
            base.Draw(gameTime);
        }
    }
}
