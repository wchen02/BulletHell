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
    public class Obj {
        internal Texture2D sprite;
        internal Vector2 position;
        internal Rectangle srcRect, posRect;
        internal bool visibility;

        internal Obj(Texture2D sprite, float x, float y, int width, int height)
        {
            this.sprite = sprite;
            position = new Vector2(x, y);
            visibility = true;
            srcRect = new Rectangle(0, 0, width, height);
            posRect = new Rectangle((int)x + width / 3, (int)y + height / 3, (int)((float)2 / 3 * width), (int)((float)2 / 3 * height));
        }

        internal Obj(Texture2D sprite, float x, float y)
            : this(sprite, x, y, sprite.Width, sprite.Height) { }

        internal float X
        {
            get { return position.X; }
        }

        internal float Y
        {
            get { return position.Y; }
        }
        
        internal int Width
        {
            get { return srcRect.Width; }
        }

        internal int Height
        {
            get { return srcRect.Height; }
        }

        internal void dispose()
        {
            sprite.Dispose();
        }

        internal bool collision(Obj anotherObj)
        {
            return posRect.Intersects(anotherObj.posRect);
        }
    }
}
